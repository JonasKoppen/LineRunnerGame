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

        public void Update(Point location)
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
}
