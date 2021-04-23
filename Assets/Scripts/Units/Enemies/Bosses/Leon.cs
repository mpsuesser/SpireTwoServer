using System.Collections.Generic;

public class Leon : Boss {
    public override Enemies Type { get; protected set; }

    public override List<Ability> Abilities { get; protected set; }

    protected override void InitializeEnemy() {
        Type = Enemies.LEON;
        Health = EnemyConstants.LEON_HEALTH;
        MaxHealth = EnemyConstants.LEON_HEALTH;
        MoveSpeed = EnemyConstants.LEON_MOVE_SPEED;
        AggroRadius = EnemyConstants.LEON_AGGRO_RADIUS;
        AttackRange = EnemyConstants.LEON_ATTACK_RANGE;

        Abilities = new List<Ability>() {
            new Abilities.Leon.RighteousFury(this),
            new Abilities.Leon.RighteousDefense(this),
            new Abilities.Leon.Swing(this)
        };
    }

    protected override EnemyState GetRestingState(Enemy _e, Unit _u = null) => new RestingStateNoRoam(_e);
    protected override EnemyState GetRoamingState(Enemy _e, Unit _u = null) => new RoamingState(_e);
    protected override EnemyState GetChasingState(Enemy _e, Unit _u) => new ChasingState(_e, _u);
    protected override EnemyState GetAttackingState(Enemy _e, Unit _u) => new AttackingState(_e, _u);
    protected override EnemyState GetDeadState(Enemy _e, Unit _u = null) => new DeadState(_e);
}