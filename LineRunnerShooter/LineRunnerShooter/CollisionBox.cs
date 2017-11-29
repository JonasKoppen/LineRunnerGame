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
        Rectangle head;
        Rectangle underFeet;
        public Rectangle Body { get { return body; } }
        public Rectangle Feet { get { return feet; } }
        public Rectangle Head { get { return head; } }
        public Rectangle Left { get { return left; } }
        public Rectangle Right { get { return right; } }
        public Rectangle UnderFeet { get { return underFeet; } }

        public CollisionBox(int x, int y, int w, int h)
        {
            body = new Rectangle(x, y, w, h - 20);
            feet = new Rectangle(x+10, y + h - 20, w-20, 20);
            head = new Rectangle(x+10, y, w-20, 20);
            left = new Rectangle(x-5, y, w / 2, h-30);
            right = new Rectangle(x + (w / 2)+5, y, w / 2, h-30);
            underFeet = new Rectangle(x + 10, y + h, w - 20, 5);
        }

        public virtual void Update(Point location)
        {
            body.Location = location;

            feet.Location = location;
            feet.X += 10;
            feet.Y += body.Height;

            head.Location = location;
            head.X += 10;

            left.Location = location;
            left.X -= 5;

            right.Location = location;
            right.X += right.Width+5;

            underFeet.Location = location;
            underFeet.X += 10;
            underFeet.Y += 20+ body.Height;
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
