using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineRunnerShooter
    /*
     * Explenation:
     * The A.R.M. onderhoud de variatie in wapens voor de player en enemy, kan gaan van melee (wat dan zelf een collisiebox terug geeft) tot een wapen dat een lijst van bullets terug geeft.
     * 
     * 
     * 
     * 
     */ 

{
    abstract class ARMBluePrint
    {
        private Texture2D _texture;
        protected float angle;
        protected Vector2 _position;
        public List<Bullet> bullets;

        public ARMBluePrint(Texture2D pix)
        {
            _texture = pix;
        }

        public abstract void Update(GameTime gameTime, Vector2 position, Vector2 mouse);
        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, 81, 36);
            Vector2 origin = new Vector2(5, 10);
            spriteBatch.Draw(_texture, _position, sourceRectangle, Color.White, angle, origin, 1.0f, SpriteEffects.None, 1);
            foreach (Bullet b in bullets)
            {
                b.Draw(spriteBatch);
            }
        }
        public abstract void Fire();

        public abstract List<Bullet> getBullets();
    }

    class ShotARM : ARMBluePrint //only has 1 bullet active 
    {

        public ShotARM(Texture2D pix, Texture2D energy, int amountBullets) : base(pix)
        {
            angle = 0;
            _position = new Vector2(200, 240);
            bullets = new List<Bullet>();
            if(amountBullets > 0)
            {
                for(int i = 0; i < amountBullets; i++)
                {
                    bullets.Add(new Bullet(energy)); //First gun can only fire 1 bullet, make a variation with more bullets and selection key
                } 
            }
        }
        public override void Update(GameTime gameTime, Vector2 position, Vector2 mouse)
        {
            _position = position;
            _position.X += 40;
            _position.Y += 65;

            float xVers =  -mouse.X + _position.X;
            float yVers =  -mouse.Y + _position.Y;
            angle = (float)Math.Atan2(xVers,yVers) + (float) (Math.PI/2);
            Console.WriteLine(angle);

            foreach(Bullet b in bullets)
            {
                b.Update(gameTime);
            }
        }

        public void Update(GameTime gameTime, Vector2 position, int dir)
        {
            _position = position;
            _position.X += 50;
            _position.Y += 65;
            if(dir == 0)
            {
                angle = (float)(Math.PI);
            }
            else if(dir == 1)
            {
                angle = 0;
            }
            foreach (Bullet b in bullets)
            {
                b.Update(gameTime);
            }
        }

        public override void Fire()
        {
            //bullet.fire(angle, _position);
            Console.WriteLine("checking bullet");
            int i = 0;
            while((i != -1))
            {
                Console.WriteLine("searching");
                if (!bullets[i].isFired)
                {
                    bullets[i].fire(angle, _position);
                    Console.WriteLine("bullet Fired");
                    i = -1;
                }
                else
                {
                    i++;
                    if(bullets.Count <= i)
                    {
                        i = -1;
                        Console.WriteLine("bullet not available");
                    }
                }
            }
        }

        public List<Rectangle> getBulletsRect()
        {
            List<Rectangle> bulletsRect = new List<Rectangle>();
            foreach (BulletBlueprint b in bullets)
            {
                bulletsRect.Add(b.CollisionRect);
            }
            return bulletsRect;
        }

        public override List<Bullet> getBullets()
        {
            return bullets;
        }

    }



    class RobotMeleeARM //Dit is een melee attack
    {
        private Texture2D pixel;
        private float angle;
        private Vector2 _position;
        bool isAttacking;
        private MeleeBullet meleeBullet;

        public RobotMeleeARM(Texture2D pix)
        {
            angle = 0;
            pixel = pix;
            _position = new Vector2(200, 240);
            meleeBullet = new MeleeBullet(pix, _position,new Vector2(80,80),1,2);
        }
        public void Update(GameTime gameTime, Vector2 position, int dir)
        {
            _position = position;
            _position.X += 50;
            _position.Y += 70 + ((dir-1) * -20);
            if (isAttacking)
            {
                angle += (float)((Math.PI / 180)*gameTime.ElapsedGameTime.TotalMilliseconds*2);
            }
            else
            {
                angle = (float)((Math.PI) * (dir-1));
            }
            isAttacking = false;
            meleeBullet.Update(angle, _position, isAttacking);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, 81, 36);
            Vector2 origin = new Vector2(5, 10);
            spriteBatch.Draw(pixel, _position, sourceRectangle, Color.White, angle, origin, 1.0f, SpriteEffects.None, 1);
            
        }

        public void Fire()
        {
            isAttacking = true;
        }

        public Rectangle attackBox()
        {
            return meleeBullet.getCollisonBox();
        }

    }
}

