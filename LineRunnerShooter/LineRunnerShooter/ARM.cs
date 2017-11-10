using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineRunnerShooter
{
    class ARM
    {
        private Texture2D pixel;
        private float angle;
        private Vector2 _position;
        public List<Bullet> bullets;

        public ARM(Texture2D pix, Texture2D energy)
        {
            angle = 0;
            pixel = pix;
            _position = new Vector2(200, 240);
            bullets = new List<Bullet>();
            bullets.Add(new Bullet(energy));
            bullets.Add(new Bullet(energy));
            bullets.Add(new Bullet(energy));
        }
        public void Update(GameTime gameTime, Vector2 position, Vector2 mouse, Vector2 camPos)
        {
            _position = position;
            _position.X += 50;
            _position.Y += 65;
            float xVers =  -mouse.X+200;
            float yVers =  -mouse.Y+200;
            angle = (float)Math.Atan2(xVers,yVers) + (float) (Math.PI/2);
            foreach(Bullet b in bullets)
            {
                b.Update(camPos, gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, 250, 195);
            Vector2 origin = new Vector2(5, 10);
            foreach (Bullet b in bullets)
            {
                b.Draw(spriteBatch);
            }
            spriteBatch.Draw(pixel, _position, sourceRectangle, Color.White, -angle, origin, 1.0f, SpriteEffects.None, 1);
        }

        public void Fire()
        {
            //bullet.fire(angle, _position);
            int i = 0;
            while((i != -1))
            {
                Console.WriteLine(i);
                if (!bullets[i].isFired)
                {
                    bullets[i].fire(angle, _position);
                    i = -1;
                }
                else
                {
                    i++;
                    if(bullets.Count <= i)
                    {
                        i = -1;
                    }
                }
            }
        }

        public List<Rectangle> getBulletsRect()
        {
            List<Rectangle> bulletsRect = new List<Rectangle>();
            foreach (Bullet b in bullets)
            {
                bulletsRect.Add(b.getCollisionRectagle());
            }
            return bulletsRect;
        }
    }
}

