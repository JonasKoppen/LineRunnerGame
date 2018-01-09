using LineRunnerShooter.Weapons;
using LineRunnerShooter.Weapons.Bullets;
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



   

   
    
}

