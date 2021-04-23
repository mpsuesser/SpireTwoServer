using UnityEngine;
using System.Collections;

namespace Abilities {
    namespace Atlas {
        public class ThunderClap : HeroAbility {
            public ThunderClap(Hero _h) : base(_h, AbilityID.AtlasThunderClap) {
                ManaGain = AbilityConstants.THUNDER_CLAP_RAGE_GAIN;
                Cooldown = AbilityConstants.THUNDER_CLAP_COOLDOWN;
            }

            private static float Delay = AbilityConstants.THUNDER_CLAP_DELAY;
            public override void Execute() {
                CoroutineRunner.instance.Run(DelayedSmash(Delay));

                Done();
            }

            private static float Radius = AbilityConstants.THUNDER_CLAP_RADIUS;
            private static float Range = AbilityConstants.THUNDER_CLAP_RANGE;
            private static float Damage = AbilityConstants.THUNDER_CLAP_DAMAGE;
            private static int EnemyLayerMask = EnemyConstants.ENEMY_LAYER_MASK;
            private IEnumerator DelayedSmash(float _delay) {
                yield return new WaitForSeconds(_delay);

                Vector3 origin = U.gameObject.transform.position;
                Vector3 direction = U.gameObject.transform.forward.normalized;

                RaycastHit[] hits = Physics.SphereCastAll(origin, Radius, direction, Range, EnemyLayerMask);
                if (hits.Length > 0) {
                    // Loop through all enemies hit by the spherecast and inflict swing damage
                    foreach (RaycastHit hit in hits) {
                        Enemy e = hit.transform.gameObject.GetComponent<Enemy>();
                        U.DealDamage(e, Damage);
                        e.AddBuff(new ThunderClapDebuff(e));
                        e.Interrupt();
                    }

                    H.RestoreMana(AdjustedManaGain);
                }
            }
        }
    }
}
