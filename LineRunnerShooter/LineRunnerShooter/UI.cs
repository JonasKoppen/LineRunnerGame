using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineRunnerShooter
{
    class UI
    {
        TimeSpan playTime;
        TimeSpan levelTime;
        bool isUpdating;
        int HeroLives;
        int points;

        public void startTimer(GameTime gameTime)
        {
            levelTime = gameTime.TotalGameTime.Duration();
            isUpdating = true;
        }

        public void showTime(SpriteBatch spriteBatch, Vector2 camPos)
        {
            spriteBatch.DrawString(General.font, (playTime.ToString(@"hh\:mm\:ss")), new Vector2(500, 50) + camPos, Color.LightPink);
            spriteBatch.DrawString(General.font, ("Live Points: " + HeroLives.ToString() + "/Points: " + points.ToString()), (camPos + new Vector2(100, 100)), Color.NavajoWhite);
        }

        public void stopTimer(GameTime gameTime)
        {
            levelTime.Subtract(gameTime.TotalGameTime);
            playTime.Add(levelTime);
            isUpdating = false;
        }

        public UI()
        {
            playTime = new TimeSpan(0);
            levelTime = new TimeSpan(0);
            isUpdating = false;
        }


        public void Update(GameTime gameTime, Hiro2 hero, int points)
        {
            if (isUpdating)
            {
                playTime = playTime.Add(levelTime.Subtract(gameTime.TotalGameTime)); 
            }
            HeroLives = hero.Lives;
            this.points = points;
        }
    }
}
