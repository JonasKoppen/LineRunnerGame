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

    class Hiro2 : User
    {
        protected bool jumpAllowed;
        protected int _JumpHeight;
        private double invincebleTime;
        List<ARMBluePrint> arsenal;
        int selectedARM = 0;
        int maxArms = 5;

        public int MaxArms { get { return maxArms; } set { if (value > maxArms) { maxArms = value; }} }

        public Hiro2(int texture, MoveMethod move, Texture2D armtexture, Texture2D bullet, Vector2 location) : base(texture,  move, bullet)
        {
  
            _spritePos = new Rectangle(0, 0, 100, 200);
            _Position = location;
            arsenal = new List<ARMBluePrint>();
            arsenal.Add(new ShotARM(armtexture, bullet,1));
            arsenal.Add( new ShotARM(armtexture, bullet,3));
            arsenal.Add(new FlameThrower(armtexture, bullet));
            arsenal.Add(new SheepANator(armtexture, bullet));
            _JumpHeight = 15;
            invincebleTime = 0;
            collisionBox = new CollisionBox(Convert.ToInt16(_Position.X), Convert.ToInt16(_Position.Y), _spritePos.Width, _spritePos.Height);
        }

        public void Update(GameTime gameTime, KeyboardState stateKey, MouseState mouse, Vector2 camPos, Vector2 mouseLoc, List<BulletBlueprint> bullets)
        {
            CheckAction();
            (_MoveMethod as MovePlayer).maxWeapon = maxArms;
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

        public void setToStartPos(Vector2 location)
        {
            _Position = location;
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
        public int maxWeapon = 2;
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

            if ((mouseState.LeftButton == ButtonState.Released) || selWeapon > 1)
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
            else if (stateKey.IsKeyDown(Keys.D3) && maxWeapon > 2)
            {
                selWeapon = 2;
            }
            else if (stateKey.IsKeyDown(Keys.D4) && maxWeapon > 3)
            {
                selWeapon = 3;
            }

        }
    }
}
