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
            _JumpHeight = 0;
            _CollisionRect = new Rectangle(100, 0, 100, 200);
            invincebleTime = 0;
        }

        public void Update(GameTime gameTime, KeyboardState stateKey, MouseState mouse, Vector2 camPos)
        {
            base.Update(gameTime, stateKey, mouse, camPos);
            if (isGrounded)
            {
                jumpAllowed = true;
            }
            CheckAction();
            invincebleTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        protected override void MoveHorizontal(Double time)
        {
            base.MoveHorizontal(time);
            if (_JumpHeight > 0)
            {
                _Position.Y -= 30 - (10 - _JumpHeight) * 2;
                _JumpHeight--;
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
            if ((_JumpHeight <= 0 && isGrounded) || jumpAllowed)
            {
                _JumpHeight = 15;
                isGrounded = false;
                gravity = 0;
                jumpAllowed = false;
            }
        }

        public void setStartPos()
        {
            _Position = new Vector2(150, 1700);
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            _spritePos = new Rectangle(0, 0, 100, 200);
            spriteBatch.Draw(_texture[_Action], _Position, _spritePos, Color.White);
            spriteBatch.Draw(_texture[0], this.getLeftCollision(), Color.Red);
            arm.Draw(spriteBatch);
            if(invincebleTime > 20)
            {
                spriteBatch.Draw(_texture[_Action], _Position, _spritePos, Color.Red);
            }
        }

        public void checkHit(Rectangle hitObject)
        {
            if (_CollisionRect.Intersects(hitObject))
            {
                _lives--;
                invincebleTime = 500;
            }
        }
    }

    class MovePlayer : MoveMethod
    {
        public override void Update(KeyboardState stateKey, bool blockLeft, bool blockRight)
        {
            isJump = false;
            isShooting = false;
            if (stateKey.IsKeyDown(Keys.Left) && !blockLeft)
            {
                movedir = 0;
            }
            else if (stateKey.IsKeyDown(Keys.Right) && !blockRight)
            {
                movedir = 1;
            }
            else
            {
                movedir = 2;
            }
            if (stateKey.IsKeyDown(Keys.Space) && !isShooting)
            {
                isShooting = true;
            }
            if (stateKey.IsKeyUp(Keys.Space))
            {
                isShooting = false;
            }
            if (stateKey.IsKeyDown(Keys.Up))
            {
                isJump = true;
            }

        }
    }
}
