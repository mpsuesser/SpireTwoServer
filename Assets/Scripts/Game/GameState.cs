using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    public static GameState instance;

    public DungeonID Dungeon { get; private set; }
    public DungeonDetails DD { get; private set; }

    // To determine if the game is started and therefore locked to new players.
    public bool HasStarted { private set; get; }
    public bool HasEnded { private set; get; }
    public int PlayersConnected { private set; get; }

    public List<Hero> HeroesSpawned { private set; get; }
    public List<Hero> AliveHeroes => HeroesSpawned.FindAll(h => h.Dead == false);
    public Hero P1 => HeroesSpawned.Count < 1 ? null : HeroesSpawned[0];
    public Hero P2 => HeroesSpawned.Count < 2 ? null : HeroesSpawned[1];

    public int UnitsSpawned { get; private set; }

    public Transform HeroesParentObject { get; private set; }
    public Transform EnemiesParentObject { get; private set; }

    public Hero[] ExistingHeroes => HeroesParentObject.GetComponentsInChildren<Hero>();
    public Enemy[] ExistingEnemies => EnemiesParentObject.GetComponentsInChildren<Enemy>();

    private System.Random RNG { get; set; }

    void Awake() {
        #region Singleton
        if (instance != null) {
            Debug.LogError("More than one GameState instance in scene!");
            return;
        }

        instance = this;
        #endregion

        HasStarted = false;
        HasEnded = false;
        PlayersConnected = 0;
        HeroesSpawned = new List<Hero>();
        UnitsSpawned = 0;

        RNG = new System.Random();
    }

    #region Registering OnSceneLoaded hook
    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    #endregion

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        Debug.Log($"Scene index: {scene.buildIndex}");
        if (scene.buildIndex != (int)SceneMaster.Scenes.PRELOAD) {
            HeroesParentObject = GameObject.FindWithTag(Constants.HEROES_PARENT_TAG).transform;
            EnemiesParentObject = GameObject.FindWithTag(Constants.ENEMIES_PARENT_TAG).transform;

            Dungeon = DungeonSceneMap.ByScene[(SceneMaster.Scenes)scene.buildIndex];
            DD = DungeonLoader.GetDungeonDetails(Dungeon);
            DD.UpdatedDetails += EventHandler.instance.DungeonDetailsUpdated;
            DD.DungeonCompleted += EventHandler.instance.DungeonCompleted;
        }
    }

    public void StartGame() {
        HasStarted = true;
        DD.StartRun();
    }

    public void EndGame() {
        HasEnded = true;
    }

    public GameObject SpawnHero(int _heroNum, int _ownerId) {
        // Get the prefab associated with the hero specified.
        GameObject heroPrefab = PrefabManager.instance.HeroPrefabs[(Heroes)_heroNum];

        // Instantiate the new hero.
        GameObject g = Instantiate(
            heroPrefab,
            HeroConstants.SPAWN_LOCATIONS[HeroesSpawned.Count],
            HeroConstants.SPAWN_ROTATION
        );

        // Set parent to our GameObject which will contain all active heroes.
        g.transform.SetParent(HeroesParentObject, false);

        // Get the hero component.
        Hero h = g.GetComponent<Hero>();

        // Set the owner ID of that hero.
        h.SetOwnerID(_ownerId);

        // Add the new hero to our list of active ones.
        HeroesSpawned.Add(h);

        // Return the GameObject corresponding to the newly spawned hero.
        return g;
    }

    public void SpawnEnemy(Enemies _enemyType, Vector3 _spawnPoint) {
        // Spawn the enemy and put it in our Enemies group object
        GameObject g = Instantiate(PrefabManager.instance.EnemyPrefabs[_enemyType], _spawnPoint, Quaternion.identity);
        g.transform.SetParent(GameState.instance.EnemiesParentObject.transform);
    }

    // Called by Enemy objects to get an ID
    public int GetNewUnitID() => UnitsSpawned++;

    #region Helper Functions
    public Hero GetHeroByOwner(int _ownerId) {
        foreach (Hero _h in HeroesSpawned) {
            if (_h.OwnerID == _ownerId) {
                return _h;
            }
        }

        Debug.Log($"Hero with owner ID {_ownerId} could not be found!");
        return null;
    }

    public Hero GetRandomAliveHero() {
        if (AliveHeroes.Count == 0) {
            return null;
        }

        return AliveHeroes[RNG.Next(AliveHeroes.Count)];
    }
    #endregion
}
