using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LineRunnerShooter
{
    class BlockPurple : Block
    {
        int upTime;
        int downTime;
        int time;
        bool isStable;
        bool isTouched;
        float stablePosY;
        double lastTime;

        public BlockPurple(Texture2D texture, Vector2 pos) : base(texture, pos)
        {
            upTime = 3;
            downTime = 0;
            time = 1;
            isStable = true;
            isTouched = false;
            _texturePos = new Rectangle(0, 0, 100, 100);
            stablePosY = pos.Y;
            lastTime = 0;
        }
        

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isStable)
            {
                spriteBatch.Draw(_texture, Positie, new Rectangle(0, 0, 100, 100), Color.White);
            }
            else
            {
                spriteBatch.Draw(_texture, Positie, new Rectangle(100, 0, 100, 100), Color.White);
            }
        }

        public void Update(GameTime gameTime, Rectangle player)
        {
            if (gameTime.TotalGameTime.TotalSeconds > time)
            {
                time += 1;
                if (player.Intersects(this.getCollisionRectagle()))
                {
                    isTouched = true;
                }
                if (isTouched && isStable)
                {
                    upTime--;
                    if (upTime <= 0)
                    {
                        isStable = false;
                        downTime = 5;
                    }
                }
                if (isStable)
                {
                    Positie.Y = stablePosY;
                }
                else
                {
                    downTime--;
                }
            }
            UpdatePosition(gameTime.TotalGameTime.TotalMilliseconds);
        }

        public void UpdatePosition(double time)
        {
            if (!isStable)
            {
                double dt = time - lastTime;
                Positie.Y += Convert.ToInt16(dt/4);
                if (downTime <= 0)
                {
                    isTouched = false;
                    isStable = true;
                    upTime = 3;
                }
            }
            else
            {
                Positie.Y = stablePosY;
            }
            lastTime = time;
        }
    }
}
