using System.Collections.Generic;

public static class DungeonSceneMap {
    public static readonly Dictionary<SceneMaster.Scenes, DungeonID> ByScene = new Dictionary<SceneMaster.Scenes, DungeonID>() {
        { SceneMaster.Scenes.COLISEUM, DungeonID.Coliseum }
    };
}