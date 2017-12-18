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
    /*
     * The enemy class
     * beter shoot them before they see you and rush forward
     * 
     * 
     */ 
    class Orih : User
    {
        //TODO: attack modes: spinning arms (done), shooting
        public bool isAlive;
        public bool seePlayer;
        private bool isAttacking;
        private int lastMove;
        protected RobotMeleeARM robotARM;


        public Orih(int textureL, MoveMethod move, Texture2D bullet, Vector2 pos) : base(textureL, move, bullet)
        {
            _spritePos = new Rectangle(pos.ToPoint(), new Point(60, 200));
            _Position = pos;
            _StartPos = pos;
            isAlive = true;
            isAttacking = false;
            _lives = 3;
            collisionBox = new RoboCollisionBox(Convert.ToInt16(_Position.X), Convert.ToInt16(_Position.Y), _spritePos.Width, _spritePos.Height);
            robotARM = new RobotMeleeARM(bullet);
            maxSpeed = 8;
        }
        public Orih(int textureL, MoveMethod move, Texture2D armpix, Texture2D bullet, Vector2 pos) : base(textureL, move, bullet)
        {
            _spritePos = new Rectangle(pos.ToPoint(), new Point(60,200));
            _Position = pos;
            _StartPos = pos;
            isAlive = true;
            isAttacking = false;
            _lives = 3;
            collisionBox = new RoboCollisionBox(Convert.ToInt16(_Position.X), Convert.ToInt16(_Position.Y), _spritePos.Width, _spritePos.Height);
            robotARM = new RobotMeleeARM(armpix);
            maxSpeed = 8;
        }
        public Orih(int textureL,Rectangle spritePos, MoveMethod move, Texture2D armpix, Texture2D bullet, Vector2 pos) : base(textureL, move, bullet)
        {
            _spritePos = spritePos;
            _Position = pos;
            _StartPos = pos;
            isAlive = true;
            isAttacking = false;
            _lives = 3;
            collisionBox = new RoboCollisionBox(Convert.ToInt16(_Position.X), Convert.ToInt16(_Position.Y), _spritePos.Width, _spritePos.Height);
            robotARM = new RobotMeleeARM(armpix);
            maxSpeed = 8;
        }
        public void Update(GameTime gameTime, KeyboardState stateKey, List<BulletBlueprint> bullets)
        {
            if (isAlive)
            {
                _MoveMethod.Update(stateKey, new MouseState(), canLeft, canRight);
                base.Update(gameTime, stateKey, bullets);
                
                if (isAttacking)
                {
                    Console.WriteLine("Attacing");
                    robotARM.Fire();
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
            _spritePos.X = 0;
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
        
        public virtual List<BulletBlueprint> getBullets()
        {
            return robotARM.getBullets();
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

            public void Update(bool canLeft, bool canRight)
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

        public override void Update(KeyboardState keyState, MouseState mouseState, bool _canLeft, bool _canRight)
        {
            Update(_canLeft, _canRight);
        }
    }
    
}
