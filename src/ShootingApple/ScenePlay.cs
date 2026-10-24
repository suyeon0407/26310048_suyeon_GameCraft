using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingApple
{
    class ScenePlay : SceneBase
    {
        private G2Texture? _basketBack1;
        private G2Texture? _basketFront1;

        private G2Texture?[] _idleWalk =
            new G2Texture?[5];

        private G2Texture?[] _numbers =
            new G2Texture?[10];

        private G2Texture? _timeText;
        private G2Texture? _scoreText;
        private G2Texture? _controlText;

        private G2Texture? _normalApple;
        private G2Texture? _goldApple;
        private G2Texture? _rottenApple;
        private G2Texture? _bugApple;

        private int _score = 0;
        private int _timeLeft = 80;

        private const float DigitStep = 12.0f;

        override public void Init()
        {
            base.Init();

            _basketBack1 =
                new("resource/BasketBack1.png");

            _basketFront1 =
                new("resource/BasketFront1.png");

            for (int i = 0; i < 5; i++)
            {
                _idleWalk[i] =
                    new G2Texture(
                        $"resource/IdleWalk_Gun{i + 1}.png");
            }

            for (int i = 0; i < 10; i++)
            {
                _numbers[i] =
                    new G2Texture(
                        $"resource/{i}_Text.png");
            }

            _timeText =
                new("resource/TimeText1.png");

            _scoreText =
                new("resource/ScoreText1.png");

            _controlText =
                new("resource/ControlText.png");

            _normalApple =
                new("resource/NormalApple.png");

            _goldApple =
                new("resource/GoldApple.png");

            _rottenApple =
                new("resource/RottenApple.png");

            _bugApple =
                new("resource/BugApple.png");
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

            DrawPlayer();
            DrawPlayUI();

            DrawTestApples();
        }

        private void DrawPlayer()
        {
            DrawCenter(
                _basketBack1,
                70.0f,
                285.0f);

            DrawBottomCenter(
                _idleWalk[0],
                96.0f,
                110.0f,
                92.0f);

            DrawCenter(
                _basketFront1,
                70.0f,
                285.0f);
        }

        private void DrawNumber(
            int number,
            float x,
            float y)
        {
            string text =
                number.ToString();

            float currentX = x;

            foreach (char ch in text)
            {
                int index =
                    ch - '0';

                _numbers[index]?.Draw(
                    currentX,
                    y);

                currentX += DigitStep;
            }
        }

        private float GetNumberWidth(
            int number)
        {
            return
                number.ToString().Length
                * DigitStep;
        }

        private void DrawPlayUI()
        {
            const float topY = 10.0f;

            _timeText?.Draw(
                10.0f,
                topY);

            DrawNumber(
                _timeLeft,
                42.0f,
                topY - 2.0f);

            float scoreWidth =
                GetNumberWidth(_score);

            float scoreValueX =
                ScreenWidth
                - 10.0f
                - scoreWidth;

            float scoreLabelX =
                scoreValueX
                - 34.0f;

            _scoreText?.Draw(
                scoreLabelX,
                topY);

            DrawNumber(
                _score,
                scoreValueX,
                topY - 2.0f);

            _controlText?.Draw(
                ScreenWidth - 51.0f,
                ScreenHeight - 47.0f);
        }

        private void DrawTestApples()
        {
            _normalApple?.Draw(
                250.0f,
                120.0f);

            _goldApple?.Draw(
                285.0f,
                120.0f);

            _rottenApple?.Draw(
                320.0f,
                120.0f);

            _bugApple?.Draw(
                355.0f,
                120.0f);
        }

        override public void Destroy()
        {
            _basketBack1?.Dispose();
            _basketFront1?.Dispose();

            foreach (var texture in _idleWalk)
            {
                texture?.Dispose();
            }

            foreach (var texture in _numbers)
            {
                texture?.Dispose();
            }

            _timeText?.Dispose();
            _scoreText?.Dispose();
            _controlText?.Dispose();

            _normalApple?.Dispose();
            _goldApple?.Dispose();
            _rottenApple?.Dispose();
            _bugApple?.Dispose();

            base.Destroy();
        }
    }
}
