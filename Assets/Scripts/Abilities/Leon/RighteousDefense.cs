using UnityEngine;
using System.Collections;

namespace Abilities {
    namespace Leon {
        public class RighteousDefense : EnemyAbility {
            public RighteousDefense(Enemy _e) : base(_e) {
                Cooldown = EnemyConstants.LEON_RIGHTEOUS_DEFENSE_COOLDOWN;
            }

            public override void Execute() {
                ServerSend.EnemyAbilityFired(E.ID, AbilityID.LeonRighteousDefense);
                CoroutineRunner.instance.StartCoroutine(DoAction());

                Done();
            }

            private static float Delay = EnemyConstants.LEON_RIGHTEOUS_DEFENSE_DELAY;
            private static float SlamRadius = EnemyConstants.LEON_RIGHTEOUS_DEFENSE_TRANSITION_DAMAGE_RADIUS;
            private static float SlamDamage = EnemyConstants.LEON_RIGHTEOUS_DEFENSE_TRANSITION_DAMAGE;
            private static int HeroLayerMask = Constants.HERO_LAYER_MASK;
            private IEnumerator DoAction() {
                // Delay
                yield return new WaitForSeconds(Delay);

                // Slam damage
                Vector3 origin = E.transform.position;
                Collider[] hits = Physics.OverlapSphere(origin, SlamRadius, HeroLayerMask);
                if (hits.Length > 0) {
                    foreach (Collider hit in hits) {
                        Hero h = hit.gameObject.GetComponent<Hero>();
                        E.DealDamage(h, SlamDamage);
                    }
                }

                // Buffs
                U.PurgeBuffOfType<RighteousFuryBuff>();
                U.AddBuff(new RighteousDefenseBuff(E));
            }

            // Called as part of RighteousFury's execution.
            public void SetDelayCooldown() {
                PutOnCooldown(true, EnemyConstants.LEON_RIGHTEOUS_DEFENSE_MINIMUM_SPACING_TIME);
            }
        }
    }
}
