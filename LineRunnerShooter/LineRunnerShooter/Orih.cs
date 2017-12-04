using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LineRunnerShooter
{
    class Orih : User
    {
        //TODO: attack modes: spinning arms (done), shooting
        public bool isAlive;
        public bool seePlayer;
        private Rectangle attackBox;
        private bool isAttacking;
        private int lastMove;
        private Texture2D _Texture;
        protected RobotARM robotARM;


        public Orih(Texture2D textureL, Texture2D textureR, MoveMethod move, Texture2D bullet, int posX) : base(textureL, textureR, move, bullet)
        {
            _spritePos = new Rectangle(posX, 0, 60, 200);
            _Position.X = posX;
            _StartPos.X = posX;
            arm = null;
            isAlive = true;
            isAttacking = false;
            _lives = 3;
            attackBox = new Rectangle(posX, 0, 60, 60);
            _Texture = textureL;
            collisionBox = new RoboCollisionBox(Convert.ToInt16(_Position.X), Convert.ToInt16(_Position.Y), _spritePos.Width, _spritePos.Height);
            robotARM = new RobotARM(bullet);
            maxSpeed = 8;
        }
        public Orih(Texture2D textureL, Texture2D textureR, MoveMethod move, Texture2D armpix, Texture2D bullet, int posX) : base(textureL, textureR, move, bullet)
        {
            _spritePos = new Rectangle(posX, 0, 60, 200);
            _Position.X = posX;
            _StartPos.X = posX;
            arm = null;
            isAlive = true;
            isAttacking = false;
            _lives = 3;
            attackBox = new Rectangle(posX, 0, 60, 60);
            _Texture = textureL;
            collisionBox = new RoboCollisionBox(Convert.ToInt16(_Position.X), Convert.ToInt16(_Position.Y), _spritePos.Width, _spritePos.Height);
            robotARM = new RobotARM(armpix);
            maxSpeed = 8;
        }
        public void Update(GameTime gameTime, KeyboardState stateKey, bool isHit)
        {
            if (isAlive)
            {
                base.Update(gameTime, stateKey);
                
                if (isHit)
                {
                    Console.WriteLine("I am hit");
                    _lives--;
                }
                if (isAttacking)
                {
                    Console.WriteLine("Attacing");
                    robotARM.Fire();
                    attackBox.Location = _Position.ToPoint();
                    attackBox.Y -= 30;
                }
                if (lastMove != _MoveMethod.Movedir)
                {
                    isAttacking = false;
                    maxSpeed = 8;
                }
                if (_lives <= 0)
                {
                    Reset();
                }
                robotARM.Update(gameTime, _Position, _MoveMethod.Movedir);
            }
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            _spritePos = new Rectangle(0, 0, 60, 200);
            if (isAlive)
            {
                base.draw(spriteBatch);
                robotARM.Draw(spriteBatch);
            }
            if (isAttacking)
            {
                //spriteBatch.Draw(_Texture, getAttackRect(), Color.Red);
            }
        }

        public void SeePlayer(Rectangle player)
        {
            Rectangle _ViewRectangle;
            if (_MoveMethod.Movedir == 1)
            {
                _ViewRectangle = new Rectangle(Convert.ToInt16(_Position.X), Convert.ToInt16(_Position.Y) + 25, 150, 100);
            }
            else
            {
                _ViewRectangle = new Rectangle(Convert.ToInt16(_Position.X) - 240, Convert.ToInt16(_Position.Y) + 25, 150, 100);
            }

            if (player.Intersects(_ViewRectangle))
            {
                seePlayer = true;
                Console.WriteLine("I see you");
                Attack();
            }
            else
            {
                seePlayer = false;
            }
        }
        public void Attack()
        {
            isAttacking = true;
            maxSpeed = 20;
            lastMove = _MoveMethod.Movedir;
        }

        public override void Reset()
        {
            isAlive = false;
            gravity = 0;
            maxSpeed = 0;
            _Position = new Vector2(500, 6000);
        }

        public Rectangle getAttackRect()
        {
            Rectangle rectOut = new Rectangle();
            if (isAttacking)
            {
                rectOut = robotARM.attackBox();
            }
            return rectOut;

        }

        public override void checkEnviroments(List<Rectangle> level)
        {
            RoboCollisionBox roboBox = collisionBox as RoboCollisionBox;
            base.checkEnviroments(level);
            foreach(Rectangle rect in level)
            {
                if (rect.Intersects(roboBox.SenseLeft) && canLeft)
                {
                    canLeft = false;
                }
                if (rect.Intersects(roboBox.SenseRight) && canRight)
                {
                    canRight = false;
                }
            }
        }
    }
        class RobotMove : MoveMethod
        {
            private Random r;

            public RobotMove()
            {
                r = new Random();
                movedir = 0;
            }

            public override void Update(KeyboardState stateKey, bool canLeft, bool canRight)
            {
                int move = r.Next(0, 2);
                if (canLeft && movedir == 0)
                {
                    movedir = 1;
                }
                if (canRight && movedir == 1)
                {
                    movedir = 0;
                }
            }
        }
    
}
