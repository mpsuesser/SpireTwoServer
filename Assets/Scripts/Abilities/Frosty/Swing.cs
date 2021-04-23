using UnityEngine;
using System.Collections;

namespace Abilities {
    namespace Frosty {
        public class Swing : EnemyAbility {
            public Swing(Enemy _e) : base(_e) {
                Cooldown = EnemyConstants.FROSTY_SWING_FREQUENCY;
            }

            public override void Execute() {
                if (E.State.ID != EnemyStateID.Attacking) {
                    Debug.Log("State was not Attacking when Frosty swing was to be executed!");
                    return;
                }

                Unit target = (E.State as AttackingState).Attacking;
                ServerSend.EnemyAbilityFired(E.ID, AbilityID.FrostySwing);
                CoroutineRunner.instance.Run(DoSwing(target));

                Done();
            }

            private IEnumerator DoSwing(Unit _target) {
                yield return new WaitForSeconds(EnemyConstants.FROSTY_SWING_SPEED);

                if (E != null && _target != null) {
                    E.DealDamage(_target, EnemyConstants.FROSTY_SWING_DAMAGE);
                }
            }
        }
    }
}
