// -------------------------------------------------------------------------------------------------------------------------------------------------------------
// Author: 3dapi (https://github.com/3dapi)
// -------------------------------------------------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using ShootingApple;
using Vortice.Mathematics;

class GameMain : G2AppBase
{
    public override System.Drawing.Size ScreenSize
        => GameGlobal.ScreenSize;

    public override string GameName
        => GameGlobal.GameName;

    private readonly List<SceneBase> gameScene =
        new();

    private SceneBase gameSceneCurrent;

    public GameMain()
    {
        gameScene.Add(
            new SceneBegin());

        gameScene.Add(
            new SceneLobby());

        gameScene.Add(
            new ScenePlay());

        gameScene.Add(
            new SceneResult());

        gameSceneCurrent =
            gameScene[
                SceneBase.SCENE_PHASE_BEGIN
            ];
    }

    protected override void Initialize()
    {
        gameSceneCurrent.Init();
    }

    protected override void Update()
    {
        gameSceneCurrent.Update();
    }

    protected override void Render()
    {
        gameSceneCurrent.Render();
    }

    public void ChangeScene(
        int scenePhase)
    {
        gameSceneCurrent.Destroy();

        gameSceneCurrent =
            gameScene[scenePhase];

        gameSceneCurrent.Init();
    }

    public override void Dispose()
    {
        gameSceneCurrent.Destroy();

        base.Dispose();
    }
}