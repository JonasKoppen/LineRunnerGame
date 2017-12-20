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
     * Bullet class, contains different bullet types (not all are realy a bullet but naming is not a priority)
     * A bullet requires a position, collisionRectangle (1 will do),  a state and a TimeToLive (because working with vision boxes is not a good idea, trust me, I tried)
     * should add destroy on blockhit
     */ 

    interface IBullet
    {
        Rectangle CollisionRect { get; }
        int HitTarget();
        int HitTarget(Rectangle target);
    }

    abstract class BulletBlueprint : IBullet
    {
        protected Texture2D _texture;
        protected Vector2 _positie;
        protected Vector2 _size;

        public bool IsFired { get; set; }

        protected int _damage;

        public Rectangle CollisionRect { get {return new Rectangle(_positie.ToPoint(), _size.ToPoint()); } }

        public BulletBlueprint(Texture2D texture, Vector2 pos, Vector2 size, int damage)
        {
            _texture = texture;
            _positie = pos;
            _size = size;
            IsFired = false;
            _damage = damage;
        }

        public virtual int HitTarget()
        {
            IsFired = false;
            return _damage;
        }
        public virtual int HitTarget(Rectangle item)
        {
            int dam = 0;
            if (item.Intersects(CollisionRect) && IsFired)
            {
                dam = _damage;
                IsFired = false;
            }
            return dam;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (IsFired)
            {
                spriteBatch.Draw(_texture, _positie, Color.White);
            }
        }

        public void ResetBullet()
        {
            _positie = new Vector2(100, 5000);
            IsFired = false;
        }

    }
    class Bullet : BulletBlueprint
    {
        public Vector2 _direction; //speed is always constant here so, no velocity
        private double timeToLive; //works better than with

        public Bullet(Texture2D texture, int damage) : base(texture, new Vector2(0,0), new Vector2(50,50), damage)
        {
            timeToLive = 0;
        }
        public Bullet(Texture2D texture, Point size, int damage) : base(texture, new Vector2(0, 0), size.ToVector2(), damage)
        {
            timeToLive = 0;
        }

         
        
        public void Fire(Vector2 pos)
        {
            if (!IsFired)
            {
                _positie = pos;
                _direction.X = 0;
                _direction.Y = 10;
                IsFired = true;
                timeToLive = 1000;
            }

        }
        public void Fire(float angle, Vector2 pos)
        {
            if (!IsFired)
            {
                _positie = pos;
                _direction.X = (float)(Math.Cos(angle) * 1.5);
                _direction.Y = -(float)(Math.Sin(angle) * 1.5);
                IsFired = true;
                timeToLive = 1000;
            }

        }

        public void Update(GameTime gameTime)
        {
            timeToLive -= gameTime.ElapsedGameTime.TotalMilliseconds;
            if (IsFired)
            {
                _positie.X += Convert.ToInt32(gameTime.ElapsedGameTime.TotalMilliseconds * _direction.X);
                _positie.Y += Convert.ToInt32(gameTime.ElapsedGameTime.TotalMilliseconds * _direction.Y);
                if ((timeToLive < 0))
                {
                    HitTarget();
                }
            }
            else
            {
                _positie = new Vector2(0, 1000);
            }

            
        }

        public override int HitTarget() //aka the reset function, returns the damage
        {
            IsFired = false;
            return base.HitTarget();
        }
    }

    class MeleeBullet : BulletBlueprint
    {

        private float _angle;
        public MeleeBullet(Texture2D texture, Vector2 pos, Vector2 size, int damage) : base(texture, pos, size, damage)
        {
        }
        public void Update(float angle, Vector2 pos, bool isAttacking)
        {
            _angle = angle;
            _positie = pos;
            IsFired = isAttacking;
        }

        public void SetDamage(int damage)
        {
            _damage = damage;
        }

        public Rectangle GetCollisonBox()
        {
            Rectangle attack = new Rectangle(new Point(), _size.ToPoint());
            int size = Convert.ToInt16(_size.X);    //We gaan er vanuit dat de collisionbox een vierkant is, kan aangepast worden naar rectangle
            if (Math.Cos(_angle) > 0 && Math.Sin(_angle) > 0)
            {
                attack.Location = _positie.ToPoint() + new Point(0, -size);
            }
            else if (Math.Cos(_angle) <= 0 && Math.Sin(_angle) > 0)
            {
                attack.Location = _positie.ToPoint() + new Point(-size, -size);
            }
            else if (Math.Cos(_angle) <= 0 && Math.Sin(_angle) <= 0)
            {
                attack.Location = _positie.ToPoint() + new Point(-size, 0);
            }
            else if (Math.Cos(_angle) > 0 && Math.Sin(_angle) <= 0)
            {
                attack.Location = _positie.ToPoint();
            }
            return attack;
        }

        public override int HitTarget(Rectangle item)
        {
            int damage = 0;
            if (item.Intersects(GetCollisonBox()) && IsFired)
            {
                damage = _damage;
            }
            return damage;
        }

    }

    class Flame : BulletBlueprint
    {
        private int _id;
        Random r;
        public Flame(Texture2D texture, Vector2 pos, Vector2 size, int damage, int id) : base(texture, pos, size, damage)
        {
            _id = id;
            r = new Random();
            
        }

        public void Update(Vector2 pos, float angle, bool isFire)
        {
            IsFired = isFire;
            _positie = pos;
            if (IsFired)
            {
                _positie.X += (float)(Math.Cos(angle) * (float)(_id * _size.X)*1.5 + (General.random.Next(-5,5) * _id));
                _positie.Y += -(float)(Math.Sin(angle) * (float)(_id * _size.Y) * 1.5 + (General.random.Next(-5,5) * _id));
            }
            else
            {
                _positie = new Vector2(0, 1000);
            }
            
        }


    }

    class SheepBeam : BulletBlueprint
    {
        private float _id;
        Random r;
        public SheepBeam(Texture2D texture, Vector2 pos, Vector2 size, int damage, int id) : base(texture, pos, size, damage)
        {
            _id = id;
            r = new Random();

        }

        public void Update(Vector2 pos, float angle, bool isFire)
        {
            IsFired = isFire;
            _positie = pos;
            if (IsFired)
            {
                _positie.X += (float)(Math.Cos(angle) * (float)(_id * _size.X) *0.90);
                _positie.Y += -(float)(Math.Sin(angle) * (float)(_id * _size.Y) * 0.90);
            }
            else
            {
                _positie = new Vector2(0, 1000);
            }
        }
    }
}
