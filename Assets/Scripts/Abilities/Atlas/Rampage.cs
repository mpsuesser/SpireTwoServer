using UnityEngine;

namespace Abilities {
    namespace Atlas {
        public class Rampage : HeroAbility {

            public Rampage(Hero _h) : base(_h, AbilityID.AtlasRampage) {}

            public override void Execute() {
                Debug.Log("Adding Rampage buff!");
                U.AddBuff(new RampageBuff(U));

                Done();
            }
        }
    }
}
