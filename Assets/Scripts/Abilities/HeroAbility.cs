using UnityEngine;
using System.Collections;

public abstract class HeroAbility : Ability {
    public Hero H { get; private set; }
    public AbilityID ID { get; set; }

    public HeroAbility(Hero _h, AbilityID _id) : base(_h) {
        H = _h;
        ID = _id;
    }

    public override void Done() {
        PutOnCooldown(H);

        ServerSend.AbilityFired(H.ID, ID, Cooldown);
    }

    public override bool Available() {
        if (AdjustedManaCost > H.Mana) {
            return false;
        }

        return base.Available();
    }
}