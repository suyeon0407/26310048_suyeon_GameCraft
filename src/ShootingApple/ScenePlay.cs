using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vortice.Mathematics;
using Vortice.XInput;
using NAudio.Wave;

namespace ShootingApple
{
    class ScenePlay : SceneBase
    {
        private G2Texture? _basketBack1;
        private G2Texture? _basketFront1;

        private G2Texture?[] _idleWalk =
            new G2Texture?[5];

        private G2Texture? _controlText;

        private G2Texture? _normalApple;
        private G2Texture? _goldApple;
        private G2Texture? _rottenApple;
        private G2Texture? _bugApple;

        private G2Texture? _point;

        private WaveOut? _walkSoundOutput;
        private AudioFileReader? _walkSoundReader;

        private int _score = 0;
        private int _timeLeft = 80;

        private const float MoveSpeed = 1.0f;

        private int _walkFrame = 0;
        private int _walkFrameCounter = 0;

        private float _walkPhase = 0.0f;

        private const int WalkFrameDelay = 18;
        private const float WalkBobAmount = 1.0f;
        private const float WalkBobSpeed = 0.07f;

        private const float AppleMinDistance = 22.0f;

        private class AppleData
        {
            public float WorldX;
            public float Y;

            public float StartY;

            public int Value;
            public int Type;

            public bool IsFront;

            public double SpawnTime;

            public bool IsFalling;
        }

        private List<AppleData> _apples =
          new List<AppleData>();

        private HashSet<int> _spawnedBackTrees =
            new HashSet<int>();

        private HashSet<int> _spawnedFrontTrees =
            new HashSet<int>();

        private Random _random =
            new Random();

        override public void Init()
        {
            base.Init();

            Cursor.Hide();

            _point =
                new G2Texture(
                    "resource/Point1.png");

            _walkSoundReader =
            new AudioFileReader(
              "resource/Sound/WalkSound.wav");

            _walkSoundReader.Volume = 0.40f;

            _walkSoundOutput =
                new WaveOut();

            _walkSoundOutput.Init(
                _walkSoundReader);

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

            _walkFrame = 0;
            _walkFrameCounter = 0;

            _walkPhase = 0.0f;
            _cameraBobY = 0.0f;

            SpawnApplesForTree(0, false);
            SpawnApplesForTree(1, false);

            SpawnApplesForTree(0, true);
            SpawnApplesForTree(1, true);
        }

        override public void Update()
        {
            var input =
                G2AppBase.Instance?.Input;

            if (input == null)
                return;

            bool moveLeft =
                input.IsKeyDown(Keys.A) ||
                input.IsKeyPress(Keys.A);

            bool moveRight =
                input.IsKeyDown(Keys.D) ||
                input.IsKeyPress(Keys.D);


            if (moveLeft && !moveRight)
            {
                _cameraX -= MoveSpeed;
            }

            if (moveRight && !moveLeft)
            {
                _cameraX += MoveSpeed;
            }


            bool isMoving =
                moveLeft ^ moveRight;

            UpdateWalkAnimation(isMoving);
            UpdateWalkSound(isMoving);
        }

        override public void Render()
        {
            DrawSky();
            DrawGround();

            DrawFence();

            DrawBackTrees();
            DrawApples(false);

            DrawFrontTrees();
            DrawApples(true);

            DrawPlayer();
            DrawPlayUI();

            DrawCrosshair();
        }

        private void DrawCrosshair()
        {
            var input =
                G2AppBase.Instance?.Input;

            if (input == null)
                return;

            var mouse =
                input.MousePosition;

            _point?.Draw(
                mouse.X - 12.0f,
                mouse.Y - 12.0f);
        }

        private void DrawPlayer()
        {
            DrawCenter(
                _basketBack1,
                70.0f,
                285.0f);

            float gunX =
                (ScreenWidth - 96.0f) / 2.0f
                - 25.0f;

            float gunY =
                ScreenHeight
                - 110.0f
                - 92.0f;

            _idleWalk[_walkFrame]?.Draw(
                gunX,
                gunY);

            DrawCenter(
                _basketFront1,
                70.0f,
                285.0f);
        }

        private void DrawPlayUI()
        {
            _messageFont?.DrawText(
                $"Time : {_timeLeft}",
                new Rect(
                    5.0f,
                    5.0f,
                    100.0f,
                    30.0f),
                new Color4(
                    0.0f,
                    0.0f,
                    0.0f,
                    1.0f)
            );

            _messageFont?.DrawText(
                $"Score : {_score}",
                new Rect(
                    ScreenWidth - 105.0f,
                    5.0f,
                    100.0f,
                    30.0f),
                new Color4(
                    0.0f,
                    0.0f,
                    0.0f,
                    1.0f)
            );

            _controlText?.Draw(
                ScreenWidth - 51.0f,
                ScreenHeight - 47.0f);
        }
        private void SpawnApplesForTree(
            int treeIndex,
            bool isFront)
        {
            float treeWorldX;
            float treeY;

            if (isFront)
            {
                treeWorldX =
                    treeIndex * TreeSpacing
                    + FrontTreeOffsetX;

                treeY = FrontTreeY;
            }
            else
            {
                treeWorldX =
                    treeIndex * TreeSpacing;

                treeY = BackTreeY;
            }

            int appleCount =
                _random.Next(1, 4);

            for (int i = 0; i < appleCount; i++)
            {
                float appleX = 0.0f;
                float appleY = 0.0f;

                bool validPosition = false;

                for (int attempt = 0;
                     attempt < 20;
                     attempt++)
                {
                    appleX =
                        treeWorldX
                        + _random.Next(12, 59);

                    appleY =
                        treeY
                        + _random.Next(8, 39);

                    validPosition = true;

                    foreach (AppleData other in _apples)
                    {
                        if (other.IsFront != isFront)
                            continue;

                        float dx =
                            appleX - other.WorldX;

                        float dy =
                            appleY - other.Y;

                        float distance =
                            MathF.Sqrt(
                                dx * dx +
                                dy * dy);

                        if (distance < AppleMinDistance)
                        {
                            validPosition = false;
                            break;
                        }
                    }

                    if (validPosition)
                        break;
                }

                if (!validPosition)
                    continue;

                AppleData apple =
                    new AppleData();

                apple.WorldX = appleX;
                apple.Y = appleY;
                apple.StartY = appleY;

                apple.Value =
                    _random.Next(1, 10);

                apple.Type =
                    _random.Next(0, 4);

                apple.IsFront =
                    isFront;

                apple.IsFalling =
                    false;

                _apples.Add(apple);
            }
        }

        private void DrawApples(bool isFront)
        {
            foreach (AppleData apple in _apples)
            {
                if (apple.IsFront != isFront)
                    continue;

                float drawX =
                    apple.WorldX - _cameraX;

                G2Texture? texture =
                    apple.Type switch
                    {
                        0 => _normalApple,
                        1 => _goldApple,
                        2 => _rottenApple,
                        3 => _bugApple,
                        _ => _normalApple
                    };

                texture?.Draw(
                    drawX,
                    apple.Y + _cameraBobY);
            }
        }

        private void UpdateWalkSound(bool isMoving)
        {
            if (_walkSoundOutput == null ||
                _walkSoundReader == null)
            {
                return;
            }

            if (isMoving)
            {
                if (_walkSoundOutput.PlaybackState
                    != PlaybackState.Playing)
                {
                    _walkSoundReader.Position = 0;
                    _walkSoundOutput.Play();
                }
            }
            else
            {
                if (_walkSoundOutput.PlaybackState
                    == PlaybackState.Playing)
                {
                    _walkSoundOutput.Stop();
                    _walkSoundReader.Position = 0;
                }
            }
        }

        private void UpdateWalkAnimation(
        bool isMoving)
        {
            if (isMoving)
            {
                _walkFrameCounter++;

                if (_walkFrameCounter >= WalkFrameDelay)
                {
                    _walkFrameCounter = 0;

                    _walkFrame++;

                    if (_walkFrame >= 5)
                    {
                        _walkFrame = 0;
                    }
                }

                _walkPhase += WalkBobSpeed;

                _cameraBobY =
                    MathF.Sin(_walkPhase)
                    * WalkBobAmount;
            }
            else
            {
                _walkFrame = 0;
                _walkFrameCounter = 0;

                _walkPhase = 0.0f;
                _cameraBobY = 0.0f;
            }
        }

        override public void Destroy()
        {
            Cursor.Show();
            _point?.Dispose();

            _basketBack1?.Dispose();
            _basketFront1?.Dispose();

            foreach (var texture in _idleWalk)
            {
                texture?.Dispose();
            }

            _controlText?.Dispose();

            _normalApple?.Dispose();
            _goldApple?.Dispose();
            _rottenApple?.Dispose();
            _bugApple?.Dispose();

            _walkSoundOutput?.Stop();
            _walkSoundOutput?.Dispose();

            _walkSoundReader?.Dispose();

            base.Destroy();
        }
    }
}
