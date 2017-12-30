using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineRunnerShooter
{
    interface IUpdatetableBlock
    {
        void Update(GameTime gameTime);
    }

    interface ICollidableBlocks
    {
        Rectangle GetCollisionRectagle();
    }
}
