using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineRunner : MonoBehaviour
{
    public static CoroutineRunner instance;

    void Awake() {
        #region Singleton
        if (instance != null) {
            Debug.LogError("More than one CoroutineRunner instance in scene!");
            return;
        }

        instance = this;
        #endregion
    }

    public Coroutine Run(IEnumerator _coroutine) {
        return StartCoroutine(_coroutine);
    }

    public void Stop(Coroutine _coroutine) {
        StopCoroutine(_coroutine);
    }
}
