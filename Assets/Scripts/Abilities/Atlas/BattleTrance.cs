using UnityEngine;

namespace Abilities {
    namespace Atlas {
        public class BattleTrance : HeroAbility {
            public BattleTrance(Hero _h) : base(_h, AbilityID.AtlasBattleTrance) {
                Cooldown = AbilityConstants.BATTLE_TRANCE_COOLDOWN;
                ManaCost = AbilityConstants.BATTLE_TRANCE_RAGE_COST;
            }

            public override void Execute() {
                H.ExpendMana(AdjustedManaCost);

                Debug.Log("Adding BattleTrance buff!");
                U.AddBuff(new BattleTranceBuff(U));

                Done();
            }
        }
    }
}
