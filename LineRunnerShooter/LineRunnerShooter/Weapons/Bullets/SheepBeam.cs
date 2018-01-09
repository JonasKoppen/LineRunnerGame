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
                _positie.X += (float)(Math.Cos(angle) * (float)(_id * _size.X) * 0.90);
                _positie.Y += -(float)(Math.Sin(angle) * (float)(_id * _size.Y) * 0.90);
            }
            else
            {
                _positie = new Vector2(0, 1000);
            }
        }
    }
}
