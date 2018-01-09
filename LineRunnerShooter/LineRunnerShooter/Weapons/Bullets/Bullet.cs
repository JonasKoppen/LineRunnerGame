using LineRunnerShooter.Weapons.Bullets;
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
     * Bullet class, contains different bullet types (not all are realy a bullet but naming is not a priority)
     * A bullet requires a position, collisionRectangle (1 will do),  a state and a TimeToLive (because working with vision boxes is not a good idea, trust me, I tried)
     * should add destroy on blockhit
     */ 



  
    class Bullet : BulletBlueprint
    {
        public Vector2 _direction; //speed is always constant here so, no velocity
        private double timeToLive; //works better than with

        public Bullet(Texture2D texture, int damage) : base(texture, new Vector2(0,0), new Vector2(50,50), damage)
        {
            timeToLive = 0;
        }
        public Bullet(Texture2D texture, Point size, int damage) : base(texture, new Vector2(0, 0), size.ToVector2(), damage)
        {
            timeToLive = 0;
        }

         
        
        public void Fire(Vector2 pos)
        {
            if (!IsFired)
            {
                _positie = pos;
                _direction.X = 0;
                _direction.Y = 10;
                IsFired = true;
                timeToLive = 1000;
            }

        }
        public void Fire(float angle, Vector2 pos)
        {
            if (!IsFired)
            {
                _positie = pos;
                _direction.X = (float)(Math.Cos(angle) * 1.5);
                _direction.Y = -(float)(Math.Sin(angle) * 1.5);
                IsFired = true;
                timeToLive = 1000;
            }

        }

        public void Update(GameTime gameTime)
        {
            timeToLive -= gameTime.ElapsedGameTime.TotalMilliseconds;
            if (IsFired)
            {
                _positie.X += Convert.ToInt32(gameTime.ElapsedGameTime.TotalMilliseconds * _direction.X);
                _positie.Y += Convert.ToInt32(gameTime.ElapsedGameTime.TotalMilliseconds * _direction.Y);
                if ((timeToLive < 0))
                {
                    HitTarget();
                }
            }
            else
            {
                _positie = new Vector2(0, 1000);
            }

            
        }

        public override int HitTarget() //aka the reset function, returns the damage
        {
            IsFired = false;
            return base.HitTarget();
        }
    }

   

   


}
