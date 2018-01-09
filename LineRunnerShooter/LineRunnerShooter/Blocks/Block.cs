using LineRunnerShooter.Blocks;
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
     * Blocks are the foundation of the game, the level(builder) wil use them for making the level, not much happens here after construction is complete
     * The Lava class is like the Block but without the collionRectangle but with an animation wich requires an update method
     * The target block has also no collisoin rect, but will be shootable, if destroyed it wil return points.
     */

    class Block : BlockBlueprint, ICollidableBlocks //Moet een block bleuprint maken
    {
        public Block(int texture, Vector2 pos) : base(texture, pos)
        {  
        }

        public Block(int texture, Vector2 pos, Vector2 textPos) : base(texture, pos)
        {
            _texturePos = new Rectangle(textPos.ToPoint(), new Point(100,100));
        }

        public virtual Rectangle GetCollisionRectagle()
        {
            return new Rectangle(_positie.ToPoint(),_texturePos.Size);
        }
    }
}
