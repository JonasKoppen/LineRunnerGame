﻿using System;
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
        double time;
        bool isStable;
        bool isTouched;
        float stablePosY;
        double lastTime;

        public BlockPurple(Texture2D texture, Vector2 pos) : base(texture, pos)
        {
            upTime = 1;
            downTime = 0;
            time = 0;
            isStable = true;
            isTouched = false;
            _texturePos = new Rectangle(0, 35, 100, 35);
            stablePosY = pos.Y +10;
            lastTime = 0;
            collisionRect.Height = 60;
        }
        

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isStable)
            {
                spriteBatch.Draw(_texture, Positie, _texturePos, Color.White);
            }
            else
            {
                spriteBatch.Draw(_texture, Positie, _texturePos, Color.White);
            }
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
                    _texturePos.X = 0;
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
                Positie.Y += Convert.ToInt16(dt/4);
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
            }
            lastTime = time;
        }
    }
}
