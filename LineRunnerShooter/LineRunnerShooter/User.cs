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
        //TODO: healt
        //TODO: vaste schiethoek (met de muis is niet echt ++)
        //TODO: Hiro sprite maken

        protected int time; //for update 

        protected List<Texture2D> _texture; //textures
        protected Rectangle _spritePos;     //texture pos
        protected int _Action;              //actioin movement
        public Vector2 _Position;        //position
        protected Vector2 _StartPos;
        protected MoveMethod _MoveMethod;   //movemethod 
        protected int gravity;
        protected int _lives;
        protected double slow;



        public bool canLeft;                //true = mag naar links
        public bool canRight;               //true = mag naar rechts
        public bool isGrounded;             //true = ik sta op de grond

        /*
        protected Rectangle _CollisionRight;   //for move right
        protected Rectangle _CollisionLeft;    //for move left
        protected Rectangle _CollisionRect; //hit box
        protected Rectangle feetCollisionRect; //bepaald isGrounded gebruik voor collisie met de grond
        */
        protected CollisionBox collisionBox;

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
            _spritePos = new Rectangle(100, 190, 100, 200);
            isGrounded = false;
            canLeft = true;
            canRight = true;
            _lives = 5;
            _StartPos = new Vector2(500, 300);
            slow = 2;
            collisionBox = new CollisionBox(Convert.ToInt16(_Position.X), Convert.ToInt16(_Position.Y), _spritePos.Width, _spritePos.Height);
        }

        public virtual void Update(GameTime gameTime, KeyboardState stateKey)
        {
            double totalTime = gameTime.ElapsedGameTime.TotalMilliseconds;
            time += Convert.ToInt32(totalTime);
            if (time > 20)
            {
                time = 0;
                _spritePos.X += 100;
                if (_spritePos.X > 700)
                {
                    _spritePos.X = 0;
                }
            }

            UpdateFI(totalTime, stateKey);
            if (_Position.Y > 3000 && (_Position.X > 500 || _Position.X <0))
            {
                Reset();
            }
            if (isGrounded)
            {
                gravity = 0;
            }
            collisionBox.Update(_Position.ToPoint());
        }

        public virtual void Update(GameTime gameTime, KeyboardState stateKey, MouseState mouseState, Vector2 camPos)
        {
            Vector2 mousePos = mouseState.Position.ToVector2();
            arm.Update(gameTime, _Position, _MoveMethod.Movedir, camPos);
            Update(gameTime, stateKey);
            collisionBox.Update(_Position.ToPoint());
        }

        public virtual void UpdateFI(double dt, KeyboardState stateKey)
        {
            _MoveMethod.Update(stateKey, canLeft, canRight);
            MoveHorizontal(dt);
            MoveVertical(dt);

        }

        public virtual void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture[_Action], _Position, _spritePos, Color.White);
            //spriteBatch.Draw(_texture[0], collisionBox.Left, _spritePos, Color.Red);
            //spriteBatch.Draw(_texture[0], collisionBox.Right, _spritePos, Color.Red);
            //spriteBatch.Draw(_texture[0], collisionBox.Feet, _spritePos, Color.Blue);
        }

        protected virtual void MoveHorizontal(double time)
        {
            int distance = Convert.ToInt16(time / slow);
            switch (_MoveMethod.Movedir)
            {
                case (0):
                    _Position.X -= distance;
                    _Action = 0;
                    break;
                case (1):
                    _Position.X += distance;
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
            /*
            if(canLeft && canRight)
            {
                _Position.Y -= 5;
            }
            */

        }

        public Rectangle getCollisionRectagle()
        {
            return collisionBox.Body;
        }

        public virtual Rectangle getFeetCollisionRect()
        {
            return collisionBox.Feet;
        }

        public Rectangle getRightCollision()
        {
            return collisionBox.Right;
        }

        public Rectangle getLeftCollision()
        {
            return collisionBox.Left;
        }

        public void PlatformUpdate(int platform)
        {
            _Position.Y += platform*2; //TODO: first official bug, kan niet verwijderd worden zonder dat dit een probleem geeft
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

        public virtual List<Rectangle> getBulletsRect()
        {
            return arm.getBulletsRect();
        }

        public virtual void Reset()
        {
            _Position = _StartPos;
            gravity = -1;
            _lives--;
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
