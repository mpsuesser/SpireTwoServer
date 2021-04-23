using UnityEngine;

public class PreloadManager : MonoBehaviour {
    public static PreloadManager instance;

    private bool sceneMasterReady;

    private bool EverythingLoaded {
        get {
            return sceneMasterReady;
        }
    }

    private void Awake() {
        instance = this;

        sceneMasterReady = false;
    }

    public void Ready(SceneMaster _sm) {
        sceneMasterReady = true;
        CheckLoadStatus();
    }

    public void CheckLoadStatus() {
        if (EverythingLoaded) {
            SceneMaster.instance.LoadColiseum();
        }
    }
}
