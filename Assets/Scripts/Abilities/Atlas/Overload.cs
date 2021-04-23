using UnityEngine;

namespace Abilities {
    namespace Atlas {
        public class Overload : HeroAbility {
            private static float AdditionalRageCost = AbilityConstants.OVERLOAD_ADDITIONAL_RAGE_COST;
            private float AdjustedMaxManaCost {
                get {
                    float ac = AdditionalRageCost;
                    foreach (Buff b in U.Buffs) {
                        ac = b.AbilityCostMultiplier(ac);
                    }

                    return AdjustedManaCost + ac;
                }
            }

            public Overload(Hero _h) : base(_h, AbilityID.AtlasOverload) {
                Cooldown = AbilityConstants.OVERLOAD_COOLDOWN;
                ManaCost = AbilityConstants.OVERLOAD_MIN_RAGE_COST;
            }

            private static float Radius = AbilityConstants.OVERLOAD_RADIUS;
            private static int EnemyLayerMask = EnemyConstants.ENEMY_LAYER_MASK;
            public override void Execute() {
                // How much mana are we actually spending on this? We have a min and a max, so we need to calculate it
                float cost = Mathf.Min(H.Mana, AdjustedMaxManaCost);
                H.ExpendMana(cost);

                // Will be a number between 0 and 1, inclusive.
                float powerRatio = (cost - AdjustedManaCost) / (AdjustedMaxManaCost - AdjustedManaCost);

                Collider[] hits = Physics.OverlapSphere(U.gameObject.transform.position, Radius, EnemyLayerMask);
                if (hits.Length > 0) {
                    foreach (Collider hit in hits) {
                        Enemy e = hit.gameObject.GetComponent<Enemy>();
                        InflictOverloadEffectOnEnemy(e, powerRatio);
                    }
                }

                Done();
            }

            private static float MinDamage = AbilityConstants.OVERLOAD_MIN_DAMAGE;
            private static float MaxDamage = AbilityConstants.OVERLOAD_MAX_DAMAGE;
            private static float MinKnockback = AbilityConstants.OVERLOAD_MIN_KNOCK_POWER;
            private static float MaxKnockback = AbilityConstants.OVERLOAD_MAX_KNOCK_POWER;
            private void InflictOverloadEffectOnEnemy(Enemy _e, float _powerRatio) {
                // Damage
                float damage = MinDamage + ((MaxDamage - MinDamage) * _powerRatio);
                U.DealDamage(_e, damage);

                // Knockback
                float knockPower = MinKnockback + ((MaxKnockback - MinKnockback) * _powerRatio);
                Vector3 knockDir = (_e.gameObject.transform.position - U.gameObject.transform.position).normalized + Vector3.up;
                _e.RB.AddForce(knockDir * knockPower, ForceMode.VelocityChange);
            }
        }
    }
}
