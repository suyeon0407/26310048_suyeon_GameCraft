// -------------------------------------------------------------------------------------------------------------------------------------------------------------
// Author: 3dapi (https://github.com/3dapi)
// -------------------------------------------------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using ShootingApple;
using Vortice.Mathematics;
using NAudio.Wave;

class GameMain : G2AppBase
{
    public override System.Drawing.Size ScreenSize
        => GameGlobal.ScreenSize;

    public override string GameName
        => GameGlobal.GameName;

    private readonly List<SceneBase> gameScene =
        new();

    private SceneBase gameSceneCurrent;

    private WaveOut? _bgmOutput;
    private AudioFileReader? _bgmReader;

    private bool _bgmPlaying = true;

    public GameMain()
    {
        gameScene.Add(new SceneBegin());
        gameScene.Add(new SceneLobby());
        gameScene.Add(new ScenePlay());
        gameScene.Add(new SceneResult());

        gameSceneCurrent =
            gameScene[SceneBase.SCENE_PHASE_BEGIN];
    }

    protected override void Initialize()
    {
        _bgmReader =
            new AudioFileReader(
                "resource/Sound/BGM.wav");

        _bgmReader.Volume = 0.05f;

        _bgmOutput =
            new WaveOut();

        _bgmOutput.Init(
            _bgmReader);

        _bgmOutput.Play();

        gameSceneCurrent.Init();
    }

    private void UpdateBGM()
    {
        if (!_bgmPlaying)
            return;

        if (_bgmOutput == null ||
            _bgmReader == null)
        {
            return;
        }

        if (_bgmOutput.PlaybackState
            != PlaybackState.Playing)
        {
            _bgmReader.Position = 0;
            _bgmOutput.Play();
        }
    }

    protected override void Update()
    {
        UpdateBGM();

        gameSceneCurrent.Update();

        if (gameSceneCurrent.NextScene != -1)
        {
            int nextScene =
                gameSceneCurrent.NextScene;

            gameSceneCurrent.NextScene = -1;

            ChangeScene(nextScene);
        }
    }

    protected override void Render()
    {
        gameSceneCurrent.Render();
    }

    public void ChangeScene(
        int scenePhase)
    {
        if (scenePhase ==
            SceneBase.SCENE_PHASE_RESULT)
        {
            _bgmPlaying = false;

            _bgmOutput?.Stop();
        }

        gameSceneCurrent.Destroy();

        gameSceneCurrent =
            gameScene[scenePhase];

        gameSceneCurrent.Init();
    }

    public override void Dispose()
    {
        gameSceneCurrent.Destroy();

        _bgmOutput?.Stop();
        _bgmOutput?.Dispose();

        _bgmReader?.Dispose();

        base.Dispose();
    }
}