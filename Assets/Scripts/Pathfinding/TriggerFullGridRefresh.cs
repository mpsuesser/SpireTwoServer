using UnityEngine;

public class TriggerFullGridRefresh : MonoBehaviour {
    void Awake() {
        Grid.instance.RefreshAllNodes();
    }
}