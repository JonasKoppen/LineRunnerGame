using LineRunnerShooter.Weapons.Bullets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineRunnerShooter.Weapons.Bullets
{
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
}
