using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LineRunnerShooter
{
    class Orih : User
    {
        public bool isAlive;
        public bool seePlayer;

        /*
        public Orih(Texture2D textureL, Texture2D textureR, MoveMethod move, Texture2D bullet) : base(textureL, textureR, move, bullet)
        {
            _spritePos = new Rectangle(0, 0, 60, 200);
            _CollisionRect = _spritePos;
            _CollisionRight = new Rectangle(0, 0, 30, 20);
            _CollisionLeft = new Rectangle(30, 0, 30, 20);
            feetCollisionRect = new Rectangle(0, 0, 60, 10);
            _Position.X = 900;
            arm = null;
            isAlive = true;
            
        }
        */
        public Orih(Texture2D textureL, Texture2D textureR, MoveMethod move, Texture2D bullet, int posX) : base(textureL, textureR, move, bullet)
        {
            _spritePos = new Rectangle(posX, 0, 60, 200);
            _CollisionRect = _spritePos;
            _CollisionRight = new Rectangle(0, 0, 30, 20);
            _CollisionLeft = new Rectangle(30, 0, 30, 20);
            _Position.X = posX;
            _StartPos.X = posX;
            arm = null;
            isAlive = true;

        }
        public void Update(GameTime gameTime, KeyboardState stateKey, bool isHit)
        {
            base.Update(gameTime, stateKey);
            if (isHit)
            {
                isAlive = false;
                Console.WriteLine("I am hit");
                _Position = new Vector2(0, 10000);
                
            }
            
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            _spritePos = new Rectangle(0, 0, 60, 200);
            if (isAlive)
            {
                base.draw(spriteBatch);
            }
            
            

        }

        public void SeePlayer(Rectangle player)
        {
            Rectangle _ViewRectangle;
            if(_MoveMethod.Movedir == 1)
            {
                _ViewRectangle = new Rectangle(Convert.ToInt16(_Position.X), Convert.ToInt16(_Position.Y), 300, 200);
            }
            else
            {
                _ViewRectangle = new Rectangle(Convert.ToInt16(_Position.X)-240, Convert.ToInt16(_Position.Y), 300, 200);
            }
            
            if (player.Intersects(_ViewRectangle))
            {
                seePlayer = true;
                Console.WriteLine("I see you");
            }
            else
            {
                seePlayer = false;
            }
        }
    }

    class RobotMove : MoveMethod
    {
        private Random r;

        public RobotMove()
        {
            r = new Random();
            movedir = 0;
        }

        public override void Update(KeyboardState stateKey, bool moveLeft, bool moveRight)
        {
            int move = r.Next(0, 100);
            if (moveLeft && movedir == 0)
            {
                movedir = 1;
            }
            if (moveRight && movedir == 1)
            {
                 movedir = 0;  
            }
        }
    }
}
