using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    public static PrefabManager instance;

    void Awake() {
        #region Singleton
        if (instance != null) {
            Debug.LogError("More than one PrefabManager instance in scene!");
            return;
        }

        instance = this;
        #endregion
    }

    // Heroes
    public GameObject atlasPrefab;
    public GameObject priestessPrefab;

    // Enemies
    public GameObject gruntPrefab;
    public GameObject spellslingerPrefab;
    public GameObject leonPrefab;

    // Dicts
    public Dictionary<Heroes, GameObject> HeroPrefabs { get; private set; }
    public Dictionary<Enemies, GameObject> EnemyPrefabs { get; private set; }

    void Start() {
        HeroPrefabs = new Dictionary<Heroes, GameObject>() {
            { Heroes.ATLAS, atlasPrefab },
            { Heroes.PRIESTESS, priestessPrefab }
        };

        EnemyPrefabs = new Dictionary<Enemies, GameObject>() {
            { Enemies.GRUNT, gruntPrefab },
            { Enemies.SPELLSLINGER, spellslingerPrefab },
            { Enemies.LEON, leonPrefab }
        };
    }
}
