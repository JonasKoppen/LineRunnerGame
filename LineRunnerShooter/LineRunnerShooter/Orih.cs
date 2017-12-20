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
            _position = pos;
            _startPos = pos;
            isAlive = true;
            isAttacking = false;
            _lives = 3;
            collisionBox = new RoboCollisionBox(Convert.ToInt16(_position.X), Convert.ToInt16(_position.Y), _spritePos.Width, _spritePos.Height);
            robotARM = new RobotMeleeARM(bullet);
            maxSpeed = 8;
        }
        public Orih(int textureL, MoveMethod move, Texture2D armpix, Texture2D bullet, Vector2 pos) : base(textureL, move, bullet)
        {
            _spritePos = new Rectangle(pos.ToPoint(), new Point(60,200));
            _position = pos;
            _startPos = pos;
            isAlive = true;
            isAttacking = false;
            _lives = 3;
            collisionBox = new RoboCollisionBox(Convert.ToInt16(_position.X), Convert.ToInt16(_position.Y), _spritePos.Width, _spritePos.Height);
            robotARM = new RobotMeleeARM(armpix);
            maxSpeed = 8;
            robotARM.SetDamage(5);
        }
        public Orih(int textureL,Rectangle spritePos, MoveMethod move, Texture2D armpix, Texture2D bullet, Vector2 pos) : base(textureL, move, bullet)
        {
            _spritePos = spritePos;
            _position = pos;
            _startPos = pos;
            isAlive = true;
            isAttacking = false;
            _lives = 10;
            collisionBox = new RoboCollisionBox(Convert.ToInt16(_position.X), Convert.ToInt16(_position.Y), _spritePos.Width, _spritePos.Height);
            robotARM = new RobotMeleeARM(armpix);
            maxSpeed = 8;
            robotARM.SetDamage(5);
        }
        public override void Update(GameTime gameTime, KeyboardState stateKey, List<BulletBlueprint> bullets)
        {
            
            if (isAlive)
            {
                _moveMethod.Update(stateKey, new MouseState(), canLeft, canRight);
                base.Update(gameTime, stateKey, bullets);
                
                if (isAttacking)
                {
                    Console.WriteLine("Attacing");
                    robotARM.Fire();
                }
                if (lastMove != _moveMethod.Movedir)
                {
                    isAttacking = false;
                    maxSpeed = 8;
                }
                if (_lives <= 0)
                {
                    Reset();
                }
            }
            else
            {
                _position = new Vector2(100, 5000);
            }
            robotARM.Update(gameTime, _position, _moveMethod.Movedir);
            CheckHit(bullets);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _spritePos.X = 0;
            if (isAlive)
            {
                base.Draw(spriteBatch);
                //robotARM.Draw(spriteBatch); //Arm is te lelijk
            }
            if (isAttacking)
            {
                //spriteBatch.Draw(_Texture, getAttackRect(), Color.Red);
            }
        }

        public void SeePlayer(Rectangle player)
        {
            Rectangle _ViewRectangle;
            if (_moveMethod.Movedir == 1)
            {
                _ViewRectangle = new Rectangle(Convert.ToInt16(_position.X), Convert.ToInt16(_position.Y) + 25, 150, 100);
            }
            else
            {
                _ViewRectangle = new Rectangle(Convert.ToInt16(_position.X) - 240, Convert.ToInt16(_position.Y) + 25, 150, 100);
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
            lastMove = _moveMethod.Movedir;
        }

        public override void Reset()
        {
            isAlive = false;
            gravity = 0;
            maxSpeed = 0;
            _position = new Vector2(500, 6000);
        }

        public override void CheckEnviroments(List<Rectangle> level)
        {
            RoboCollisionBox roboBox = collisionBox as RoboCollisionBox;
            base.CheckEnviroments(level);
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
        protected override void Sheep()
        {
            base.Sheep();
            robotARM.Disable();
            robotARM.SetDamage(0);
        }

        public virtual List<BulletBlueprint> GetBullets()
        {
            return robotARM.Bullets;
        }
        
    }
        class RobotMove : MoveMethod
        {

            public RobotMove()
            {
                movedir = 0;
            }

            public void Update(bool canLeft, bool canRight)
            {
            if (General.random.Next(0,1000) > 995)
            {
                ChangeDir();
            }
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

        public void ChangeDir()
        {
            movedir = Math.Abs(movedir - 1);
        }

        
    }
    
}
