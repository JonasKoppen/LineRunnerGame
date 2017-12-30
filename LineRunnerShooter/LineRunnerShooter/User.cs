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
    /*
     * The user class is the blueprint for the player and the enemy's
     * does gravity and basic movement, 
     * 
     */ 
    class Character
    {

        protected int time; //for update 

        protected Texture2D _texture; //textures
        protected Rectangle _spritePos;     //texture pos
        protected int _action;              //actioin movement
        public Vector2 _position;        //position
        protected Vector2 _velocity;    //for change of position
        protected Vector2 _startPos;    //start position
        protected MoveMethod _moveMethod;   //movemethod 
        protected const double gravity = 9.81;       //gravity
        protected int _lives;               //live points
        protected double slow;              //slows down user
        protected double maxSpeed;          //maximum speed the user can have

        public bool canLeft;                //true = mag naar links
        public bool canRight;               //true = mag naar rechts
        public bool isGrounded;             //true = ik sta op de grond

        protected CollisionBox collisionBox;


        public int Lives { get { return _lives; } }

        public Vector2 Location { get { return _position; } }

        public Character(int texture, MoveMethod move, Texture2D bullet)
        {
            _texture = General._afbeeldingEnemys[texture];
            _action = 0;
            _position = new Vector2(0, 500);
            time = 0;
            _moveMethod = move;
            _spritePos = new Rectangle(100, 190, 100, 200);
            isGrounded = false;
            canLeft = true;
            canRight = true;
            _lives = 20;
            _startPos = new Vector2(500, 1200);
            slow = 30;
            collisionBox = new CollisionBox(Convert.ToInt16(_position.X), Convert.ToInt16(_position.Y), _spritePos.Width, _spritePos.Height);
            _velocity = new Vector2(0,0);
            maxSpeed = 15;
        }

        public virtual void Update(GameTime gameTime, KeyboardState stateKey, List<BulletBlueprint> bullets)
        {
            double totalTime = gameTime.ElapsedGameTime.TotalMilliseconds;
            time += Convert.ToInt32(totalTime);
            if (time > 20)
            {
                time = 0;
                SpritePosUpdate();
            }

            UpdateFI(totalTime);
            if (_position.Y > 2500 && (_position.X >300))
            {
                Reset();
            }
            collisionBox.Update(_position.ToPoint());
            CheckHit(bullets);
        }

        protected virtual void SpritePosUpdate()
        {
            _spritePos.X += 100;
            if (_spritePos.X > 700)
            {
                _spritePos.X = 0;
            }
        }

        public virtual void Update(GameTime gameTime, KeyboardState stateKey, MouseState mouseState, Vector2 camPos, List<BulletBlueprint> bullets)
        {
            Vector2 mousePos = mouseState.Position.ToVector2();
            collisionBox.Update(_position.ToPoint());
            Update(gameTime, stateKey, bullets);
        }

        public virtual void UpdateFI(double dt)
        {
            MoveHorizontal(dt);
            MoveVertical(dt);
            _position += _velocity;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, _spritePos, Color.White);
            //spriteBatch.Draw(_texture[0], collisionBox.Left, _spritePos, Color.Red);
            //spriteBatch.Draw(_texture[0], collisionBox.Right, _spritePos, Color.Red);
            //spriteBatch.Draw(_texture[0], collisionBox.Feet, _spritePos, Color.Blue);
        }

        protected virtual void MoveHorizontal(double time)
        {
            double speed = Math.Abs(_velocity.X) + time/slow;
            if(speed > maxSpeed) { speed = maxSpeed; }
            switch (_moveMethod.Movedir)
            {
                case (0):
                    _velocity.X = -(float)(speed);
                    _action = 0;
                    _spritePos.Y = _spritePos.Size.Y;
                    break;
                case (1):
                    _velocity.X = (float)(speed);
                    _action = 1;
                    _spritePos.Y = 0;
                    break;
                default:
                    if (Math.Abs(_velocity.X) < 1)
                    {
                        _velocity.X = 0;
                    }
                    else
                    {
                        _velocity.X /= 1.2F;
                    }
                    break;
            }
            if (!isGrounded)
            {
                _velocity.X = _velocity.X * 0.95f;
            }
        }

        public virtual void MoveVertical()
        {
            if (!isGrounded)
            {
                _velocity.Y += 1;
            }
        }

        public virtual void MoveVertical(double time)
        {
            if (!isGrounded)
            {
                _velocity.Y += (float)((time / 400.0)*gravity); 
            }
            else { _velocity.Y = 0; }
            if (_velocity.Y > 20) { _velocity.Y = 20; }
        }

        public Rectangle GetCollisionRectagle()
        {
            return collisionBox.Body;
        }

        public virtual Rectangle GetFeetCollisionRect()
        {
            return collisionBox.UnderFeet;
        }

        public Rectangle GetRightCollision()
        {
            return collisionBox.Right;
        }

        public Rectangle GetLeftCollision()
        {
            return collisionBox.Left;
        }

        public void PlatformUpdate(Vector2 platformVelocity)
        {
            _position += platformVelocity; 
        }

        public virtual void Reset()
        {
            _position = _startPos;
            _lives--;
        }

        public virtual void CheckEnviroments(List<Rectangle> level)
        {
            isGrounded = false;
            canLeft = true;
            canRight = true;
            bool hitHead = false;

            foreach(Rectangle rect in level)
            {
                if(rect.Intersects(collisionBox.UnderFeet))
                {
                    isGrounded = true;
                }
                if (rect.Intersects(collisionBox.Feet) && isGrounded) //TODO: Lift glitcht weer
                {
                    _position.Y -= 1;
                    _velocity.Y = 0;
                }
                if (rect.Intersects(GetLeftCollision())&&canLeft)
                {
                    canLeft = false;
                    _position.X += Math.Abs(_velocity.X);
                }
                if (rect.Intersects(GetRightCollision())&&canRight)
                {
                    canRight = false;
                    _position.X -= Math.Abs(_velocity.X);
                }
                if(rect.Intersects(collisionBox.Head) && !hitHead)
                {
                    hitHead = true;
                    _velocity.Y = Math.Abs(_velocity.Y);
                }
            }
        }

        public virtual void CheckHit(List<BulletBlueprint> bullets)
        {
            if(bullets != null)
            {
                int i = 0;
                while (i < bullets.Count) //Only one bullet per update can hit the object //TODO: think about this
                {
                    if (collisionBox.Body.Intersects(bullets[i].CollisionRect))
                    {
                        TakeDamage(bullets[i].HitTarget());
                        
                        if(bullets[i] is SheepBeam && _lives < 3)
                        {
                            Sheep();
                        }
                        i = bullets.Count + 1;
                    }
                    i++;
                }
            }
        }

        protected virtual void Sheep() //for sheepinating the enemy (player can too)
        {
            _texture = General._afbeeldingEnemys[11];
            _spritePos = new Rectangle(0, 0, 100, 200);
            _lives = 100000;
        }

        protected virtual void TakeDamage(int damage)
        {
            _lives -= damage;
        }
    }

    public abstract class MoveMethod
    {
        protected int movedir;
        public bool isJump;
        public bool isShooting;

        public int Movedir { get { return movedir; } protected set { movedir = value; } }

        public abstract void Update(KeyboardState keyState, MouseState mouseState, bool canLeft, bool canRight);
    }
}
