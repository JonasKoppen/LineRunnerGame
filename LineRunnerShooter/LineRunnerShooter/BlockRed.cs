using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineRunnerShooter
{
    /*
     * The red platform cycles between active and non-active mode, active means you can stand on it, non-active means you fall through it
     * beautifull animation made by me :)
     * Also works with counters wich tick every x milliseconds
     * 
     */
    class BlockRed : Block, IUpdatetableBlock, ICollidableBlocks
    {
        private bool isActive;
        protected double time;
        int state;
        bool isRotating;
        int redTime;
        int greenTime;
        public BlockRed(Vector2 pos) : base(1, pos)
        {
            isActive = true;
            time = 3;
            state = 2;
            isRotating = false;
            _texturePos = new Rectangle(100, 0, 100, 100);
            redTime = 2000;
            greenTime = 4000;
        }

        public BlockRed(Vector2 pos, int red, int green) : base(1, pos)
        {
            isActive = true;
            time = 3;
            state = 2;
            isRotating = true;
            _texturePos = new Rectangle(100, 0, 100, 100);
            redTime = red;
            greenTime = green;
        }

        public override Rectangle GetCollisionRectagle()
        {
            Rectangle rectangle;
            if (isActive)
            {
                rectangle = base.GetCollisionRectagle();
            }
            else
            {
                rectangle = new Rectangle();
            }
            rectangle.Y += 20;
            return rectangle;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
                spriteBatch.Draw(General._afbeeldingBlokken[_textureNum], Positie,_texturePos, Color.White);    
        }

        public void Update(GameTime gameTime) //TODO: check of dit beter kan, ziet er rommelig uit
        {
            time -= gameTime.ElapsedGameTime.TotalMilliseconds;
            if(time <0)
            {
                if (isActive)
                {
                    time = redTime;
                }
                else
                {
                    time = greenTime;
                }
                isRotating = true; //trigger voor rotatie animatie
            }
            if (isRotating)
            {
                _texturePos.X = state * 100;
                state++;
                if(state >= 20 && _texturePos.Y == 0)
                {
                    isRotating = false;
                    state = 1;
                    _texturePos.Y = 100;
                    isActive = false;
                }
                if(state >= 17 && _texturePos.Y == 100)
                {
                    isRotating = false;
                    state = 3;
                    _texturePos.Y = 0;
                    isActive = true;
                }
                if(state > 8)
                {
                    isActive = false;
                }
                _texturePos.X = state * 100;
            }
    }
    }
}
