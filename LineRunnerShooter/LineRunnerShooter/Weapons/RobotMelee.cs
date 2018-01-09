using LineRunnerShooter.Weapons;
using LineRunnerShooter.Weapons.Bullets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineRunnerShooter.Weapons
{
    class RobotMeleeARM //Dit is een melee attack
    {
        private Texture2D pixel;
        private float angle;
        private Vector2 _position;
        bool isAttacking;
        private MeleeBullet meleeBullet;

        public List<BulletBlueprint> Bullets { get { return GetBullets(); } }

        public RobotMeleeARM(Texture2D pix)
        {
            angle = 0;
            pixel = pix;
            _position = new Vector2(200, 240);
            meleeBullet = new MeleeBullet(pix, _position, new Vector2(80, 80), 1);
        }
        public void Update(GameTime gameTime, Vector2 position, int dir)
        {
            _position = position;
            _position.X += 50;
            _position.Y += 70 + ((dir - 1) * -20);
            if (isAttacking)
            {
                angle += (float)((Math.PI / 180) * gameTime.ElapsedGameTime.TotalMilliseconds * 2);
            }
            else
            {
                angle = (float)((Math.PI) * (dir - 1));
            }
            isAttacking = false;
            meleeBullet.Update(angle, _position, isAttacking);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, 81, 36);
            Vector2 origin = new Vector2(5, 10);
            spriteBatch.Draw(pixel, _position, sourceRectangle, Color.White, angle, origin, 1.0f, SpriteEffects.None, 1);

        }

        public void Fire()
        {
            isAttacking = true;
        }

        private List<BulletBlueprint> GetBullets()
        {
            List<BulletBlueprint> bullets = new List<BulletBlueprint>() {
                                                            meleeBullet
                                                            };
            return bullets;
        }

        public void SetDamage(int damage)
        {
            meleeBullet.SetDamage(damage);
        }

        public void Disable()
        {
            meleeBullet.ResetBullet();
        }
    }
}
