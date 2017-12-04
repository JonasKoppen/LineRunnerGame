using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineRunnerShooter
{
    class Lift : Block //TODO: add reset function zodat ik die liften niet altijd opnieuw moet aanmaken, liften kunnen nu gereset worden vooral handig met de tutorial
    {
        bool goingUp;
        Vector2 eindPos;
        public bool isActive;
        int slow;
        double time;
        public Lift(Texture2D texture, Vector2 startPos, Vector2 eindPos) : base(texture, startPos)
        {
            goingUp = true;
            this.eindPos = eindPos;
            collisionRect = new Rectangle(0, 0, 200, 100);
            _texturePos.Width = 200;
            slow = 3;
            time = 0;
        }

        public int Update(GameTime gameTime, Rectangle player)
        {
            int change = 0;
            if (isActive)
            {
                if (goingUp)
                {                 
                    change = -Convert.ToInt16(gameTime.ElapsedGameTime.TotalMilliseconds/slow);
                    Positie.Y += change;
                }
                if ((!player.Intersects(getCollisionRectagle())) || !goingUp )
                {
                    change = 0;
                }
            }
            if(Positie.Y <= eindPos.Y)
            {
                isActive = false;
            }
            return change;
        }

        public void Update(GameTime gameTime, User player)
        {
            int change = 0;
            time += gameTime.ElapsedGameTime.TotalMilliseconds;
            if(time > 150)
            {
                time = 0;
                _texturePos.X += 200;
                if(_texturePos.X >= 800)
                {
                    _texturePos.X = 0;
                }
            }
            if (isActive)
            {
                if (goingUp)
                {
                    change = -Convert.ToInt16(gameTime.ElapsedGameTime.TotalMilliseconds / slow);
                    Positie.Y += change;
                }
                if (player.getFeetCollisionRect().Intersects(getCollisionRectagle()))
                {
                    player.PlatformUpdate(change);
                    player.isGrounded = true;
                }
            }
            if (Positie.Y <= eindPos.Y)
            {
                isActive = false;
            }
        }

        public void setPos(Vector2 loc)
        {
            Positie = loc;
        }

        public void setDest(Vector2 dest)
        {
            eindPos = dest;
        }

        public void activate(Rectangle player)
        {
            if (!isActive)
            {
                isActive = player.Intersects(getCollisionRectagle());
            }
        }
        public void activate()
        {
            if (!isActive)
            {
                isActive = true;
            }
        }
        /*
        public int PushUpDown(Rectangle player)
        {
            int uit = 0;
            if (player.Intersects(getCollisionRectagle()))
            {
                if (goingUp)
                {
                    uit -= 3;
                }
                else if (!goingUp)
                {
                    uit += 0;
                }

            }
            return uit;
        }
        */

        public override void Draw(SpriteBatch spriteBatch) //TODO: dit moet in  1 draw gebeuren
        {
            base.Draw(spriteBatch);
            
            
            
        }
    }

}
