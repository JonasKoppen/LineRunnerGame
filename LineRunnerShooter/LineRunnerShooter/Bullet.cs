using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineRunnerShooter
{
    class Bullet
    {
        public Texture2D _texture;
        public Vector2 Positie;
        public Vector2 _direction;
        public bool isFired;
        private Rectangle collisionRect;
        private Rectangle viewBox;
        private double timeToLive;

        public Bullet(Texture2D texture)
        {
            _texture = texture;
            Positie = new Vector2();
            isFired = false;
            collisionRect = new Rectangle(Positie.ToPoint(), new Point(50, 50));
            viewBox = new Rectangle(0, 0, 1000, 1000);
            timeToLive = 0;
        }
        public Bullet(Texture2D texture, Point size)
        {
            _texture = texture;
            Positie = new Vector2();
            isFired = false;
            collisionRect = new Rectangle(Positie.ToPoint(), size);
            timeToLive = 0;
        }

        public void fire(float angle, Vector2 pos)
        {
            if (!isFired)
            {
                Positie = pos;
                _direction.X = (float)(Math.Cos(angle));
                _direction.Y = -(float)(Math.Sin(angle));
                isFired = true;
                timeToLive = 1000;
            }
            
        }
        public void fire(Vector2 pos)
        {
            if (!isFired)
            {
                Positie = pos;
                _direction.X = 0;
                _direction.Y = 10;
                isFired = true;
                timeToLive = 1000;
            }

        }

        public void Update(Vector2 camPos, GameTime gameTime)
        {
            timeToLive -= gameTime.ElapsedGameTime.TotalMilliseconds;
            if (isFired)
            {
                Positie.X += Convert.ToInt32(gameTime.ElapsedGameTime.TotalMilliseconds * _direction.X);
                Positie.Y += Convert.ToInt32(gameTime.ElapsedGameTime.TotalMilliseconds * _direction.Y);
            }
            if (timeToLive <0)
            {
                isFired = false;
                Positie = new Vector2(1000,1000);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isFired)
            {
                spriteBatch.Draw(_texture, Positie, Color.White);
            }
            
        }

        public Rectangle getCollisionRectagle()
        {
            collisionRect.Location = Positie.ToPoint();
            return collisionRect;
        }

        public void HitTarge()
        {
            isFired = false;
            Positie = new Vector2();
        }
    }
}
