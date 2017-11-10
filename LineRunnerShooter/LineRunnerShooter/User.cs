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
        protected int time;
        protected List<Texture2D> _texture;
        protected Rectangle _spritePos;
        protected int _Action;
        protected Vector2 _Position;
        protected MoveMethod _MoveMethod;
        protected Rectangle feetCollisionRect;
        protected Rectangle _CollisionRect;
        public bool isGrounded;
        protected ARM arm;
        public bool canLeft;
        public bool canRight;
        public Rectangle _CollisionRight;
        public Rectangle _CollisionLeft;

        public Vector2 Location { get { return _Position; }}

        public User(Texture2D textureL, Texture2D textureR, MoveMethod move, Texture2D bullet)
        {
            _texture = new List<Texture2D>();
            _texture.Add(textureL);
            _texture.Add(textureR);
            _Action = 0;
            _Position = new Vector2(0,500);
            time = 0;
            _MoveMethod = move;
            _spritePos = new Rectangle(0, 90, 100, 200);
            feetCollisionRect = new Rectangle(0, 0, 80, 10);
            isGrounded = false;
            _CollisionRight = new Rectangle(0, 0, 30, 20);
            _CollisionLeft = new Rectangle(30, 0, 30, 20);
        }

        public virtual void Update(GameTime gameTime, KeyboardState stateKey)
        {
            int totalTime = Convert.ToInt32(gameTime.TotalGameTime.TotalMilliseconds);
            if(totalTime > time)
            {
                _MoveMethod.Update(stateKey, canLeft, canRight);
                time += 10;
                Move();
                _CollisionRect.Location = _Position.ToPoint();
            }
            if(_Position.Y > 3000 && _Position.X > 500)
            {
                Reset();
            }
            _CollisionLeft.Location = _Position.ToPoint();
            _CollisionRight.Location = _Position.ToPoint();
            _CollisionRight.X += 30;
            _CollisionRight.Y += 170;
            _CollisionLeft.Y += 170;


        }

        public virtual void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture[_Action], _Position, _spritePos, Color.White);
            
        }

        protected virtual void Move()
        {
            if (!isGrounded)
            {
                _Position.Y += 3;
            }
            _spritePos.X += 100;
            if (_spritePos.X > 700)
            {
                _spritePos.X = 0;
            }
            switch (_MoveMethod.Movedir)
            {
                case (0):

                    _Position.X -= 3; 
                    _Action = 0;
                    break;
                case (1):
                    _Position.X += 3; 
                    _Action = 1;
                    break;
                case (3):
                    //arm.Fire();
                    break;

                default:
                    _spritePos.X = 0;
                    break;
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

        public void PlatformUpdate(int platform)
        {
            _Position.Y += platform;
        }

        public bool getBulletCollision(Rectangle item)
        {
            return item.Intersects(arm.bullet.getCollisionRectagle());
        }

        public bool getBulletsCollision(Rectangle target)
        {
            bool isHit = false;
            foreach(Bullet b in arm.bullets)
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
