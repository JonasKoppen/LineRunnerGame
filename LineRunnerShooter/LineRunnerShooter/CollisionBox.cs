using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineRunnerShooter
{
    class CollisionBox
    {
        Rectangle body;
        Rectangle feet;
        Rectangle left;
        Rectangle right;
        public Rectangle Body { get { return body; } }
        public Rectangle Feet { get { return feet; } }
        public Rectangle Left { get { return left; } }
        public Rectangle Right { get { return right; } }

        public CollisionBox(int x, int y, int w, int h)
        {
            body = new Rectangle(x, y, w, h - 20);
            feet = new Rectangle(x, y + h - 20, w, 20);
            left = new Rectangle(x-5, y, w / 2, h-30);
            right = new Rectangle(x + (w / 2)+5, y, w / 2, h-30);
        }

        public virtual void Update(Point location)
        {
            body.Location = location;

            feet.Location = location;
            feet.Y += body.Height;

            left.Location = location;
            left.X -= 5;

            right.Location = location;
            right.X += right.Width+5;
        }

    }

    class RoboCollisionBox : CollisionBox
    {
        Rectangle senseLeft; //deze is ter hoogte van de block onder de voeten maar heeft een offset van -X, dit gaan we gebruiken om te zien of de volgende blok er wel is
        Rectangle senseRight;

        public Rectangle SenseLeft { get { return senseLeft; } }
        public Rectangle SenseRight { get { return senseRight; } }
        public RoboCollisionBox(int x, int y, int w, int h) : base(x, y, w, h)
        {
            senseLeft = new Rectangle(x - 100, y + h + 10, 20, 20);
            senseRight = new Rectangle(x +w+100, y + h + 10, 20, 20);
        }

        public override void Update(Point location)
        {
            base.Update(location);
            senseLeft.Location = location + new Point(-100,Body.Height+10);
            senseRight.Location = location + new Point(Body.Width + 100, Body.Height + 10);
        }
    }
}
