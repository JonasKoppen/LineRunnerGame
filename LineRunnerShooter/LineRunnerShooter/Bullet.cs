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

    interface IBullet
    {
        int OwnerId { get; }
        Rectangle CollisionRect { get; }
        int hitTarget();
        int hitTarget(Rectangle target);


    }

    abstract class BulletBlueprint : IBullet
    {
        private Texture2D _texture;
        protected Vector2 Positie;
        protected Vector2 _size;

        public bool isFired { get; set; }

        protected int _damage;

        public Rectangle CollisionRect { get {return new Rectangle(Positie.ToPoint(), _size.ToPoint()); } }

        public int OwnerId { get; private set; }

        public BulletBlueprint(Texture2D texture, Vector2 pos, Vector2 size, int damage, int owner)
        {
            _texture = texture;
            Positie = pos;
            _size = size;
            isFired = false;
            _damage = damage;
            OwnerId = owner;
        }

        public virtual int hitTarget()
        {
            return _damage;
        }
        public virtual int hitTarget(Rectangle item)
        {
            int dam = 0;
            if (item.Intersects(CollisionRect))
            {
                dam = _damage;
            }
            return dam;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (isFired)
            {
                spriteBatch.Draw(_texture, Positie, Color.White);
            }
        }

    }
    class Bullet : BulletBlueprint
    {
        public Vector2 _direction; //speed is always constant here so, no velocity
        private double timeToLive; //works better than with

        public Bullet(Texture2D texture) : base(texture, new Vector2(0,0), new Vector2(50,50), 1, 1)
        {
            timeToLive = 0;
        }
        public Bullet(Texture2D texture, Point size) : base(texture, new Vector2(0, 0), size.ToVector2(), 1, 1)
        {
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

        public override int hitTarget() //aka the reset function, returns the damage
        {
            isFired = false;
            return base.hitTarget();
        }
    }

    class Melee : BulletBlueprint
    {
        public Melee(Texture2D texture, Vector2 pos, Vector2 size, int damage, int owner) : base(texture, pos, size, damage, owner)
        {
        }
    }
}
