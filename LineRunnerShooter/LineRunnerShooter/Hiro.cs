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
     * The hero of the game, probobly most complex class the game has (players are so much work, why can they just not simply move left and right, why do players require the hero of the game to run and shoot his way through?)
     * Is the only thing that can jump, Goes invinceble (onaantastbaar, mss beter even translate erbij pakken) when hit
     * 
     * 
     */ 
    class Hiro : User
    {
        protected bool jumpAllowed;
        protected int _JumpHeight;
        private double invincebleTime;


        public Hiro(Texture2D textureL, Texture2D textureR, MoveMethod move, Texture2D armtexture, Texture2D bullet, int posX, int posY) : base(textureL, textureR, move, bullet)
        {
            _spritePos = new Rectangle(0, 0, 100, 200);
            _Position.X = posX;
            _Position.Y = posY;
            arm = new ARM(armtexture, bullet);
            _JumpHeight = 15;
            invincebleTime = 0;
            collisionBox = new CollisionBox(Convert.ToInt16(_Position.X), Convert.ToInt16(_Position.Y), _spritePos.Width, _spritePos.Height);
        }

        public void Update(GameTime gameTime, KeyboardState stateKey, MouseState mouse, Vector2 camPos, Vector2 mouseLoc)
        {
            CheckAction();
            _MoveMethod.Update(stateKey, mouse, canLeft, canRight);
            base.Update(gameTime, stateKey, mouse, camPos);
            arm.Update(gameTime, _Position, mouseLoc);
            if (isGrounded)
            {
                jumpAllowed = true;
            }
            invincebleTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        private void CheckAction()
        {
            if (_MoveMethod.isShooting)
            {
                arm.Fire();
            }
            if (_MoveMethod.isJump)
            {
                Jump();
            }
        }
        private void Jump()
        {
            if (jumpAllowed)
            {
                isGrounded = false;
                jumpAllowed = false;
                _Velocity.Y = -_JumpHeight;
            }
        }

        public void setStartPos()
        {
            _Position = new Vector2(150, 1700);
            _Velocity = new Vector2(0, 0);
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            _spritePos = new Rectangle(0, 0, 100, 200);
            spriteBatch.Draw(_texture[_Action], _Position, _spritePos, Color.White);
            arm.Draw(spriteBatch);
            if(invincebleTime > 20)
            {
                spriteBatch.Draw(_texture[_Action], _Position, _spritePos, Color.Red);
            }
            //base.draw(spriteBatch);
        }

        public void checkHit(Rectangle hitObject)
        {
            if (collisionBox.Body.Intersects(hitObject))
            {
                _lives--;
                invincebleTime = 500;
            }
        }
    }

    class Hiro2 : User
    {
        protected bool jumpAllowed;
        protected int _JumpHeight;
        private double invincebleTime;


        public Hiro2(Texture2D textureL, Texture2D textureR, MoveMethod move, Texture2D armtexture, Texture2D bullet, int posX, int posY) : base(textureL, textureR, move, bullet)
        {
            _spritePos = new Rectangle(0, 0, 100, 200);
            _Position.X = posX;
            _Position.Y = posY;
            arm = new ARM(armtexture, bullet);
            _JumpHeight = 15;
            invincebleTime = 0;
            collisionBox = new CollisionBox(Convert.ToInt16(_Position.X), Convert.ToInt16(_Position.Y), _spritePos.Width, _spritePos.Height);
        }

        public void Update(GameTime gameTime, KeyboardState stateKey, MouseState mouse, Vector2 camPos, Vector2 mouseLoc)
        {
            CheckAction();
            _MoveMethod.Update(stateKey, mouse, canLeft, canRight);
            base.Update(gameTime, stateKey, mouse, camPos);
            arm.Update(gameTime, _Position, mouseLoc);
            if (isGrounded)
            {
                jumpAllowed = true;
            }
            invincebleTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        protected override void spritePosUpdate()
        {
            _spritePos.X += 100;
            if(_MoveMethod.Movedir == 1)
            {
                _spritePos.Y = 0;
            }
            else if(_MoveMethod.Movedir == 0)
            {
                _spritePos.Y = 200;
            }
            if (_MoveMethod.Movedir == 2 || (!isGrounded))
            {
                if (_spritePos.X < 900 || _spritePos.X >= 1300)
                {
                    _spritePos.X = 900;
                }   
            }
            else
            {
                if(_spritePos.X >= 900)
                {
                    _spritePos.X = 0;
                }
            }

            if (!isGrounded)
            {
                _spritePos.X = 0;
            }
        }

        private void CheckAction()
        {
            if (_MoveMethod.isShooting)
            {
                arm.Fire();
            }
            if (_MoveMethod.isJump)
            {
                Jump();
            }
        }
        private void Jump()
        {
            if (jumpAllowed)
            {
                isGrounded = false;
                jumpAllowed = false;
                _Velocity.Y = -_JumpHeight;
            }
        }

        public void setStartPos()
        {
            _Position = new Vector2(150, 1700);
            _Velocity = new Vector2(0, 0);
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            if(_spritePos.Y == 0)
            {
                spriteBatch.Draw(_texture[0], _Position, _spritePos, Color.White);
                arm.Draw(spriteBatch);
                if (invincebleTime > 20)
                {
                    spriteBatch.Draw(_texture[_Action], _Position, _spritePos, Color.Red);
                }
            }
            else
            {
                arm.Draw(spriteBatch);
                spriteBatch.Draw(_texture[0], _Position, _spritePos, Color.White);
                if (invincebleTime > 20)
                {
                    spriteBatch.Draw(_texture[_Action], _Position, _spritePos, Color.Red);
                }
            }
            
            //base.draw(spriteBatch);
        }

        public void checkHit(Rectangle hitObject)
        {
            if (collisionBox.Body.Intersects(hitObject))
            {
                _lives--;
                invincebleTime = 500;
            }
        }

        protected override void takeDamage(int damage)
        {
            base.takeDamage(damage);
            invincebleTime = 500;
        }

        public override void checkEnviroments(List<Rectangle> level)
        {
            base.checkEnviroments(level);
            foreach(Rectangle rect in level)
            {
                foreach (BulletBlueprint b in arm.getBullets())
                {
                    if (b.CollisionRect.Intersects(rect))
                    {
                        b.hitTarget();
                    }
                }
            }
        }

        public List<Bullet> getBullets()
        {
            return arm.getBullets();
        }
    }

    class MovePlayer : MoveMethod
    {
        private bool lastSpaceState = false;
        public override void Update(KeyboardState stateKey, MouseState mouseState, bool canLeft, bool canRight)
        {

            isJump = false;
            isShooting = false;
            if (stateKey.IsKeyDown(Keys.Q) && canLeft)
            {
                movedir = 0;
            }
            else if (stateKey.IsKeyDown(Keys.D) && canRight)
            {
                movedir = 1;
            }
            else
            {
                movedir = 2;
            }
            if ((mouseState.LeftButton == ButtonState.Pressed) && !lastSpaceState) //http://www.gamefromscratch.com/post/2015/06/28/MonoGame-Tutorial-Handling-Keyboard-Mouse-and-GamePad-Input.aspx
            {
                isShooting = true;
                lastSpaceState = true;
            }
            if (stateKey.IsKeyUp(Keys.Z))
            {
                lastSpaceState = false;
            }
            if (stateKey.IsKeyDown(Keys.Z))
            {
                isJump = true;
            }

        }
    }
}
