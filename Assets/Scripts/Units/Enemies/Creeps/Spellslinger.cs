using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spellslinger : Enemy
{
    public override Enemies Type { get; protected set; }

    public override List<Ability> Abilities { get; protected set; }

    protected override void InitializeEnemy() {
        Type = Enemies.SPELLSLINGER;
        Health = EnemyConstants.SPELLSLINGER_HEALTH;
        MaxHealth = EnemyConstants.SPELLSLINGER_HEALTH;
        MoveSpeed = EnemyConstants.SPELLSLINGER_MOVE_SPEED;
        AggroRadius = EnemyConstants.SPELLSLINGER_AGGRO_RADIUS;
        AttackRange = EnemyConstants.SPELLSLINGER_ATTACK_RANGE;

        Abilities = new List<Ability>() {
            new Abilities.Spellslinger.Fireball(this)
        };
    }

    protected override EnemyState GetRestingState(Enemy _e, Unit _u = null) => new RestingState(_e);
    protected override EnemyState GetRoamingState(Enemy _e, Unit _u = null) => new RoamingState(_e);
    protected override EnemyState GetChasingState(Enemy _e, Unit _u) => new ChasingState(_e, _u);
    protected override EnemyState GetAttackingState(Enemy _e, Unit _u) => new AttackingState(_e, _u);
    protected override EnemyState GetDeadState(Enemy _e, Unit _u = null) => new DeadState(_e);
}
