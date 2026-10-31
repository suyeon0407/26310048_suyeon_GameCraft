using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Mathematics;
using System.Windows.Forms;

namespace ShootingApple
{
    class SceneBegin : SceneBase
    {
        private G2Texture? _blackHalfScreen;
        private G2Texture? _gameTitle;

        private G2Texture? _startButton1;
        private G2Texture? _startButton2;

        private G2Texture? _howToPlayButton1;
        private G2Texture? _howToPlayButton2;

        private G2Texture? _exitButton1;
        private G2Texture? _exitButton2;

        private const float StartButtonWidth = 110.0f;
        private const float StartButtonHeight = 39.0f;

        private const float StartButtonY = 310.0f;

        private bool _startPressed = false;

        private bool _startWaiting = false;

        override public void Init()
        {
            _startPressed = false;
            _startWaiting = false;

            base.Init();

            _startPressed = false;

            _blackHalfScreen =
                new("resource/Black_Half_Screen1.png");

            _gameTitle =
                new("resource/GameTitle_Text1.png");

            _startButton1 =
                new("resource/Game_Start_Button_Text1.png");

            _startButton2 =
                new("resource/Game_Start_Button_Text2.png");

            _howToPlayButton1 =
                new("resource/HowToPlay_Button_Text1.png");

            _howToPlayButton2 =
                new("resource/HowToPlay_Button_Text2.png");

            _exitButton1 =
                new("resource/ExitGame_Button_Text1.png");

            _exitButton2 =
                new("resource/ExitGame_Button_Text2.png");
        }

        override public void Update()
        {
            var input =
                G2AppBase.Instance?.Input;

            if (input == null)
                return;

            if (_startWaiting)
            {
                if (_buttonSoundOutput == null ||
                    _buttonSoundOutput.PlaybackState
                        != NAudio.Wave.PlaybackState.Playing)
                {
                    RequestScene(
                        SCENE_PHASE_PLAY);
                }

                return;
            }


            var mouse =
                input.MousePosition;

            float startX =
                (ScreenWidth - StartButtonWidth)
                / 2.0f;

            bool startInside =
                IsInside(
                    mouse.X,
                    mouse.Y,
                    startX,
                    StartButtonY,
                    StartButtonWidth,
                    StartButtonHeight);


            if (input.IsButtonDown(MouseButtons.Left))
            {
                if (startInside)
                {
                    _startPressed = true;

                    PlayButtonSound();
                }
            }

            if (input.IsButtonUp(MouseButtons.Left))
            {
                if (_startPressed && startInside)
                {
                    _startWaiting = true;
                }

                _startPressed = false;
            }
        }

        override public void Render()
        {
            DrawSky();
            DrawGround();
            DrawFence();

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
                _gameTitle,
                134.0f,
                190.0f);

            if (_startPressed)
            {
                DrawCenter(
                    _startButton2,
                    StartButtonWidth,
                    StartButtonY);
            }
            else
            {
                DrawCenter(
                    _startButton1,
                    StartButtonWidth,
                    StartButtonY);
            }

            DrawCenter(
                _howToPlayButton1,
                72.0f,
                380.0f);

            DrawCenter(
                _exitButton1,
                72.0f,
                430.0f);
        }

        override public void Destroy()
        {
            _gameTitle?.Dispose();

            _startButton1?.Dispose();
            _startButton2?.Dispose();

            _howToPlayButton1?.Dispose();
            _howToPlayButton2?.Dispose();

            _exitButton1?.Dispose();
            _exitButton2?.Dispose();

            base.Destroy();
        }
    }
}
