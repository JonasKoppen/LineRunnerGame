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
     * The User Interface wit timer, live points and total earned points
     * Keeps track of the time
     */ 
    class UI
    {
        TimeSpan playTime;
        TimeSpan levelTime;
        bool isUpdating;
        int HeroLives;
        int points;
        Vector2 location;

       
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
            location.X += ( campos.X  - location.X) / 16;
            location.Y = campos.Y;
        }

        public void GameOver(Vector2 campos)
        {
            location = campos + new Vector2(-3000, 500);
        }

        public void StartTimer(GameTime gameTime)
        {
            levelTime = gameTime.TotalGameTime.Duration();
            isUpdating = true;
        }

        public void ShowTime(SpriteBatch spriteBatch, Vector2 camPos)
        {
            spriteBatch.DrawString(General.font, (playTime.ToString(@"mm\:ss\.ff")), new Vector2(2300, 50) + camPos, Color.DarkBlue);
            spriteBatch.DrawString(General.font, ("Live Points: " + HeroLives.ToString() + "/20 ||  Points: " + points.ToString()), (camPos + new Vector2(850, 50)), Color.NavajoWhite); //Punten worden niet getoond
            spriteBatch.Draw(General._afbeeldingBlokken[10], new Rectangle(camPos.ToPoint() - new Point(200, 0), new Point(3000, 1500)), Color.White);
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
            spriteBatch.DrawString(General.font, (points.ToString()), new Vector2(950, 310), Color.White);
            if ((points >= 120) && (playTime.CompareTo(new TimeSpan(0, 3, 0)) < 0)) //player has to be faster than 3 minutes and the score has to be more or equal to 120 points
            {
                spriteBatch.Draw(General._afbeeldingBlokken[14], new Rectangle(800, 420, 300, 120), new Rectangle(0, 0, 150, 50), Color.White);
            }
            else if (points > 100)
            {
                spriteBatch.Draw(General._afbeeldingBlokken[14], new Rectangle(800, 420, 300, 120), new Rectangle(0, 50, 150, 50), Color.White);
            }
            else
            {
                spriteBatch.Draw(General._afbeeldingBlokken[14], new Rectangle(800, 420, 300, 120), new Rectangle(0, 100, 150, 50), Color.White);
            }
        }

        public void ShowResultAnimated(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(General._afbeeldingBlokken[15], new Rectangle(location.ToPoint() - new Point(200, 0), new Point(2900, 1450)), Color.White);
            spriteBatch.DrawString(General.font, (playTime.ToString(@"mm\:ss\.ff")), new Vector2(1600 + location.X, 550 + location.Y), Color.White);
            spriteBatch.DrawString(General.font, (points.ToString()), new Vector2(1600 + location.X, 620 + location.Y), Color.White);
            if ((points >= 120) && (playTime.CompareTo(new TimeSpan(0, 3, 0)) < 0)) //player has to be faster than 3 minutes and the score has to be more or equal to 120 points
            {
                spriteBatch.Draw(General._afbeeldingBlokken[14], new Rectangle(Convert.ToInt16(1600 + location.X), Convert.ToInt16(900 + location.Y), 300*2, 120 * 2), new Rectangle(0, 0, 150, 50), Color.White);
            }
            else if (points > 100)
            {
                spriteBatch.Draw(General._afbeeldingBlokken[14], new Rectangle(Convert.ToInt16(1600 + location.X), Convert.ToInt16(900 + location.Y), 300 * 2, 120 * 2), new Rectangle(0, 50, 150, 50), Color.White);
            }
            else
            {
                spriteBatch.Draw(General._afbeeldingBlokken[14], new Rectangle(Convert.ToInt16(1600 + location.X), Convert.ToInt16(900 + location.Y), 300 * 2, 120 * 2), new Rectangle(0, 100, 150, 50), Color.White);
            }
        }

        public void ShowDemoEnd(SpriteBatch spriteBatch, Hiro2 hiro)
        {
            if ((points >= 120) && (playTime.CompareTo(new TimeSpan(0, 3, 0)) < 0))
            {
                spriteBatch.DrawString(General.fontBig, "Thanks for playing the Game", new Vector2(100, 200), Color.Yellow);
                spriteBatch.DrawString(General.fontBig, "Everything is unlocked!", new Vector2(110, 280), Color.Yellow);
                hiro.MaxArms = 4;
            }
            else if (points > 100)
            {
                spriteBatch.DrawString(General.fontBig, "Thanks for playing the Game", new Vector2(100, 180), Color.Yellow);
                spriteBatch.DrawString(General.fontBig, "The 3th weapon (flamthrower)!", new Vector2(80, 250), Color.Yellow);
                spriteBatch.DrawString(General.fontBig, "Is Unlocked", new Vector2(100, 320), Color.Yellow);
                if (hiro.MaxArms < 3) hiro.MaxArms = 3;
            }
            else
            {
                spriteBatch.DrawString(General.fontBig, "Thanks for playing the Game!", new Vector2(100, 180), Color.Yellow);
                spriteBatch.DrawString(General.fontBig, "Try Again and score more points!", new Vector2(0, 250), Color.Yellow);
                spriteBatch.DrawString(General.fontBig, "To unlock more weapons!", new Vector2(100, 320), Color.Yellow);
            }
        }

        public void ShowDeath(SpriteBatch spriteBatch, Vector2 campos)
        {
            spriteBatch.Draw(General._afbeeldingBlokken[13], new Rectangle(Convert.ToInt16(location.X), Convert.ToInt16(campos.Y +500), 2000, 250), Color.OrangeRed); //Punten worden niet getoond
            spriteBatch.DrawString(General.fontBig, ("PRESS ENTER TO CONTINUE"), new Vector2(location.X + 900, campos.Y + 800), Color.DarkRed);
        }

    }
}
