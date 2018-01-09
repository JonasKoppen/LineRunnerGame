using LineRunnerShooter.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineRunnerShooter.Weapons
{
    class FlameThrower : ARMBluePrint
    {
        bool isFired;
        public FlameThrower(Texture2D texture, Texture2D flame) : base(texture)
        {
            Bullets = new List<BulletBlueprint>();
            isFired = false;
            for (int i = 1; i < 10; i++)
            {
                Bullets.Add(new Flame(flame, new Vector2(0, 1000), new Vector2(25, 25), i % 2, i));
            }
            sourceRectangle.Y = 70;
        }

        public override void Fire()
        {
            isFired = true;
        }

        public override void Update(GameTime gameTime, Vector2 position, Vector2 mouse)
        {
            _position = position;
            _position.X += 40;
            _position.Y += 65;

            float xVers = -mouse.X + _position.X;
            float yVers = -mouse.Y + _position.Y;
            angle = (float)Math.Atan2(xVers, yVers) + (float)(Math.PI / 2);
            //Console.WriteLine(angle);

            foreach (Flame f in Bullets)
            {
                f.Update(_position, angle, isFired);
            }
            isFired = false;
        }
    }
}
