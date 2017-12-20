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
     * 
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
        public  void fire(float angle, Vector2 pos)
        {
            if (!isFired)
            {
                _positie = pos;
                _direction.X = Convert.ToInt16(Math.Cos(angle) * 10);
                _direction.Y = -Convert.ToInt16(Math.Sin(angle) * 8);
                isFired = true;
                isGoingUp = true;
                
            }
            
        }
        public void fire(Vector2 startPos, Vector2 destPos)
        {
            if (!isFired)
            {
                DestPos = destPos;
                _positie = startPos;
                _direction.X = 0;
                _direction.Y = -15;
                isFired = true;
            }
        }

        public void Update()
        {
            if (isFired)
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
                isFired = false;
                isGoingUp = true;
                _positie = new Vector2(0,1000000);
                _texture = General._afbeeldingEnemys[10];
            }
        }

        public Rectangle getCollisionRectagle()
        {
            Rectangle collision = new Rectangle(0,0,1,1);
            if(isFired && !isGoingUp)
            {
                collision = CollisionRect;
            }
            return collision;
        }

        public override int HitTarget(Rectangle item) //not needed anymore
        {
            int damage = 0;
            if(isFired && !isGoingUp)
            {
                damage = base.HitTarget(item);
            }
            Console.WriteLine("hi");
            return damage;
        }
    }
}
