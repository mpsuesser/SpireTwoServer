using System.Collections;
using System.Collections.Generic;

public class Atlas : Hero {
    public override List<Ability> Abilities { get; protected set; }

    public override float HealthRegen { get; protected set; }
    public override float MaxMana { get; protected set; }
    public override float Mana { get; protected set; }
    public override float ManaRegen { get; protected set; }

    protected override void InitializeHero() {
        Abilities = new List<Ability>() {
            new Abilities.Atlas.Swing(this),
            new Abilities.Atlas.ThunderClap(this),
            new Abilities.Atlas.Leap(this),
            new Abilities.Atlas.Overload(this),
            new Abilities.Atlas.BattleTrance(this),
            new Abilities.Atlas.ThunderousFortitude(this),
            new Abilities.Atlas.Rampage(this)
        };

        MoveSpeed = HeroConstants.ATLAS_MOVE_SPEED;
        MaxHealth = HeroConstants.ATLAS_HEALTH;
        Health = HeroConstants.ATLAS_HEALTH;
        HealthRegen = HeroConstants.ATLAS_HEALTH_REGEN;
        MaxMana = HeroConstants.ATLAS_MAX_RAGE;
        Mana = 0f;
        ManaRegen = HeroConstants.ATLAS_MANA_REGEN;
    }

    // TODO: Decay rage over time
    protected override IEnumerator RegenMana() {
        yield break;
    }
}
