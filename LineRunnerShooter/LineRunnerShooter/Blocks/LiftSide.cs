using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LineRunnerShooter.Blocks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LineRunnerShooter
{
    /*
     * Animated lift side (used for intro)
     * 
     */
    class LiftSide : BlockBlueprint, IUpdatetableBlock, ICollidableBlocks
    {
        double slow; 
        public LiftSide(int texture, Vector2 pos, bool isRight) : base(texture, pos)
        {
            _texturePos.X = isRight ? 100 : 0;
            _texturePos.Height = 200;
            slow = 10;
        }

        public Rectangle GetCollisionRectagle()
        {
            return new Rectangle(_positie.ToPoint(), _texturePos.Size);
        }

        public void Update(GameTime gameTime)
        {
            _texturePos.Y += Convert.ToInt16(gameTime.ElapsedGameTime.TotalMilliseconds / slow);
            if(_texturePos.Y > 800)
            {
                _texturePos.Y = 0;
            }
        }
    }
}
