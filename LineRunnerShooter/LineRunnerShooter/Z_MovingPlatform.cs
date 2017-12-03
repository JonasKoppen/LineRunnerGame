using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LineRunnerShooter
{
    class Z_MovingPlatform : Block //TODO: moving platforms zijn onhold gezet tot de rest in orde is
    {
        bool goingUp;
        bool isStatic;
        int stateCounter;
        int stateTimer;
        int baseCounter; // aantal cycles tot verandering van richting
        int baseSlow; //Speed of going up and down, groter = slower

        public Z_MovingPlatform(Texture2D texture, Vector2 pos) : base(texture, pos)
        {
            baseCounter = 20;
            baseSlow = 50;
            stateCounter = baseCounter;
            stateTimer = 500;
            goingUp = false;
        }
        public Z_MovingPlatform(Texture2D texture, Vector2 pos, int counter, int slow) : base(texture, pos)
        {
            baseCounter = counter;
            slow = slow * 2;
            baseSlow = slow;
            stateCounter = baseCounter;
            stateTimer = slow;
            goingUp = false;
        }

        public int Update(GameTime gameTime, Rectangle player)
        {
            int change = 0;
            if (gameTime.TotalGameTime.TotalMilliseconds > stateTimer)
            {
                stateTimer += baseSlow;
                if (goingUp && (!isStatic))
                {
                    Positie.Y -= 3;
                }
                else if ((!goingUp) && (!isStatic))
                {
                    Positie.Y += 3;
                }
                change = PushUpDown(player);

                stateCounter--;
                if (stateCounter <= 0)
                {
                    if (goingUp && (isStatic))
                    {
                        goingUp = false;
                        isStatic = false;
                        stateCounter = baseCounter;
                    }
                    else if ((!goingUp) && (isStatic))
                    {
                        goingUp = true;
                        isStatic = false;
                        stateCounter = baseCounter;
                    }
                    else if (!isStatic)
                    {
                        isStatic = true;
                        stateCounter = baseCounter;
                    }
                }
            }
            
            return change;
        }
        public int Update(GameTime gameTime, Rectangle player, int extra)
        {
            int change = 0;
            if (gameTime.TotalGameTime.TotalMilliseconds > stateTimer)
            {
                stateTimer += baseSlow;
                if (goingUp)
                {
                    Positie.Y -= 3;
                }
                else if (!goingUp)
                {
                    Positie.Y += 3;
                }
                change = PushUpDown(player);
                stateCounter--;
                if (stateCounter <= 0)
                {
                    goingUp = !goingUp;
                    stateCounter = baseCounter;
                }
            }
            return change;
        }

        public int PushUpDown(Rectangle player)
        {
            int uit = 0;
            if (player.Intersects(this.getCollisionRectagle()))
            {
                if (goingUp && (!isStatic))
                {
                    uit -= 3;
                }
                else if ((!goingUp) && (!isStatic))
                {
                    uit += 0;
                }

            }
            return uit;
        }
    }
    
}
