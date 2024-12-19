public enum SceneID
{
    MainGameScene,
    BattleScene
}

public static class ScenesFactory
{
    public static string GetScene(SceneID scene)
    {
        return scene switch
        {
            SceneID.MainGameScene => "MainGameScene",
            SceneID.BattleScene => "BattleScene",
            _ => throw new System.NotImplementedException(),
        };
    }
}
