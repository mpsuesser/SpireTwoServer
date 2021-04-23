using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Abilities {
    namespace Frosty {
        public class SpiritStrike : EnemyAbility {
            public SpiritStrike(Enemy _e) : base(_e) {
                Cooldown = EnemyConstants.FROSTY_SPIRIT_STRIKE_COOLDOWN;
            }

            public override void Execute() {
                if (E.State.ID != EnemyStateID.Attacking) {
                    Debug.Log("State was not Attacking when Frosty spirit strike was to be executed!");
                    return;
                }

                Hero target = GameState.instance.GetRandomAliveHero();
                if (target == null) {
                    Debug.Log("Frosty went to Ion Strike but there was no hero alive! Should not happen!");
                    return;
                }

                CoroutineRunner.instance.Run(DoStrike(target));

                Done();
            }

            private IEnumerator DoStrike(Hero _target) {
                // Set casting so we don't move or cast another ability
                E.Casting = true;

                // Capture the location of our target for the strike
                Vector3 strikeLocation = _target.transform.position;

                // Send info to client about ability being fired
                List<int> _affectedUnitIDs = new List<int>() { _target.OwnerID };
                List<Vector3> _locations = new List<Vector3>() { strikeLocation };
                ServerSend.EnemyAbilityFired(E.ID, AbilityID.FrostySpiritStrike, _affectedUnitIDs, _locations);

                // Wait for the time it takes for the strike to come down...
                yield return new WaitForSeconds(EnemyConstants.FROSTY_SPIRIT_STRIKE_DELAY);

                // Strike in that location, damaging any units standing there
                Vector3 point0 = new Vector3(strikeLocation.x, strikeLocation.y - 2f, strikeLocation.z);
                Vector3 point1 = new Vector3(strikeLocation.x, strikeLocation.y + 4f, strikeLocation.z);
                Collider[] heroColliders = Physics.OverlapCapsule(point0, point1, EnemyConstants.FROSTY_SPIRIT_STRIKE_WIDTH, Constants.HERO_LAYER_MASK);
                foreach (Collider collider in heroColliders) {
                    E.DealDamage(collider.gameObject.GetComponent<Hero>(), EnemyConstants.FROSTY_SPIRIT_STRIKE_DAMAGE);
                }

                // Unset casting so we can get on with other abilities while spirit strike is on cooldown.
                E.Casting = false;
            }
        }
    }
}
