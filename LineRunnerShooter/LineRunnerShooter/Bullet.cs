﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineRunnerShooter
{
    /*
     * Bullet class, contains different bullet types (not all are realy a bullet but naming is not a priority)
     * A bullet requires a position, collisionRectangle (1 will do),  a state and a TimeToLive (because working with vision boxes is not a good idea, trust me, I tried)
     * should add destroy on blockhit
     */ 
    class Bullet
    {
        public Texture2D _texture;
        public Vector2 Positie;
        public Vector2 _direction; //speed is always constant here so, no velocity
        public bool isFired; //is it active
        private Rectangle collisionRect;
        private double timeToLive; //works better than with
        private int damage;
        public int owner { get; private set; }

        public Bullet(Texture2D texture)
        {
            _texture = texture;
            Positie = new Vector2();
            isFired = false;
            collisionRect = new Rectangle(Positie.ToPoint(), new Point(50, 50));
            timeToLive = 0;
            owner = 1;
        }
        public Bullet(Texture2D texture, Point size)
        {
            _texture = texture;
            Positie = new Vector2();
            isFired = false;
            collisionRect = new Rectangle(Positie.ToPoint(), size);
            timeToLive = 0;
        }

        public void fire(float angle, Vector2 pos)
        {
            if (!isFired)
            {
                Positie = pos;
                _direction.X = (float)(Math.Cos(angle));
                _direction.Y = -(float)(Math.Sin(angle));
                isFired = true;
                timeToLive = 1000;
            }
            
        }
        public void fire(Vector2 pos)
        {
            if (!isFired)
            {
                Positie = pos;
                _direction.X = 0;
                _direction.Y = 10;
                isFired = true;
                timeToLive = 1000;
            }

        }

        public void Update(GameTime gameTime)
        {
            timeToLive -= gameTime.ElapsedGameTime.TotalMilliseconds;
            if (isFired)
            {
                Positie.X += Convert.ToInt32(gameTime.ElapsedGameTime.TotalMilliseconds * _direction.X);
                Positie.Y += Convert.ToInt32(gameTime.ElapsedGameTime.TotalMilliseconds * _direction.Y);
            }
            if (timeToLive <0)
            {
                hitTarget();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isFired)
            {
                spriteBatch.Draw(_texture, Positie, Color.White);
            }
            
        }

        public Rectangle getCollisionRectagle()
        {
            collisionRect.Location = Positie.ToPoint();
            return collisionRect;
        }

        public int hitTarget() //aka the reset function, returns the damage
        {
            isFired = false;
            Positie = new Vector2(100,5000);
            return damage;
        }
    }
}
