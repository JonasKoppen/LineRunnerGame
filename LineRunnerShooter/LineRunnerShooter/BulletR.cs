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
        public bool isExploding;
        private int damage;

        public BulletR(Texture2D texture) : base(texture, new Vector2(0, 0), new Vector2(50, 50), 1, 2)
        {
            isGoingUp = true;
            damage = _damage;
        }
        public BulletR(Texture2D texture, Point size) : base(texture, new Vector2(0,0), size.ToVector2(), 1, 2)
        {
            isGoingUp = true;
        }
        public  void fire(float angle, Vector2 pos)
        {
            if (!isFired)
            {
                Positie = pos;
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
                Positie = startPos;
                _direction.X = 0;
                _direction.Y = -15;
                isFired = true;
            }
        }

        public void Update()
        {
            if (isFired)
            {
                Positie = Vector2.Add(Positie, _direction);
                if(Positie.Y < 0)
                {
                    Positie.X = DestPos.X;
                    _direction.Y = 10;
                    isGoingUp = false;
                }
            }
            if(isFired && !isGoingUp)
            {
                _damage = 0;
            }
            else
            {
                _damage = damage;
            }
            if (Positie.Y > 3000)
            {
                isFired = false;
                isGoingUp = true;
                Positie = new Vector2(0,0);
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

        public Rectangle getDamageZone() //brede collisionbox
        {
            Rectangle damageZone = new Rectangle();
            if (isGoingUp)
            {
                
            }
            //else with rocket size, and an other else clause with the rocket on explosion
            
            return damageZone;
        }

        public void HitTarge()
        {
            isFired = false;
            Positie = new Vector2();
        }

        public override int hitTarget(Rectangle item)
        {
            int damage = 0;
            if(isFired && !isGoingUp)
            {
                damage = base.hitTarget(item);
            }
            Console.WriteLine("hi");
            return damage;
        }
    }
}
