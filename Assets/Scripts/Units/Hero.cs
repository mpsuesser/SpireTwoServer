using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hero : Unit
{
    // Identifiers
    public int OwnerID { get; private set; }

    // Abilities
    public override abstract List<Ability> Abilities { get; protected set; }

    // Gameplay Properties
    public abstract float HealthRegen { get; protected set; }
    public abstract float MaxMana { get; protected set; }
    public abstract float Mana { get; protected set; }
    public abstract float ManaRegen { get; protected set; }

    // Status Trackers
    public bool Jumping { get; private set; }
    protected override float GCDTime { get; set; }

    private Coroutine healthRegenCoroutine;
    private Coroutine manaRegenCoroutine;

    // Misc, private references
    private float lastJump;

    private Movement.Direction inputDirection;
    private Vector3 inputRotation; // euler angles
    
    protected override void Initialize() {
        #region Initializing Class Members
        Jumping = false;
        lastJump = Time.time;
        inputDirection = Movement.Direction.NONE;
        inputRotation = transform.eulerAngles;
        GCDTime = AbilityConstants.HERO_GCD_TIME;
        #endregion

        AfterDamage += Damaged;

        InitializeHero();
    }

    protected abstract void InitializeHero();

    private void Update() {}

    private void FixedUpdate() {
        // First update the rotation so that our velocity can be properly calculated with rotation taken into account.
        Vector3 flattenedRotation = new Vector3(0f, inputRotation.y, 0f);
        transform.eulerAngles = flattenedRotation;

        // Used for figuring out the client-side animation, has no use here
        float moveSpeedRatio = 0f;

        // Only calculate new movement if we're not in the air. If we are, just skip the velocity change and let us float up/down in the direction we initially jumped in.
        if (!Jumping) {
            // Update the velocity based on rotation and WASD input.
            if (inputDirection == Movement.Direction.NONE) {
                Stop();
            } else {
                Vector3 dir = new Vector3(0f, 0f, 0f);
                if ((inputDirection & Movement.Direction.FORWARD) != 0) dir += transform.forward;
                if ((inputDirection & Movement.Direction.RIGHT) != 0) dir += transform.right;
                if ((inputDirection & Movement.Direction.LEFT) != 0) dir -= transform.right;
                if ((inputDirection & Movement.Direction.BACKWARD) != 0) dir -= transform.forward;

                dir.y = 0f;
                RB.velocity = dir.normalized * AdjustedMoveSpeed;

                moveSpeedRatio = AdjustedMoveSpeed / MoveSpeed;
            }
        }

        ServerSend.HeroPositionUpdate(this);

        Debug.DrawRay(transform.position, transform.forward * 2, Color.red);
    }

    private void Stop() {
        RB.velocity = Vector3.zero;
    }

    public void SetDirection(Movement.Direction _dir) {
        inputDirection = _dir;
    }

    public void SetRotation(Vector3 _eulerAngles) {
        inputRotation = _eulerAngles;
    }

    public void SetOwnerID(int _id) {
        OwnerID = _id;
    }

    public void Jump() {
        // Do not jump if we are already in the process of jumping or falling.
        if (Jumping) {
            return;
        }

        RB.AddForce(Vector3.up * HeroConstants.JUMP_STRENGTH, ForceMode.VelocityChange);
        SetJumpFlags();
    }

    public void SetJumpFlags() {
        Jumping = true;
        lastJump = Time.time;
    }

    public void AbilityUsed(AbilityID _abilityID) {
        Ability ability = GetAbilityByID(_abilityID);

        if (ability == null) {
            Debug.Log($"Unable to find ability with ID {_abilityID} for hero {gameObject.name}!");

            return;
        }

        if (!ability.Available()) {
            Debug.Log("Ability is not available!");

            return;
        }

        ability.Execute();
    }

    public bool Heal(float _amt) {
        Health = Mathf.Min(Health + _amt, MaxHealth);

        ServerSend.HeroStatusUpdate(this);

        return Health == MaxHealth;
    }

    public void Damaged(float _amt) {
        ServerSend.HeroStatusUpdate(this);

        if (healthRegenCoroutine == null) {
            healthRegenCoroutine = StartCoroutine(RegenHealth());
        }
    }

    public bool RestoreMana(float _amt) {
        Mana = Mathf.Min(Mana + _amt, MaxMana);
        Debug.Log($"Mana set to {Mana}");

        ServerSend.HeroStatusUpdate(this);

        if (manaRegenCoroutine == null) {
            manaRegenCoroutine = StartCoroutine(RegenMana());
        }

        return Mana == MaxMana;
    }

    public void ExpendMana(float _amt) {
        if (_amt > Mana) {
            Debug.Log("Overused mana! This should not happen!");
            return;
        }

        Mana -= _amt;
        ServerSend.HeroStatusUpdate(this);
        
        if (manaRegenCoroutine == null) {
            manaRegenCoroutine = StartCoroutine(RegenMana());
        }
    }

    private IEnumerator RegenHealth() {
        float regenInterval = HeroConstants.REGEN_INTERVAL;
        yield return new WaitForSeconds(regenInterval);

        while (Health < MaxHealth) {
            // If we heal to full, then stop.
            if (Heal(HealthRegen)) {
                break;
            }

            yield return new WaitForSeconds(regenInterval);
        }

        healthRegenCoroutine = null;
    }

    protected virtual IEnumerator RegenMana() {
        float regenInterval = HeroConstants.REGEN_INTERVAL;
        yield return new WaitForSeconds(regenInterval);

        while (Mana < MaxMana) {
            // If we have max mana, then stop.
            if (RestoreMana(ManaRegen)) {
                break;
            }

            yield return new WaitForSeconds(regenInterval);
        }

        manaRegenCoroutine = null;
    }

    #region Collisions
    private void OnCollisionEnter(Collision _collision) {
        if (Jumping && Util.IsCollisionWithGround(_collision)) {
            Jumping = false;
        }
    }

    private void OnCollisionStay(Collision _collision) {
        // If we have triggered our jump recently, within the buffer time, ignore this OnCollisionStay call, to allow us to actually leave the collision on jump initiation.
        if (Jumping && Util.IsCollisionWithGround(_collision) && (Time.time - lastJump > HeroConstants.JUMP_BUFFER)) {
            Jumping = false;
        }
    }

    private void OnCollisionExit(Collision _collision) {
        if (Util.IsCollisionWithGround(_collision)) {
            Jumping = true;
        }
    }
    #endregion
    #region Triggers
    private void OnTriggerEnter(Collider _other) {
        switch (_other.gameObject.tag) {
            case "StartTrigger":
                EventHandler.instance.StartTriggered(this);
                break;

            case "EndTrigger":
                EventHandler.instance.EndTriggered(this);
                break;
        }
    }
    #endregion

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1.5f);
    }

    private Ability GetAbilityByID(AbilityID _id) {
        foreach (Ability a in Abilities) {
            if (a is HeroAbility ha && ha.ID == _id) {
                return a;
            }
        }

        return null;
    }
}
