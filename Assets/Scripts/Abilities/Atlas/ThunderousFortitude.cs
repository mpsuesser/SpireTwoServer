using UnityEngine;

namespace Abilities {
    namespace Atlas {
        public class ThunderousFortitude : HeroAbility {
            public ThunderousFortitude(Hero _h) : base(_h, AbilityID.AtlasThunderousFortitude) {
                Cooldown = AbilityConstants.THUNDEROUS_FORTITUDE_COOLDOWN;
                ManaCost = AbilityConstants.THUNDEROUS_FORTITUDE_RAGE_COST;
            }

            public override void Execute() {
                H.ExpendMana(AdjustedManaCost);

                Debug.Log("Adding ThunderousFortitude buff!");
                U.AddBuff(new ThunderousFortitudeBuff(U));

                Done();
            }
        }
    }
}
