using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class DungeonLoader {
    private delegate DungeonDetails DungeonDetailGetter();
    private static Dictionary<DungeonID, DungeonDetailGetter> dungeonDetailGetters = new Dictionary<DungeonID, DungeonDetailGetter>() {
            { DungeonID.Coliseum, GetColiseumDetails }
    };

    public static DungeonDetails GetDungeonDetails(DungeonID _dungeonID) {
        return dungeonDetailGetters[_dungeonID]();
    }

    private static DungeonDetails GetColiseumDetails() => new ColiseumDetails();
}
