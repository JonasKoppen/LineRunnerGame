using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LineRunnerShooter
{
    /*
     * This platforms will fall if you touch them, so be quick
     * the platforms work with a counter wich ticks every time the game has run more than 1000 milliseconds (or an other number, wich can be modified later probably, should add it???
     * 
     */ 
    class BlockPurple : Block
    {
        int upTime;
        int downTime;
        double time;
        bool isStable;
        bool isTouched;
        float stablePosY;
        double lastTime;
        Vector2 Velocity;

        public BlockPurple(Vector2 pos) : base(1, pos)
        {
            upTime = 1;
            downTime = 0;
            time = 0;
            isStable = true;
            isTouched = false;
            _texturePos = new Rectangle(0, 35, 100, 35);
            stablePosY = pos.Y +10;
            lastTime = 0;
        }
        

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(General._afbeeldingBlokken[_textureNum], Positie, _texturePos, Color.White);
        }

        public void Update(GameTime gameTime, Rectangle player)
        {
            time += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (time > 1000)
            {
                time = 0;
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
                    _texturePos.X = 0;
                }
                else
                {
                    _texturePos.X = 100;
                    downTime--;
                }
            }
            if (player.Intersects(this.getCollisionRectagle()))
            {
                isTouched = true;
            }
            UpdatePosition(gameTime.ElapsedGameTime.TotalMilliseconds);
        }

        public void UpdatePosition(double dt)
        {
            if (!isStable)
            {
                Velocity = new Vector2(0,((float)(dt/5)));
                if (downTime <= 0)
                {
                    isTouched = false;
                    isStable = true;
                    upTime = 1;
                }
            }
            else
            {
                Positie.Y = stablePosY;
                Velocity = new Vector2(0, 0);
            }
            Positie += Velocity;
            lastTime = time;
        }

        public void getPosChange(User user)
        {
            if (user.getFeetCollisionRect().Intersects(getCollisionRectagle()))
            {
                user.PlatformUpdate(Velocity);
            }
        }
    }
}
