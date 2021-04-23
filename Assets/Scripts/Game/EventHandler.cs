using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventHandler : MonoBehaviour {
    public static EventHandler instance;

    private GameState GS;

    void Awake() {
        #region Singleton
        if (instance != null) {
            Debug.LogError("More than one EventHandler instance in scene!");
            return;
        }

        instance = this;
        #endregion
    }

    #region Registering OnSceneLoaded hook
    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    #endregion

    public void OnSceneLoaded(Scene _scene, LoadSceneMode _mode) {
        Debug.Log("New scene was loaded!");
        // Refresh the grid since new objects have been instantiated and some are likely not walkable
        Grid.instance.RefreshAllNodes();
    }

    void Start() {
        GS = GameState.instance;
    }

    public void PlayerJoinedGame(int _clientId) {
        foreach (Enemy e in GS.EnemiesParentObject.GetComponentsInChildren<Enemy>()) {
            ServerSend.EnemySpawnedToClient(_clientId, e);
        }
    }

    public void UpdateMovement(int _ownerId, Movement.Direction _dir, Vector3 _eulerAngles) {
        Hero h = GS.GetHeroByOwner(_ownerId);
        if (h == null) {
            return;
        }

        h.SetRotation(_eulerAngles);
        h.SetDirection(_dir);
    }

    public void JumpInput(int _ownerId) {
        Hero h = GS.GetHeroByOwner(_ownerId);
        if (h == null) {
            return;
        }

        h.Jump();
    }

    public void AbilityPressed(int _ownerId, AbilityID _abilityID) {
        Hero h = GS.GetHeroByOwner(_ownerId);
        if (h == null) {
            return;
        }

        Debug.Log($"Hero {h.OwnerID} pressed ability {_abilityID}!");
        h.AbilityUsed(_abilityID);
    }

    public void StartTriggered(Hero _h) {
        GS.StartGame();
    }

    public void DungeonDetailsUpdated(DungeonDetails _dd) {
        ServerSend.SyncDungeonDetailsToAll(_dd);
    }

    public void DungeonCompleted(DungeonDetails _dd) {
        GS.EndGame();
        ServerSend.SyncDungeonDetailsToAll(_dd);
    }

    public void EndTriggered(Hero _h) {
        Debug.Log($"Hero {_h.OwnerID} ended the level via trigger!");
    }

    public void EnemySpawned(Enemy _e) {
        ServerSend.EnemySpawnedToAll(_e);
    }

    public void EnemyKilled(Enemy _e) {
        GS.DD.EnemyKilled(_e);

        ServerSend.EnemyKilled(_e.ID);
    }

    public void EnemyStateChanged(Enemy _e) {
        ServerSend.EnemyStateChanged(_e.ID, _e.State.ID);
    }

    public void ClientLoadedDungeon(int _clientId) {
        ServerSend.SyncDungeonDetailsToOne(_clientId, GS.DD);
    }
}
