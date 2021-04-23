using UnityEngine;
using System.Collections;

namespace Abilities {
    namespace Atlas {
        public class Leap : HeroAbility {
            public Leap(Hero _h) : base(_h, AbilityID.AtlasLeap) {
                Cooldown = AbilityConstants.LEAP_COOLDOWN;
                ManaGain = AbilityConstants.LEAP_RAGE_GAIN;
            }

            public override void Execute() {
                H.RestoreMana(AdjustedManaGain);
                
                DoLeap();

                Done();
            }

            private void DoLeap() {
                H.SetJumpFlags();
                Vector3 fw = H.gameObject.transform.forward;

                float power = Mathf.Clamp(fw.y, AbilityConstants.LEAP_MIN_POWER, AbilityConstants.LEAP_MAX_POWER);

                // Vertical force
                float verticalPowerFactor = power / AbilityConstants.LEAP_MAX_POWER;
                H.RB.AddForce(Vector3.up * verticalPowerFactor * AbilityConstants.LEAP_VERTICAL_POWER, ForceMode.VelocityChange);

                // Horizontal force, variable by Y rotation dir
                Vector3 leapDir = new Vector3(fw.x, 0f, fw.z).normalized;
                H.RB.AddForce(leapDir * power *  AbilityConstants.LEAP_HORIZONTAL_POWER, ForceMode.VelocityChange);

                IEnumerator smashRoutine = Smash(H, AbilityConstants.LEAP_DURATION);
                CoroutineRunner.instance.Run(smashRoutine);
            }

            private IEnumerator Smash(Hero _h, float _peakTime) {
                yield return new WaitForSeconds(_peakTime);

                _h.RB.useGravity = false;
                while(_h.Jumping) {
                    _h.RB.velocity = new Vector3(_h.RB.velocity.x, -AbilityConstants.LEAP_SMASH_POWER, _h.RB.velocity.z);
                    yield return null;
                }

                _h.RB.useGravity = true;

                Landed();
            }

            private void Landed() {
                float radius = AbilityConstants.LEAP_DAMAGE_RADIUS;
                int enemyLayerMask = EnemyConstants.ENEMY_LAYER_MASK;
                Collider[] hits = Physics.OverlapSphere(U.gameObject.transform.position, radius, enemyLayerMask);
                if (hits.Length > 0) {
                    // Loop through all enemies close enough at the leap destination and inflict damage
                    foreach (Collider hit in hits) {
                        Enemy e = hit.gameObject.GetComponent<Enemy>();
                        U.DealDamage(e, AbilityConstants.LEAP_DAMAGE);
                    }
                }
            }
        }
    }
}
