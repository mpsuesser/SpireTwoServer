using UnityEngine;
using System.Collections;

namespace Abilities {
    namespace Leon {
        public class RighteousFury : EnemyAbility {
            public RighteousFury(Enemy _e) : base(_e) {
                Cooldown = EnemyConstants.LEON_RIGHTEOUS_FURY_COOLDOWN;
            }

            public override void Execute() {
                ServerSend.EnemyAbilityFired(E.ID, AbilityID.LeonRighteousFury);
                CoroutineRunner.instance.StartCoroutine(DoAction());

                SetMinimumCooldownOnRighteousDefense();

                Done();
            }

            private IEnumerator DoAction() {
                yield return new WaitForSeconds(EnemyConstants.LEON_RIGHTEOUS_FURY_DELAY);

                U.PurgeBuffOfType<RighteousDefenseBuff>();
                U.AddBuff(new RighteousFuryBuff(E));
            }

            private void SetMinimumCooldownOnRighteousDefense() {
                foreach (Ability _a in E.Abilities) {
                    if (_a is Abilities.Leon.RighteousDefense rd) {
                        if (!rd.OnCooldown) {
                            rd.SetDelayCooldown();
                        }
                    }
                }
            }
        }
    }
}
