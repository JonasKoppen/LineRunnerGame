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
        //public List<BulletBlueprint> bullets; // Dit is niet juist
        protected Rectangle sourceRectangle;
        private Vector2 size;

        public virtual List<BulletBlueprint> Bullets { get; protected set; }

        public ARMBluePrint(Texture2D texture)
        {
            _texture = texture;
            sourceRectangle = new Rectangle(0, 0, 80, 36);
        }

        public ARMBluePrint(Texture2D texture, Vector2 pos, Vector2 size) : this(texture)
        {
            this._position = pos;
            this.size = size;
            sourceRectangle = new Rectangle(0, 0, 80, 36);
        }

        public abstract void Update(GameTime gameTime, Vector2 position, Vector2 mouse);
        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2(5, 10);
            spriteBatch.Draw(_texture, _position, sourceRectangle, Color.White, -angle, origin, 1.0f, SpriteEffects.None, 1);
            foreach (BulletBlueprint b in Bullets)
            {
                b.Draw(spriteBatch);
            }
        }
        public abstract void Fire();

    }

    class ShotARM : ARMBluePrint //only has X active bullets
    {

        public ShotARM(Texture2D pix, Texture2D energy, int amountBullets, int damage) : base(pix)
        {
            angle = 0;
            _position = new Vector2(200, 240);
            Bullets = new List<BulletBlueprint>();
            sourceRectangle.Y = 35;
            if (amountBullets > 0)
            {
                for(int i = 0; i < amountBullets; i++)
                {
                    Bullets.Add(new Bullet(energy, damage)); //First gun can only fire 1 bullet, make a variation with more bullets and selection key
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
            //Console.WriteLine(angle);

            foreach(Bullet b in Bullets)
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
            foreach (Bullet b in Bullets)
            {
                b.Update(gameTime);
            }
        }

        public override void Fire()
        {
            //bullet.fire(angle, _position);
            //Console.WriteLine("checking bullet");
            int i = 0;
            while((i != -1))
            {
                //Console.WriteLine("searching");
                if (!Bullets[i].IsFired)
                {
                    (Bullets[i] as Bullet).Fire(angle, _position);
                    //Console.WriteLine("bullet Fired");
                    i = -1;
                }
                else
                {
                    i++;
                    if(Bullets.Count <= i)
                    {
                        i = -1;
                        //Console.WriteLine("bullet not available");
                    }
                }
            }
        }

        public List<Rectangle> GetBulletsRect()
        {
            List<Rectangle> bulletsRect = new List<Rectangle>();
            foreach (BulletBlueprint b in Bullets)
            {
                bulletsRect.Add(b.CollisionRect);
            }
            return bulletsRect;
        }

    }



    class RobotMeleeARM //Dit is een melee attack
    {
        private Texture2D pixel;
        private float angle;
        private Vector2 _position;
        bool isAttacking;
        private MeleeBullet meleeBullet;

        public List<BulletBlueprint> Bullets { get { return GetBullets(); }}

        public RobotMeleeARM(Texture2D pix)
        {
            angle = 0;
            pixel = pix;
            _position = new Vector2(200, 240);
            meleeBullet = new MeleeBullet(pix, _position,new Vector2(80,80),1);
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

        private List<BulletBlueprint> GetBullets()
        {
            List<BulletBlueprint> bullets = new List<BulletBlueprint>() {
                                                            meleeBullet
                                                            };
            return bullets;
        }

        public void SetDamage(int damage)
        {
            meleeBullet.SetDamage(damage);
        }

        public void Disable()
        {
            meleeBullet.ResetBullet();
        }
    }

    class FlameThrower : ARMBluePrint
    {
        bool isFired;
        public FlameThrower(Texture2D texture,Texture2D flame) : base(texture)
        {
            Bullets = new List<BulletBlueprint>();
            isFired = false;
            for(int i =1; i < 10; i++)
            {
                Bullets.Add(new Flame(flame, new Vector2(0,1000), new Vector2(25, 25), i%2, i));
            }
            sourceRectangle.Y = 70;
        }

        public override void Fire()
        {
            isFired = true;
        }

        public override void Update(GameTime gameTime, Vector2 position, Vector2 mouse)
        {
            _position = position;
            _position.X += 40;
            _position.Y += 65;

            float xVers = -mouse.X + _position.X;
            float yVers = -mouse.Y + _position.Y;
            angle = (float)Math.Atan2(xVers, yVers) + (float)(Math.PI / 2);
            //Console.WriteLine(angle);

            foreach (Flame f in Bullets)
            {
                f.Update(_position,angle,isFired);
            }
            isFired = false;
        }
    }
    class SheepANator : ARMBluePrint
    {
        bool isFired;
        public SheepANator(Texture2D texture, Texture2D flame) : base(texture)
        {
            Bullets = new List<BulletBlueprint>();
            isFired = false;
            for (int i = 1; i < 10; i++)
            {
                Bullets.Add(new SheepBeam(flame, new Vector2(0, 1000), new Vector2(25, 25), i % 2, i));
            }
            sourceRectangle.Y = 115;
        }

        public override void Fire()
        {
            isFired = true;
        }


        public override void Update(GameTime gameTime, Vector2 position, Vector2 mouse)
        {
            _position = position;
            _position.X += 40;
            _position.Y += 65;

            float xVers = -mouse.X + _position.X;
            float yVers = -mouse.Y + _position.Y;
            angle = (float)Math.Atan2(xVers, yVers) + (float)(Math.PI / 2);
            //Console.WriteLine(angle);

            foreach (SheepBeam f in Bullets)
            {
                f.Update(_position, angle, isFired);
            }
            isFired = false;
        }
    }
}

