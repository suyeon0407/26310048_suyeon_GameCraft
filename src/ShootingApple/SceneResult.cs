using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Mathematics;

namespace ShootingApple
{
    class SceneResult : SceneBase
    {
        private G2Texture? _blackHalfScreen;

        private G2Texture? _gameClearTitle;
        private G2Texture? _scoreTextWhite;

        private G2Texture? _retryButton1;
        private G2Texture? _retryButton2;

        private G2Texture? _exitButton1;
        private G2Texture? _exitButton2;

        private G2Texture? _controlText;

        override public void Init()
        {
            base.Init();

            _blackHalfScreen =
                new("resource/Black_Half_Screen1.png");

            _gameClearTitle =
                new("resource/GameClear_Title_Text1.png");

            _scoreTextWhite =
                new("resource/Score_Text_White.png");

            _retryButton1 =
                new("resource/Retry_Button_Text1.png");

            _retryButton2 =
                new("resource/Retry_Button_Text2.png");

            _exitButton1 =
                new("resource/ExitGame_Button_Text1.png");

            _exitButton2 =
                new("resource/ExitGame_Button_Text2.png");

            _controlText =
                new("resource/ControlText.png");
        }

        override public void Update()
        {
        }

        override public void Render()
        {
            DrawSky();
            DrawGround();

            DrawFence();
            DrawBackTrees();
            DrawFrontTrees();

            _blackHalfScreen?.Draw(
                new Rect(
                    0.0f,
                    0.0f,
                    GameGlobal.ScreenSize.Width,
                    GameGlobal.ScreenSize.Height),
                new Rect(
                    0.0f,
                    0.0f,
                    210.0f,
                    320.0f)
            );

            DrawCenter(
                _gameClearTitle,
                94.0f,
                115.0f);

            DrawCenter(
                _scoreTextWhite,
                29.0f,
                155.0f);

            DrawCenter(
                _retryButton1,
                72.0f,
                195.0f);

            DrawCenter(
                _exitButton1,
                72.0f,
                235.0f);
        }

        override public void Destroy()
        {
            _blackHalfScreen?.Dispose();

            _gameClearTitle?.Dispose();
            _scoreTextWhite?.Dispose();

            _retryButton1?.Dispose();
            _retryButton2?.Dispose();

            _exitButton1?.Dispose();
            _exitButton2?.Dispose();

            _controlText?.Dispose();

            base.Destroy();
        }
    }
}
