using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LineRunnerShooter
{
    class BigBoy : Orih
    {
        private bool attackMode;
        private List<BulletR> rockets;
        private int firedRockets;
        private Random r;
        public BigBoy(Texture2D textureL, Texture2D textureR, MoveMethod move, Texture2D bullet, int posX) : base(textureL, textureR, move, bullet, posX)
        {
            
            rockets = new List<BulletR>();
            rockets.Add(new BulletR(bullet, new Point(50, 50)));
            rockets.Add(new BulletR(bullet, new Point(50, 50)));
            rockets.Add(new BulletR(bullet, new Point(50, 50)));
            rockets.Add(new BulletR(bullet, new Point(50, 50)));
            rockets.Add(new BulletR(bullet, new Point(50, 50)));
            rockets.Add(new BulletR(bullet, new Point(50, 50)));
            rockets.Add(new BulletR(bullet, new Point(50, 50)));
            rockets.Add(new BulletR(bullet, new Point(50, 50)));
            time = 3;
            r = new Random();
            _lives = 5;
        }


        public void Update(GameTime gameTime, KeyboardState stateKey, Vector2 player, List<Rectangle> heroBullets)
        {
            base.Update(gameTime, stateKey);
            bool isHit = false;
            foreach(Rectangle bullet in heroBullets)
            {
                if (!isHit)
                {
                    isHit = getCollisionRectagle().Intersects(bullet);
                }
            }
            if (isAlive)
            {
                if (true) //gameTime.TotalGameTime.Seconds > time
                {
                    time = gameTime.TotalGameTime.Seconds;
                    attackMode = true;
                    //firedRockets = 0;
                }
                if (attackMode)
                {
                    Attack(player);
                }
                foreach (BulletR b in rockets)
                {
                    b.Update();
                }
                if (isHit)
                {
                    _lives--;
                }
                if (_lives <= 0)
                {
                    isAlive = false;
                }
            }

            

        }

        private void Attack(Vector2 player)
        {
            player.X -= 600;
            Vector2 firePos = new Vector2(player.X + r.Next(100, 3000), 0);
            rockets[firedRockets].fire(_Position, firePos);
            firedRockets++;
            firePos = new Vector2(player.X + r.Next(200, 2000), 0);
            rockets[firedRockets].fire(_Position, firePos);
            firedRockets++;
            if(firedRockets >= rockets.Count)
            {
                attackMode = false;
                firedRockets = 0;
            }
            Console.WriteLine(firePos.ToString());
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);
            if (isAlive)
            {
                foreach (BulletR b in rockets)
                {
                    b.Draw(spriteBatch);
                }
            }

        }



    }
}
