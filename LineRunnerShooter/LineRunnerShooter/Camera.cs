using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineRunnerShooter
{
    class Camera
    {
        private readonly Viewport _viewport;
        public Rectangle Focus;
        public Camera(Viewport viewport)
        {
            _viewport = viewport;
            Rotation = 0;
            Zoom = 1;
            Origin = new Vector2(viewport.Width / 2f, viewport.Height / 2f);
            Position = Vector2.Zero;
            Focus = new Rectangle((int)Position.X + 50, (int)Position.Y + 100, 200, 500);
        }

        public Vector2 ViewportCenter
        {
            get
            {
                return new Vector2(_viewport.Width * 0.5f, _viewport.Height * 0.5f);
            }
        }

        public float Rotation { get; set; }
        public float Zoom { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Origin { get; set; }

        public Matrix GetViewMatrix()
        {
            Matrix m = Matrix.CreateTranslation(new Vector3(-Position, 0)) *
                Matrix.CreateRotationZ(Rotation) * Matrix.CreateScale(Zoom, Zoom, 1);
            return m;
        }
    }
}
