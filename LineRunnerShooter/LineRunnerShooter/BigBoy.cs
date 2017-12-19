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
    /*
     * Big Boy is the first boss of this game, his actions are seperated in 3 fases, first he waits for the player to reach a certain point, then he follows the player and attack him with rockets, 
     * he can not be hit, last phase is that he drops on the battleground and starts rushing around and keep spamming the rockets
     * after his death, the lift will work, so you cannot escape by running away
     * 
     */ 
    class BigBoy : Orih
    {
        //TODO: More scripted actions: begin when hiro is on X, enter battle scene on X, ...
        //TODO: More attack modes + smaller hitbox, only face => hiro needs the challange to aim
        private List<BulletR> rockets;
        private int firedRockets;
        private Random r;
        private int phase;
        private double elapsedTime;
        public BigBoy(int textureL, MoveMethod move,Texture2D armpix, Texture2D bullet, Vector2 pos) : base(textureL, move,armpix, bullet, pos)
        {
            rockets = new List<BulletR>();
            rockets.Add(new BulletR( new Point(50, 100)));
            rockets.Add(new BulletR( new Point(50, 100)));
            rockets.Add(new BulletR( new Point(50, 100)));
            rockets.Add(new BulletR( new Point(50, 100)));
            rockets.Add(new BulletR( new Point(50, 100)));
            rockets.Add(new BulletR( new Point(50, 100)));
            rockets.Add(new BulletR( new Point(50, 100)));
            rockets.Add(new BulletR( new Point(50, 100)));
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
                            robotARM.setDamage(0);
                        }
                        _Position.X = 600;
                        _Position.Y = 2000;
                        robotARM.Update(gameTime, _Position, _MoveMethod.Movedir);
                        break;
                    }
                case 1:
                    {
                        //base.Update(gameTime, stateKey);
                        _Position.X += (player.X - _Position.X + 400) / 8;
                        
                        _Position.Y = (float) (1350 + (Math.Sin(Convert.ToInt32(gameTime.TotalGameTime.TotalMilliseconds/100)))*8);
                        elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                        if (player.X > 4700)
                        {
                            phase++;
                            _Position.X = player.X+600;
                            isGrounded = false;
                            robotARM.setDamage(5);
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
                        robotARM.Update(gameTime, _Position, _MoveMethod.Movedir);
                        _spritePos.Location = new Point(0, 400);
                        break;
                    }
                case 2:
                    {
                        base.Update(gameTime, stateKey, bullets);
                        elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                        if (isAlive)
                        {
                            if (elapsedTime > 200 && r.Next(100) > 95)
                            {
                                (_MoveMethod as RobotMove).changeDir();
                            }
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
                                _Position = new Vector2(200, 5000);
                                robotARM.disable();
                                foreach(BulletR r in rockets)
                                {
                                    r.resetBullet();
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
            rockets[firedRockets].fire(_Position, firePos);
            firedRockets++;
            if(firedRockets >= rockets.Count)
            {
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

        public List<Rectangle> getBulletsRect()
        {
            List<Rectangle> rocketsRect = new List<Rectangle>();
            foreach(BulletR br in rockets)
            {
                rocketsRect.Add(br.getCollisionRectagle());
            }
            return rocketsRect;
        }

        public override List<BulletBlueprint> getBullets()
        {
            List<BulletBlueprint> bullets = new List<BulletBlueprint>();
            bullets.AddRange(rockets);
            bullets.AddRange(robotARM.getBullets());
            return bullets;
        }



    }
}
