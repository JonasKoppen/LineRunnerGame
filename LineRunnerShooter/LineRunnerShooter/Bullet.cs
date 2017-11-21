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

        public Bullet(Texture2D texture)
        {
            _texture = texture;
            Positie = new Vector2();
            isFired = false;
            collisionRect = new Rectangle(Positie.ToPoint(), new Point(50, 50));
            viewBox = new Rectangle(0, 0, 2000, 2000);
        }
        public Bullet(Texture2D texture, Point size)
        {
            _texture = texture;
            Positie = new Vector2();
            isFired = false;
            collisionRect = new Rectangle(Positie.ToPoint(), size);
        }

        public  void fire(float angle, Vector2 pos)
        {
            if (!isFired)
            {
                Positie = pos;
                _direction.X = Convert.ToInt16(Math.Cos(angle));
                _direction.Y = -Convert.ToInt16(Math.Sin(angle));
                isFired = true;
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
            }

        }

        public void Update(Vector2 camPos, GameTime gameTime)
        {
            viewBox.Location = camPos.ToPoint() - new Point(200,800);
            if (isFired)
            {
                Positie.X += Convert.ToInt32(gameTime.ElapsedGameTime.TotalMilliseconds * _direction.X);
                Positie.Y += Convert.ToInt32(gameTime.ElapsedGameTime.TotalMilliseconds * _direction.Y);
            }
            if (!collisionRect.Intersects(viewBox))
            {
                isFired = false;
                Positie = new Vector2();
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
