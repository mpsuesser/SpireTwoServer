using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMaster : MonoBehaviour {
    public static SceneMaster instance; // singleton

    void Awake() {
        #region Singleton
        if (instance != null) {
            Debug.LogError("More than one SceneMaster instance in scene!");
            return;
        }
        instance = this;
        #endregion

        DontDestroyOnLoad(gameObject);
    }

    public enum Scenes {
        PRELOAD = 0,
        COLISEUM
    }

    void Start() {
        PreloadManager.instance.Ready(this);
    }

    public void LoadColiseum() {
        SceneManager.LoadScene((int)Scenes.COLISEUM, LoadSceneMode.Additive);
    }
}
