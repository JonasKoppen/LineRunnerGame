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
        Vector2 location;

        public void StartTimer(GameTime gameTime)
        {
            levelTime = gameTime.TotalGameTime.Duration();
            isUpdating = true;
        }

        public void ShowTime(SpriteBatch spriteBatch, Vector2 camPos)
        {
            spriteBatch.DrawString(General.font, (playTime.ToString(@"mm\:ss\.ff")), new Vector2(2300, 50) + camPos, Color.DarkBlue);
            spriteBatch.DrawString(General.font, ("Live Points: " + HeroLives.ToString() + "/20 ||  Points: " + points.ToString()), (camPos + new Vector2(850, 50)), Color.NavajoWhite); //Punten worden niet getoond
            spriteBatch.Draw(General._afbeeldingBlokken[10], new Rectangle(camPos.ToPoint() - new Point(200,0), new Point(3000,1500)), Color.White);
        }

        public void StopTimer(GameTime gameTime)
        {
            levelTime.Subtract(gameTime.TotalGameTime);
            playTime.Add(levelTime);
            isUpdating = false;
        }

        public void ShowResult(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(General.font, (playTime.ToString(@"mm\:ss\.ff")), new Vector2(870, 270), Color.White);
            spriteBatch.DrawString(General.font, ( points.ToString()), new Vector2(1000, 320), Color.White);
            if((points > 90) && (playTime.CompareTo(new TimeSpan(0,3,0)) < 0))
            {
                spriteBatch.Draw(General._afbeeldingBlokken[14], new Rectangle(800, 420, 300, 120), new Rectangle(0, 0, 150, 50), Color.White);
            }
            else if (points > 50)
            {
                spriteBatch.Draw(General._afbeeldingBlokken[14], new Rectangle(800, 420, 300, 120), new Rectangle(0, 50, 150, 50), Color.White);
            }
            else
            {
                spriteBatch.Draw(General._afbeeldingBlokken[14], new Rectangle(800, 420, 300, 120), new Rectangle(0, 100, 150, 50), Color.White);
            }


        }

        public void ShowDeath(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(General._afbeeldingBlokken[13], new Rectangle(Convert.ToInt16(location.X), Convert.ToInt16(location.Y), 2000,250), Color.OrangeRed); //Punten worden niet getoond
            spriteBatch.DrawString(General.fontBig, ("PRESS ENTER TO CONTINUE"), new Vector2(location.X+ 900 , location.Y +400), Color.Black);
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

        public void UpdateDeath(Vector2 campos, GameTime gameTime)
        {
            location.X += ( campos.X  - location.X) / 8; 
        }

        public void GameOver(Vector2 campos)
        {
            location = campos + new Vector2(-3000, 500);
        }
    }
}
