using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineRunnerShooter
{
    class Lift : Block
    {
        bool goingUp;
        Vector2 eindPos;
        public bool isActive;
        int slow;
        public Lift(Texture2D texture, Vector2 startPos, Vector2 eindPos) : base(texture, startPos)
        {
            goingUp = true;
            this.eindPos = eindPos;
            collisionRect = new Rectangle(0, 0, 200, 50);
            _texturePos.Width = 200;
            slow = 8;
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
            
            Positie.X += 100;
            spriteBatch.Draw(_texture, Positie, Color.Yellow);
            Positie.X -= 100;
            
        }
    }

}
