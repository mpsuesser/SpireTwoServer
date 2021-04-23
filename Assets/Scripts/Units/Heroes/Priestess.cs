using System.Collections.Generic;

public class Priestess : Hero {
    public override List<Ability> Abilities { get; protected set; }

    public override float HealthRegen { get; protected set; }
    public override float MaxMana { get; protected set; }
    public override float Mana { get; protected set; }
    public override float ManaRegen { get; protected set; }

    protected override void InitializeHero() {
        Abilities = new List<Ability>() {
            new Abilities.Atlas.Leap(this),
            new Abilities.Atlas.Leap(this),
            new Abilities.Atlas.Leap(this),
            new Abilities.Atlas.Leap(this),
            new Abilities.Atlas.Leap(this),
            new Abilities.Atlas.Leap(this),
            new Abilities.Atlas.Leap(this)
        };

        MoveSpeed = HeroConstants.PRIESTESS_MOVE_SPEED;
        MaxHealth = HeroConstants.PRIESTESS_HEALTH;
        Health = HeroConstants.PRIESTESS_HEALTH;
        HealthRegen = HeroConstants.PRIESTESS_HEALTH_REGEN;
        MaxMana = HeroConstants.PRIESTESS_MAX_MANA;
        Mana = HeroConstants.PRIESTESS_MAX_MANA;
        ManaRegen = HeroConstants.PRIESTESS_MANA_REGEN;
    }
}
