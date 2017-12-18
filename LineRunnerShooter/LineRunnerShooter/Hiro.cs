﻿using System;
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

        /*
    class Hiro : User
    {
        protected bool jumpAllowed;
        protected int _JumpHeight;
        private double invincebleTime;
        ShotARM SingleShotArm;
        ShotARM TripleShotArm;


        public Hiro(Texture2D textureL, Texture2D textureR, MoveMethod move, Texture2D armtexture, Texture2D bullet, int posX, int posY) : base(textureL, textureR, move, bullet)
        {
            _spritePos = new Rectangle(0, 0, 100, 195);
            _Position.X = posX;
            _Position.Y = posY;
            SingleShotArm = new ShotARM(armtexture, bullet,1);
            TripleShotArm = new ShotARM(armtexture, bullet,3);
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
    */
    class Hiro2 : User
    {
        protected bool jumpAllowed;
        protected int _JumpHeight;
        private double invincebleTime;
        List<ARMBluePrint> arsenal;
        int selectedARM = 1;


        public Hiro2(int texture, MoveMethod move, Texture2D armtexture, Texture2D bullet, int posX, int posY) : base(texture,  move, bullet)
        {
  
            _spritePos = new Rectangle(0, 0, 100, 200);
            _Position.X = posX;
            _Position.Y = posY;
            arsenal = new List<ARMBluePrint>();
            arsenal.Add(new ShotARM(armtexture, bullet,1));
            arsenal.Add( new ShotARM(armtexture, bullet,3));
            arsenal.Add(new FlameThrower(armtexture, bullet));
            _JumpHeight = 15;
            invincebleTime = 0;
            collisionBox = new CollisionBox(Convert.ToInt16(_Position.X), Convert.ToInt16(_Position.Y), _spritePos.Width, _spritePos.Height);
        }

        public void Update(GameTime gameTime, KeyboardState stateKey, MouseState mouse, Vector2 camPos, Vector2 mouseLoc, List<BulletBlueprint> bullets)
        {
            CheckAction();
            _MoveMethod.Update(stateKey, mouse, canLeft, canRight);
            base.Update(gameTime, stateKey, mouse, camPos, bullets);
            arsenal[selectedARM].Update(gameTime, _Position, mouseLoc);
            if (isGrounded)
            {
                jumpAllowed = true;
            }
            invincebleTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
            selectedARM = (_MoveMethod as MovePlayer).selWeapon;
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
                arsenal[selectedARM].Fire();
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
                spriteBatch.Draw(_texture, _Position, _spritePos, Color.White);
                arsenal[selectedARM].Draw(spriteBatch);
                if (invincebleTime > 20)
                {
                    spriteBatch.Draw(_texture, _Position, _spritePos, Color.Red);
                }
            }
            else
            {
                arsenal[selectedARM].Draw(spriteBatch);
                spriteBatch.Draw(_texture, _Position, _spritePos, Color.White);
                if (invincebleTime > 20)
                {
                    spriteBatch.Draw(_texture, _Position, _spritePos, Color.Red);
                }
            }
            
            //base.draw(spriteBatch);
        }

        protected override void takeDamage(int damage)
        {
            if(invincebleTime < 1)
            {
                base.takeDamage(damage);
            }
            invincebleTime = 1000;
        }

        public List<BulletBlueprint> getBullets()
        {
            return arsenal[selectedARM].getBullets();
        }
    }

    class MovePlayer : MoveMethod
    {
        private bool lastShootState = false;
        public int selWeapon;
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

            if ((mouseState.LeftButton == ButtonState.Pressed) && !lastShootState) //http://www.gamefromscratch.com/post/2015/06/28/MonoGame-Tutorial-Handling-Keyboard-Mouse-and-GamePad-Input.aspx
            {
                isShooting = true;
                lastShootState = true;
            }

            if ((mouseState.LeftButton == ButtonState.Released) || selWeapon == 2)
            {
                lastShootState = false;
            }

            if (stateKey.IsKeyDown(Keys.Z))
            {
                isJump = true;
            }

            if(stateKey.IsKeyDown(Keys.D1))
            {
                selWeapon = 0;
            }
            else if (stateKey.IsKeyDown(Keys.D2))
            {
                selWeapon = 1;
            }
            else if (stateKey.IsKeyDown(Keys.D3))
            {
                selWeapon = 2;
            }

        }
    }
}
