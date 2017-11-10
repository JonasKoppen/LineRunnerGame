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
    class Hiro2 : User2
    {
        protected bool jumpAllowed;
        protected int _JumpHeight;


        public Hiro2(Texture2D textureL, Texture2D textureR, MoveMethod move, Texture2D armtexture, Texture2D bullet, int posX, int posY) : base(textureL, textureR, move, bullet)
        {
            _spritePos = new Rectangle(0, 0, 100, 200);
            _Position.X = posX;
            _Position.Y = posY;
            arm = new ARM(armtexture, bullet);
            _JumpHeight = 0;
            _CollisionRect = new Rectangle(100, 0, 100, 200);
            speedX = 7;
        }

        public void Update(GameTime gameTime, KeyboardState stateKey, MouseState mouseState, Vector2 camPos)
        {
            Vector2 mousePos = mouseState.Position.ToVector2();
            base.Update(gameTime, stateKey);
            arm.Update(gameTime, _Position, mousePos, camPos);
            if (isGrounded)
            {
                jumpAllowed = true;
            }
            CheckAction();

        }

        protected override void MoveHorizontal()
        {
            base.MoveHorizontal();
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
            //spriteBatch.Draw(_texture[0], this.getFeetCollisionRect(), Color.Red);
            arm.Draw(spriteBatch);
        }
    }

    class MovePlayer2 : MoveMethod
    {
        public override void Update(KeyboardState stateKey, bool moveLeft, bool moveRight)
        {
            isJump = false;
            isShooting = false;
            if (stateKey.IsKeyDown(Keys.Left))
            {
                movedir = 0;
            }
            else if (stateKey.IsKeyDown(Keys.Right))
            {
                movedir = 1;
            }
            else
            {
                movedir = 2;
            }
            if (stateKey.IsKeyDown(Keys.Space))
            {
                isShooting = true;
            }
            if (stateKey.IsKeyDown(Keys.Up))
            {
                isJump = true;
            }

        }
    }
}
