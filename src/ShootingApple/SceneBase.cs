using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.DirectWrite;
using NAudio.Wave;

namespace ShootingApple
{
    class SceneBase
    {
        public static int SCENE_PHASE_BEGIN = 0;
        public static int SCENE_PHASE_LOBBY = 1;
        public static int SCENE_PHASE_PLAY = 2;
        public static int SCENE_PHASE_RESULT = 3;

        protected const float ScreenWidth = 360.0f;
        protected const float ScreenHeight = 540.0f;

        protected G2Texture? _skyBackground;
        protected G2Texture? _background;
        protected G2Texture? _fence;
        protected G2Texture? _tree1;
        protected G2Texture? _tree2;

        protected G2Font? _messageFont;

        protected WaveOut? _buttonSoundOutput;
        protected AudioFileReader? _buttonSoundReader;

        protected float _cameraX = 0.0f;
        protected float _cameraBobY = 0.0f;

        protected const float SkyWidth = 135.0f;
        protected const float SkyHeight = 86.0f;

        protected const float GroundWidth = 135.0f;
        protected const float GroundHeight = 60.0f;

        protected const float FenceTileStep = 42.0f;
        protected const float FenceY = 190.0f;

        protected const float TreeSpacing = 120.0f;

        protected const float BackTreeY = 155.0f;
        protected const float FrontTreeY = 200.0f;

        protected const float FrontTreeOffsetX = 60.0f;

        public int NextScene = -1;

        protected void RequestScene(int scenePhase)
        {
            NextScene = scenePhase;
        }


        virtual public void Init()
        {

            _buttonSoundReader =
            new AudioFileReader(
             "resource/Sound/ButtonClick.wav");

            _buttonSoundReader.Volume = 0.55f;

            _buttonSoundOutput =
                new WaveOut();

            _buttonSoundOutput.Init(
                _buttonSoundReader);

            NextScene = -1;

            _messageFont = new G2Font(
            "Arial",
            14.0f,
            textAlignment: TextAlignment.Center,
            paragraphAlignment: ParagraphAlignment.Center
            );

            _skyBackground =
                new("resource/SkyBackGround.png");

            _background =
                new("resource/BackGround.png");

            _fence =
                new("resource/Fence.png");

            _tree1 =
                new("resource/Tree1.png");

            _tree2 =
                new("resource/Tree2.png");
        }

        protected void PlayButtonSound()
        {
            if (_buttonSoundOutput == null ||
                _buttonSoundReader == null)
            {
                return;
            }

            _buttonSoundOutput.Stop();

            _buttonSoundReader.Position = 0;

            _buttonSoundOutput.Play();
        }

        protected void DrawCenter(
         G2Texture? texture,
          float width,
          float y)
        {
            float x =
                (ScreenWidth - width) / 2.0f;

            texture?.Draw(x, y);
        }

        protected bool IsInside(
            float mouseX,
            float mouseY,
            float x,
            float y,
            float width,
            float height)
        {
            return
                mouseX >= x &&
                mouseX <= x + width &&
                mouseY >= y &&
                mouseY <= y + height;
        }

        protected void DrawRight(
         G2Texture? texture,
         float width,     
         float xMargin,
         float y)
        {
            float x =
                ScreenWidth
                - width
                - xMargin;

            texture?.Draw(x, y);
        }

        protected void DrawBottomCenter(
         G2Texture? texture,
          float width,
          float height,
          float bottomMargin = 0.0f)
        {
            float x =
                (GameGlobal.ScreenSize.Width - width) / 2.0f;

            float y =
                GameGlobal.ScreenSize.Height
                - height
                - bottomMargin;

            texture?.Draw(x, y);
        }

        virtual public void Destroy()
        {
            _messageFont?.Dispose();
            _skyBackground?.Dispose();
            _background?.Dispose();
            _fence?.Dispose();

            _tree1?.Dispose();
            _tree2?.Dispose();

            _buttonSoundOutput?.Stop();
            _buttonSoundOutput?.Dispose();

            _buttonSoundReader?.Dispose();
        }

        virtual public void Update()
        {
        }

        virtual public void Render()
        {
        }

        protected void DrawSky()
        {
            if (_skyBackground == null)
                return;

            int startIndex =
                (int)MathF.Floor(
                    _cameraX / SkyWidth) - 1;

            int endIndex =
                (int)MathF.Ceiling(
                    (_cameraX + ScreenWidth)
                    / SkyWidth) + 1;

            for (float y = 0.0f;
                 y < ScreenHeight;
                 y += SkyHeight)
            {
                for (int i = startIndex;
                     i <= endIndex;
                     i++)
                {
                    float worldX =
                        i * SkyWidth;

                    float drawX =
                        worldX - _cameraX;

                    _skyBackground.Draw(
                        drawX,
                        y);
                }
            }
        }

        protected void DrawGround()
        {
            if (_background == null)
                return;

            const float groundStartY = 155.0f;

            int startIndex =
                (int)MathF.Floor(
                    _cameraX / GroundWidth) - 1;

            int endIndex =
                (int)MathF.Ceiling(
                    (_cameraX + ScreenWidth)
                    / GroundWidth) + 1;

            for (float y = groundStartY;
                 y < ScreenHeight;
                 y += GroundHeight)
            {
                for (int i = startIndex;
                     i <= endIndex;
                     i++)
                {
                    float worldX =
                        i * GroundWidth;

                    float drawX =
                        worldX - _cameraX;

                    _background.Draw(
                        drawX,
                        y + _cameraBobY);
                }
            }
        }

        protected void DrawFence()
        {
            if (_fence == null)
                return;

            int startIndex =
                (int)MathF.Floor(
                    _cameraX
                    / FenceTileStep) - 1;

            int endIndex =
                (int)MathF.Ceiling(
                    (_cameraX + ScreenWidth)
                    / FenceTileStep) + 1;

            for (int i = startIndex;
                 i <= endIndex;
                 i++)
            {
                float worldX =
                    i * FenceTileStep;

                float drawX =
                    worldX - _cameraX;

                _fence.Draw(
                    drawX,
                    FenceY + _cameraBobY);
            }
        }

        protected void DrawFrontTrees()
        {
            int startIndex =
                (int)MathF.Floor(
                    (_cameraX - 200.0f)
                    / TreeSpacing);

            int endIndex =
                (int)MathF.Ceiling(
                    (_cameraX + ScreenWidth + 200.0f)
                    / TreeSpacing);

            for (int i = startIndex;
                 i <= endIndex;
                 i++)
            {
                float worldX =
                    i * TreeSpacing
                    + FrontTreeOffsetX;

                float drawX =
                    worldX - _cameraX;

                if (i % 2 == 0)
                {
                    _tree2?.Draw(
                        drawX,
                        FrontTreeY + _cameraBobY);
                }
                else
                {
                    _tree1?.Draw(
                        drawX,
                        FrontTreeY + _cameraBobY);
                }
            }
        }

        protected void DrawBackTrees()
        {
            int startIndex =
                (int)MathF.Floor(
                    (_cameraX - 200.0f)
                    / TreeSpacing);

            int endIndex =
                (int)MathF.Ceiling(
                    (_cameraX + ScreenWidth + 200.0f)
                    / TreeSpacing);

            for (int i = startIndex;
                 i <= endIndex;
                 i++)
            {
                float worldX =
                    i * TreeSpacing;

                float drawX =
                    worldX - _cameraX;

                if (i % 2 == 0)
                {
                    _tree1?.Draw(
                        drawX,
                        BackTreeY + _cameraBobY);
                }
                else
                {
                    _tree2?.Draw(
                        drawX,
                        BackTreeY + _cameraBobY);
                }
            }
        }
    }
}
