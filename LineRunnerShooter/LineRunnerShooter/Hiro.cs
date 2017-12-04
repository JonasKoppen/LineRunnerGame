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

    class MovePlayer : MoveMethod
    {
        private bool lastSpaceState = false;
        public override void Update(KeyboardState stateKey, MouseState mouseState, bool canLeft, bool canRight)
        {

            isJump = false;
            isShooting = false;
            if (stateKey.IsKeyDown(Keys.Left) && canLeft)
            {
                movedir = 0;
            }
            else if (stateKey.IsKeyDown(Keys.Right) && canRight)
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
            if (stateKey.IsKeyUp(Keys.Space))
            {
                lastSpaceState = false;
            }
            if (stateKey.IsKeyDown(Keys.Up))
            {
                isJump = true;
            }

        }
    }
}
