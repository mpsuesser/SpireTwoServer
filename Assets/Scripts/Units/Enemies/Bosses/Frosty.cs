using System.Collections.Generic;

public class Frosty : Boss {
    public override Enemies Type { get; protected set; }

    public override List<Ability> Abilities { get; protected set; }

    protected override void InitializeEnemy() {
        Type = Enemies.FROSTY;
        Health = EnemyConstants.FROSTY_HEALTH;
        MaxHealth = EnemyConstants.FROSTY_HEALTH;
        MoveSpeed = EnemyConstants.FROSTY_MOVE_SPEED;
        AggroRadius = EnemyConstants.FROSTY_AGGRO_RADIUS;
        AttackRange = EnemyConstants.FROSTY_ATTACK_RANGE;

        Abilities = new List<Ability>() {
            new Abilities.Frosty.SpiritBomb(this),
            new Abilities.Frosty.SpiritStrike(this),
            new Abilities.Frosty.Swing(this)
        };
    }

    protected override EnemyState GetRestingState(Enemy _e, Unit _u = null) => new RestingStateNoRoam(_e);
    protected override EnemyState GetRoamingState(Enemy _e, Unit _u = null) => new RoamingState(_e);
    protected override EnemyState GetChasingState(Enemy _e, Unit _u) => new ChasingState(_e, _u);
    protected override EnemyState GetAttackingState(Enemy _e, Unit _u) => new AttackingState(_e, _u);
    protected override EnemyState GetDeadState(Enemy _e, Unit _u = null) => new DeadState(_e);
}