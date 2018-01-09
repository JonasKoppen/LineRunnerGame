using LineRunnerShooter.Weapons.Bullets;
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
     * Like bullets but made for the boss to work like rockets, should add explosion on ground hit, this one is scripted , can be used for the Annihilator
     * Like the boss itself, this bullets are more scripted than the rest
     */
    class BulletR : BulletBlueprint
    {
        public Vector2 DestPos;
        public Vector2 _direction;
        public bool isGoingUp;
        private int damage;

        public BulletR() : base(General._afbeeldingEnemys[10], new Vector2(0, 0), new Vector2(50, 50), 1)
        {
            isGoingUp = true;
            damage = _damage;
        }
        public BulletR(Point size) : base(General._afbeeldingEnemys[10], new Vector2(0,0), size.ToVector2(), 1)
        {
            isGoingUp = true;
        }
        public  void Fire(float angle, Vector2 pos)
        {
            if (!IsFired)
            {
                _positie = pos;
                _direction.X = Convert.ToInt16(Math.Cos(angle) * 10);
                _direction.Y = -Convert.ToInt16(Math.Sin(angle) * 8);
                IsFired = true;
                isGoingUp = true;
                
            }
            
        }
        public void Fire(Vector2 startPos, Vector2 destPos)
        {
            if (!IsFired)
            {
                DestPos = destPos;
                _positie = startPos;
                _direction.X = 0;
                _direction.Y = -15;
                IsFired = true;
            }
        }

        public void Update()
        {
            if (IsFired)
            {
                _positie = Vector2.Add(_positie, _direction);
                if(_positie.Y < 0)
                {
                    _positie.X = DestPos.X;
                    _direction.Y = 10;
                    isGoingUp = false;
                    _texture = General._afbeeldingEnemys[9];
                }
            }
            if (_positie.Y > 3000)
            {
                IsFired = false;
                isGoingUp = true;
                _positie = new Vector2(0,1000000);
                _texture = General._afbeeldingEnemys[10];
            }
        }

        public Rectangle GetCollisionRectagle()
        {
            Rectangle collision = new Rectangle(0,0,1,1);
            if(IsFired && !isGoingUp)
            {
                collision = CollisionRect;
            }
            return collision;
        }

        public override int HitTarget(Rectangle item) //not needed anymore
        {
            int damage = 0;
            if(IsFired && !isGoingUp)
            {
                damage = base.HitTarget(item);
            }
            Console.WriteLine("hi");
            return damage;
        }
    }
}
