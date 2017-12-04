using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineRunnerShooter
{
    /*
     * Like bullets but made for the boss to work like rockets, should add explosion on ground hit
     * 
     */
    class BulletR
    {
        public Texture2D _texture;
        public Vector2 Positie;
        public Vector2 DestPos;
        public Vector2 _direction;
        public bool isFired;
        public bool isGoingUp;
        private Rectangle collisionRect;

        public BulletR(Texture2D texture)
        {
            _texture = texture;
            Positie = new Vector2();
            isFired = false;
            isGoingUp = true;
            collisionRect = new Rectangle(Positie.ToPoint(), new Point(50, 50));
        }
        public BulletR(Texture2D texture, Point size)
        {
            _texture = texture;
            Positie = new Vector2();
            isFired = false;
            collisionRect = new Rectangle(Positie.ToPoint(), size);
            isGoingUp = true;
        }

        public  void fire(float angle, Vector2 pos)
        {
            if (!isFired)
            {
                Positie = pos;
                _direction.X = Convert.ToInt16(Math.Cos(angle) * 10);
                _direction.Y = -Convert.ToInt16(Math.Sin(angle) * 8);
                isFired = true;
                isGoingUp = true;
            }
            
        }
        public void fire(Vector2 startPos, Vector2 destPos)
        {
            if (!isFired)
            {
                DestPos = destPos;
                Positie = startPos;
                _direction.X = 0;
                _direction.Y = -15;
                isFired = true;
            }
        }

        public void Update()
        {
            if (isFired)
            {
                Positie = Vector2.Add(Positie, _direction);
                if(Positie.Y < 0)
                {
                    Positie.X = DestPos.X;
                    _direction.Y = 10;
                    isGoingUp = false;
                }
            }
            if (Positie.Y > 3000)
            {
                isFired = false;
                isGoingUp = true;
                Positie = new Vector2(0,0);
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
            if (isGoingUp)
            {
                collisionRect.Location = new Point(0,0);
            }
            else
            {
                collisionRect.Location = Positie.ToPoint();
            }
            
            return collisionRect;
        }

        public void HitTarge()
        {
            isFired = false;
            Positie = new Vector2();
        }
    }
}
