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
            spriteBatch.DrawString(General.font, (playTime.ToString(@"mm\:ss\.ff")), new Vector2(2300, 50) + camPos, Color.DarkBlue);
            spriteBatch.DrawString(General.font, ("Live Points: " + HeroLives.ToString() + "/Points: " + points.ToString()), (camPos + new Vector2(900, 50)), Color.NavajoWhite); //Punten worden niet getoond
            spriteBatch.Draw(General._afbeeldingBlokken[10], new Rectangle(camPos.ToPoint() - new Point(200,0), new Point(3000,1500)), Color.White);
        }

        public void stopTimer(GameTime gameTime)
        {
            levelTime.Subtract(gameTime.TotalGameTime);
            playTime.Add(levelTime);
            isUpdating = false;
        }

        public void showResult(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(General.font, (playTime.ToString(@"mm\:ss\.ff")), new Vector2(870, 270), Color.White);
            spriteBatch.DrawString(General.font, ( HeroLives.ToString()), new Vector2(1000, 320), Color.White); //Punten worden niet getoond
            spriteBatch.DrawString(General.font, (points.ToString()), new Vector2(1000, 370), Color.White); //Punten worden niet getoond
            spriteBatch.DrawString(General.font, ("BAD"), new Vector2(970, 450), Color.Red); //Punten worden niet getoond
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
                playTime = levelTime.Subtract(gameTime.TotalGameTime); 
            }
            HeroLives = hero.Lives;
            this.points = points;
        }
    }
}
