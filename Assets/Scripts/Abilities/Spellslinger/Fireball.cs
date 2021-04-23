using UnityEngine;
using System.Collections;

namespace Abilities {
    namespace Spellslinger {
        public class Fireball : EnemyAbility {
            public Fireball(Enemy _e) : base(_e) {
                Cooldown = EnemyConstants.SPELLSLINGER_FIREBALL_COOLDOWN;
            }

            public override void Execute() {
                if (E.State.ID != EnemyStateID.Attacking) {
                    Debug.Log("State was not Attacking when grunt swing was to be executed!");
                    return;
                }

                Unit target = (E.State as AttackingState).Attacking;
                CoroutineRunner.instance.StartCoroutine(Cast(target));

                Done();
            }

            private IEnumerator Cast(Unit _target) {
                E.Casting = true;
                ServerSend.EnemyAbilityFired(E.ID, AbilityID.SpellslingerFireballCastStart);

                yield return new WaitForSeconds(EnemyConstants.SPELLSLINGER_FIREBALL_CAST_TIME);

                if (E == null) {
                    yield break;
                }

                E.Casting = false;

                if (E.State.ID == EnemyStateID.Dead) {
                    yield break;
                }

                if (_target == null) {
                    yield break;
                }

                ServerSend.EnemyAbilityFired(E.ID, AbilityID.SpellslingerFireballShoot);
                yield return ShootFireball(_target);
            }

            private IEnumerator ShootFireball(Unit _target) {
                Vector3 approxFireballLocation = E.transform.position;
                Vector3 dirToTarget;

                while ((approxFireballLocation - _target.transform.position).magnitude > 3f) {
                    if (E == null || _target == null) {
                        yield break;
                    }

                    dirToTarget = (_target.transform.position - approxFireballLocation).normalized;
                    approxFireballLocation += dirToTarget * (Constants.SPELL_PROJECTILE_SPEED * Time.deltaTime);

                    yield return null;
                }

                E.DealDamage(_target, EnemyConstants.SPELLSLINGER_FIREBALL_DAMAGE);
            }
        }
    }
}
