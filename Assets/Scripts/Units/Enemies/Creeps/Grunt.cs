using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grunt : Enemy
{
    public override Enemies Type { get; protected set; }

    public override List<Ability> Abilities { get; protected set; }

    protected override void InitializeEnemy() {
        Type = Enemies.GRUNT;
        Health = EnemyConstants.GRUNT_HEALTH;
        MaxHealth = EnemyConstants.GRUNT_HEALTH;
        MoveSpeed = EnemyConstants.GRUNT_MOVE_SPEED;
        AggroRadius = EnemyConstants.GRUNT_AGGRO_RADIUS;
        AttackRange = EnemyConstants.GRUNT_ATTACK_RANGE;

        Abilities = new List<Ability>() {
            new Abilities.Grunt.Swing(this)
        };
    }

    protected override EnemyState GetRestingState(Enemy _e, Unit _u = null) => new RestingState(_e);
    protected override EnemyState GetRoamingState(Enemy _e, Unit _u = null) => new RoamingState(_e);
    protected override EnemyState GetChasingState(Enemy _e, Unit _u) => new ChasingState(_e, _u);
    protected override EnemyState GetAttackingState(Enemy _e, Unit _u) => new AttackingState(_e, _u);
    protected override EnemyState GetDeadState(Enemy _e, Unit _u = null) => new DeadState(_e);
}
