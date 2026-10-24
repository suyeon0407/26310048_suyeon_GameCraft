using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingApple
{
    class SceneLobby : SceneBase
    {
        private G2Texture? _controlText;

        override public void Init()
        {
            base.Init();

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

            DrawRight(
                _controlText,
                41.0f,
                10.0f,
                150.0f);
        }

        override public void Destroy()
        {
            _controlText?.Dispose();

            base.Destroy();
        }
    }
}
