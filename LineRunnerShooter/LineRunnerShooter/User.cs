using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineRunnerShooter
{
    class User : ICollide
    {
        protected int time; //for update 
        protected double lastTime;

        protected List<Texture2D> _texture; //textures
        protected Rectangle _spritePos;     //texture pos
        protected int _Action;              //actioin movement
        public Vector2 _Position;        //position
        protected MoveMethod _MoveMethod;   //movemethod 
        protected int speedX;
        protected int gravity;
        protected int _lives;



        public bool canLeft;                //true = mag naar links
        public bool canRight;               //true = mag naar rechts
        public bool isGrounded;             //true = ik sta op de grond


        public Rectangle _CollisionRight;   //for move right
        public Rectangle _CollisionLeft;    //for move left
        protected Rectangle _CollisionRect; //hit box
        protected Rectangle feetCollisionRect; //bepaald isGrounded gebruik voor collisie met de grond

        protected ARM arm;

        public Vector2 Location { get { return _Position; } }

        public User(Texture2D textureL, Texture2D textureR, MoveMethod move, Texture2D bullet)
        {
            _texture = new List<Texture2D>();
            _texture.Add(textureL);
            _texture.Add(textureR);
            _Action = 0;
            _Position = new Vector2(0, 500);
            time = 0;
            _MoveMethod = move;
            _spritePos = new Rectangle(0, 90, 100, 200);
            feetCollisionRect = new Rectangle(0, 0, 80, 10);
            isGrounded = false;
            _CollisionRight = new Rectangle(0, 0, 30, 20);
            _CollisionLeft = new Rectangle(30, 0, 30, 20);
            canLeft = true;
            canRight = true;
            speedX = 3;
            _lives = 5;
            lastTime = 0;
        }

        public virtual void Update(GameTime gameTime, KeyboardState stateKey)
        {
            int totalTime = Convert.ToInt32(gameTime.TotalGameTime.TotalMilliseconds);
            if (totalTime > time)
            {
                time += 10;
                _spritePos.X += 100;
                if (_spritePos.X > 700)
                {
                    _spritePos.X = 0;
                }
            }
            UpdateFI(gameTime, stateKey);
            if (_Position.Y > 3000 && (_Position.X > 500 || _Position.X <0))
            {
                Reset();
            }
            if (isGrounded)
            {
                gravity = 0;
            }
            
        }

        public virtual void Update(GameTime gameTime, KeyboardState stateKey, MouseState mouseState, Vector2 camPos)
        {
            Vector2 mousePos = mouseState.Position.ToVector2();
            arm.Update(gameTime, _Position, mousePos, camPos);
            Update(gameTime, stateKey);
        }

        public virtual void UpdateFI(GameTime gameTime, KeyboardState stateKey)
        {
            double dt = gameTime.TotalGameTime.TotalMilliseconds - lastTime;
            _MoveMethod.Update(stateKey, canLeft, canRight);
            MoveHorizontal(dt);
            MoveVertical(dt);
            lastTime = gameTime.TotalGameTime.TotalMilliseconds;

        }

        public virtual void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture[_Action], _Position, _spritePos, Color.White);

        }

        protected virtual void MoveHorizontal()
        {
            _spritePos.X += 100;
            if (_spritePos.X > 700)
            {
                _spritePos.X = 0;
            }
            switch (_MoveMethod.Movedir)
            {
                case (0):
                    _Position.X -= speedX;
                    _Action = 0;
                    break;
                case (1):
                    _Position.X += speedX;
                    _Action = 1;
                    break;
                default:
                    _spritePos.X = 0;
                    break;
            }
        }
        protected virtual void MoveHorizontal(double time)
        {
            switch (_MoveMethod.Movedir)
            {
                case (0):
                    _Position.X -= Convert.ToInt16(time /2);
                    _Action = 0;
                    break;
                case (1):
                    _Position.X += Convert.ToInt16(time /2);
                    _Action = 1;
                    break;
                default:
                    _spritePos.X = 0;
                    break;
            }
        }

        public virtual void MoveVertical()
        {
            if (!isGrounded)
            {
                _Position.Y += 1 + (gravity / 2);
                gravity++;
            }
        }

        public virtual void MoveVertical(double time)
        {
            if (!isGrounded)
            {
                _Position.Y += Convert.ToInt16((time / 4) + (gravity / 2)); 
                gravity++;
            }

        }

        public Rectangle getCollisionRectagle()
        {
            _CollisionRect.Location = _Position.ToPoint();
            return _CollisionRect;
        }

        public virtual Rectangle getFeetCollisionRect()
        {
            feetCollisionRect.Location = _Position.ToPoint();
            feetCollisionRect.X += 10;
            feetCollisionRect.Y += 190;
            return feetCollisionRect;
        }

        public Rectangle getRightCollision()
        {
            _CollisionRight.Location = _Position.ToPoint();
            _CollisionRight.X += 30;
            _CollisionRight.Y += 170;
            return _CollisionRight;

        }

        public Rectangle getLeftCollision()
        {
            _CollisionLeft.Location = _Position.ToPoint();
            _CollisionLeft.Y += 170;
            return _CollisionLeft;
        }

        public void PlatformUpdate(int platform)
        {
            _Position.Y += platform;
        }

        public bool getBulletsCollision(Rectangle target)
        {
            bool isHit = false;
            foreach (Bullet b in arm.bullets)
            {
                if (target.Intersects(b.getCollisionRectagle()))
                {
                    isHit = true;
                    b.HitTarge();
                }
            }
            return isHit;
        }

        public void Reset()
        {
            _Position = new Vector2(500, 300);
        }
    }

    public abstract class MoveMethod
    {
        protected int movedir;
        public bool isJump;
        public bool isShooting;
        public abstract void Update(KeyboardState stateKey, bool moveLeft, bool moveRight);

        public int Movedir { get { return movedir; } protected set { movedir = value; } }
    }
}
