using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LineRunnerShooter.Weapons.Bullets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LineRunnerShooter
{
    /*
     * Big Boy is the first boss of this game, his actions are seperated in 3 fases, first he waits for the player to reach a certain point, then he follows the player and attack him with rockets, 
     * he can not be hit, last phase is that he drops on the battleground and starts rushing around and keep spamming the rockets
     * after his death, the lift will work, so you cannot escape by running away
     * 
     */ 
    class BigBoy : Enemy
    {
        //TODO: More scripted actions: begin when hiro is on X, enter battle scene on X, ...
        //TODO: More attack modes + smaller hitbox, only face => hiro needs the challange to aim
        private List<BulletR> rockets;
        private int firedRockets;
        private Random r;
        private int phase;
        private double elapsedTime;

        public bool IsAlive { get {
                bool tmp = true;
                if (!isAlive || phase == 3) tmp = false;
                return tmp;
            } }
        public BigBoy(int textureL, MoveMethod move, Texture2D armpix, Texture2D bullet, Vector2 pos) : base(textureL, move, armpix, bullet, pos)
        {
            rockets = new List<BulletR>() {
            new BulletR(new Point(50, 100)),
            new BulletR(new Point(50, 100)),
            new BulletR(new Point(50, 100)),
            new BulletR(new Point(50, 100)),
            new BulletR(new Point(50, 100)),
            new BulletR(new Point(50, 100)),
            new BulletR(new Point(50, 100)),
            new BulletR(new Point(50, 100))
            };
            time = 3;
            r = new Random();
            _lives = 30;
            phase = 0;
            _spritePos.Size = new Point(120, 200);
        }


        public void Update(GameTime gameTime, KeyboardState stateKey, Rectangle player, List<BulletBlueprint> bullets)
        {
            
            bool isHit = false;
            switch (phase)
            {
                case 0:
                    {
                        if(player.X > 600 && player.Y > 1000)
                        {
                            phase++;
                            robotARM.SetDamage(0);
                        }
                        _position.X = 600;
                        _position.Y = 2000;
                        robotARM.Update(gameTime, _position, _moveMethod.Movedir);
                        break;
                    }
                case 1:
                    {
                        //base.Update(gameTime, stateKey);
                        _position.X += (player.X - _position.X + 400) / 8;
                        
                        _position.Y = (float) (1350 + (Math.Sin(Convert.ToInt32(gameTime.TotalGameTime.TotalMilliseconds/100)))*8);
                        elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                        if (player.X > 6600)
                        {
                            phase++;
                            _position.X = player.X+800;
                            isGrounded = false;
                            robotARM.SetDamage(5);
                        }
                        if(elapsedTime > 1000)
                        {
                            elapsedTime = 0;
                            Attack(player.Location.ToVector2());
                            Attack(player.Location.ToVector2());
                            Attack(player.Location.ToVector2());
                        }
                        foreach (BulletR b in rockets)
                        {
                            b.Update();
                        }
                        robotARM.Update(gameTime, _position, _moveMethod.Movedir);
                        _spritePos.Location = new Point(0, 400);
                        break;
                    }
                case 2:
                    {
                        base.Update(gameTime, stateKey, bullets);
                        elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                        if(General.random.Next(0,100)> 95)
                        {
                            Attack();
                        }
                        if (isAlive)
                        {
                            if (elapsedTime > 250)
                            {
                                elapsedTime = 0;
                                Attack(new Vector2(5000,600));
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
                                _position = new Vector2(200, 5000);
                                robotARM.Disable();
                                foreach(BulletR r in rockets)
                                {
                                    r.ResetBullet();
                                }
                            }
                            SeePlayer(player);
                        }
                        
                        break;
                    }
                case 3:
                    {
                        base.Update(gameTime, stateKey, bullets);
                        elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                        if (isAlive)
                        {
                            robotARM.Disable();
                            foreach (BulletR r in rockets)
                            {
                                r.ResetBullet();
                            }
                            if (isHit)
                            {
                                _lives--;
                            }
                            if (_lives <= 0)
                            {
                                isAlive = false;
                                _position = new Vector2(200, 5000);
                                robotARM.Disable();
                                foreach (BulletR r in rockets)
                                {
                                    r.ResetBullet();
                                }
                            }
                            SeePlayer(player);
                        }

                        break;
                    }
            }
        }

        private void Attack(Vector2 player)
        {
            player.X -= 600;
            Vector2 firePos = new Vector2(player.X + r.Next(100, 3000), 0);
            rockets[firedRockets].Fire(_position, firePos);
            firedRockets++;
            if(firedRockets >= rockets.Count)
            {
                firedRockets = 0;
            }
            Console.WriteLine(firePos.ToString());
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (isAlive)
            {
                foreach (BulletR b in rockets)
                {
                    b.Draw(spriteBatch);
                }
            }

        }

        public List<Rectangle> GetBulletsRect()
        {
            List<Rectangle> rocketsRect = new List<Rectangle>();
            foreach(BulletR br in rockets)
            {
                rocketsRect.Add(br.GetCollisionRectagle());
            }
            return rocketsRect;
        }

        public override List<BulletBlueprint> GetBullets()
        {
            List<BulletBlueprint> bullets = new List<BulletBlueprint>();
            bullets.AddRange(rockets);
            bullets.AddRange(robotARM.Bullets);
            return bullets;
        }

        protected override void Sheep()
        {
            base.Sheep();
            phase = 3;
        }

    }
}
