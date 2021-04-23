using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Abilities {
    namespace Frosty {
        public class SpiritBomb : EnemyAbility {
            private static float[] PhasePercentages = EnemyConstants.FROSTY_SPIRIT_BOMB_PERCENTAGES;
            private bool[] PhaseCompleted = new bool[PhasePercentages.Length];

            public SpiritBomb(Enemy _e) : base(_e) {
                if (PhaseCompleted.Length != PhasePercentages.Length) {
                    Debug.Log("Frosty phase status trackers are not aligned in length! Should not happen!");
                    return;
                }

                Reset();
            }

            public override bool Available() {
                if (E.IsGCDActive) {
                    return false;
                }

                for (int i = 0; i < PhaseCompleted.Length; i++) {
                    if (PhaseCompleted[i] == false && E.HealthPercentage < PhasePercentages[i]) {
                        return true;
                    }
                }

                return false;
            }

            public override void Execute() {
                // First mark this phase as consumed
                for (int i = 0; i < PhaseCompleted.Length; i++) {
                    if (PhaseCompleted[i] == false) {
                        PhaseCompleted[i] = true;
                        break;
                    }
                }

                // Let the client know the phase is beginning
                ServerSend.EnemyAbilityFired(E.ID, AbilityID.FrostySpiritBombStartPhase);

                // Begin the phase
                CoroutineRunner.instance.Run(BeginPhase());
            }

            private IEnumerator BeginPhase() {
                // Set casting so we don't do anything else or move around
                E.Casting = true;

                // Apply debuff to take more damage
                E.AddBuff(new SpiritBombDebuff(E));

                // Spawn bombs until one has been soaked
                int bombCount = 0;
                bool hasBeenSoaked = false;
                while (hasBeenSoaked == false) {
                    if (bombCount == 0) {
                        yield return new WaitForSeconds(EnemyConstants.FROSTY_SPIRIT_BOMB_INITIAL_DELAY);
                    } else {
                        yield return new WaitForSeconds(EnemyConstants.FROSTY_SPIRIT_BOMB_SUBSEQUENT_DELAY);
                    }

                    // Spawn the bomb in a separate coroutine with a callback function which sets our sentinel in this loop.
                    yield return CoroutineRunner.instance.Run(
                        SpawnBomb(
                            bombCount,
                            (_wasSoaked) => hasBeenSoaked = _wasSoaked
                        )
                    );

                    bombCount++;
                }

                // Remove debuff
                E.PurgeBuffOfType<SpiritBombDebuff>();

                // End the phase
                ServerSend.EnemyAbilityFired(E.ID, AbilityID.FrostySpiritBombEndPhase);

                // End our casting so Frosty goes back to casting other spells
                E.Casting = false;
            }

            private IEnumerator SpawnBomb(int _bombCount, System.Action<bool> _soakedCallback) {
                // Pick a random direction, make sure it's in LoS, store the location
                Vector3 bombLocation = Util.GetRandomPointInRangeNotBlocked(E.transform.position, EnemyConstants.FROSTY_SPIRIT_BOMB_MAX_DISTANCE, Pathfinding.instance);

                // Send the spawn event to the client
                ServerSend.EnemyAbilityFired(E.ID, AbilityID.FrostySpiritBombSpawn, null, new List<Vector3>() { bombLocation });

                // Wait for bomb time
                yield return new WaitForSeconds(EnemyConstants.FROSTY_SPIRIT_BOMB_TIME);

                // Check if heroes are in the bomb radius
                // If any are, deal damage to them, return true to callback
                // If not, deal damage to all heroes based on bomb count multiplier
                Collider[] heroColliders = Physics.OverlapSphere(bombLocation, EnemyConstants.FROSTY_SPIRIT_BOMB_RADIUS, Constants.HERO_LAYER_MASK);
                if (heroColliders.Length > 0) {
                    List<int> heroIdsAffected = new List<int>();
                    foreach (Collider collider in heroColliders) {
                        Hero h = collider.gameObject.GetComponent<Hero>();
                        heroIdsAffected.Add(h.OwnerID);
                        E.DealDamage(h, EnemyConstants.FROSTY_SPIRIT_BOMB_SOAK_DAMAGE);
                    }

                    // Let the client know it exploded
                    ServerSend.EnemyAbilityFired(E.ID, AbilityID.FrostySpiritBombExplode, heroIdsAffected);

                    // Update bool in original coroutine via callback
                    _soakedCallback(true);
                } else {
                    // Calculate damage to deal based on bombCount
                    float damage = EnemyConstants.FROSTY_SPIRIT_BOMB_BASE_AOE_DAMAGE;
                    if (_bombCount > 0) {
                        damage *= Mathf.Pow(EnemyConstants.FROSTY_SPIRIT_BOMB_AOE_MULTIPLIER, _bombCount);
                    }

                    // Deal the damage to each hero
                    foreach (Hero h in GameState.instance.AliveHeroes) {
                        E.DealDamage(h, damage);
                    }

                    // Let the client know it exploded
                    ServerSend.EnemyAbilityFired(E.ID, AbilityID.FrostySpiritBombExplode);

                    // Update bool in original coroutine via callback
                    _soakedCallback(false);
                }

                yield return null;
            }

            public override void Reset() {
                for (int i = 0; i < PhaseCompleted.Length; i++) {
                    PhaseCompleted[i] = false;
                }
            }
        }
    }
}
