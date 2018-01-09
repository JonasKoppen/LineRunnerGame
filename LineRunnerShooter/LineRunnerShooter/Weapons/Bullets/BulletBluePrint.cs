using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LineRunnerShooter.Weapons.Bullets
{
    abstract class BulletBlueprint : IBullet
    {
        protected Texture2D _texture;
        protected Vector2 _positie;
        protected Vector2 _size;

        public bool IsFired { get; set; }

        protected int _damage;

        public Rectangle CollisionRect { get { return new Rectangle(_positie.ToPoint(), _size.ToPoint()); } }

        Rectangle IBullet.CollisionRect => throw new NotImplementedException();

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

        public int HitTarget(Rectangle target)
        {
            throw new NotImplementedException();
        }
    }
}
