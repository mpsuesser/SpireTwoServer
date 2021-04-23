using UnityEngine;
using System.Collections;

namespace Abilities {
    namespace Atlas {
        public class Swing : HeroAbility {
            public Swing(Hero _h) : base(_h, AbilityID.AtlasSwing) {}

            private static float Radius = AbilityConstants.SWING_RADIUS;
            private static float Range = AbilityConstants.SWING_RANGE;
            private static float Damage = AbilityConstants.SWING_DAMAGE;
            private static int EnemyLayerMask = EnemyConstants.ENEMY_LAYER_MASK;
            public override void Execute() {
                Vector3 origin = U.gameObject.transform.position;
                Vector3 direction = U.gameObject.transform.forward.normalized;

                RaycastHit[] hits = Physics.SphereCastAll(origin, Radius, direction, Range, EnemyLayerMask);
                if (hits.Length > 0) {
                    // Loop through all enemies hit by the spherecast and inflict swing damage
                    foreach (RaycastHit hit in hits) {
                        Enemy e = hit.transform.gameObject.GetComponent<Enemy>();
                        U.DealDamage(e, Damage);
                    }
                    
                    H.RestoreMana(GetRageGain(hits.Length));
                }

                Done();
            }

            private float GetRageGain(int _enemiesHit) {
                // Initial gain
                float gain = AbilityConstants.SWING_RAGE_GAIN_SINGLE_HIT + ((_enemiesHit - 1) * AbilityConstants.SWING_RAGE_GAIN_ADDITIONAL_ENEMIES);

                // Apply any modifiers from buffs
                foreach (Buff b in U.Buffs) {
                    gain = b.ManaGainMultiplier(gain);
                }

                return gain;
            }
        }
    }
}
