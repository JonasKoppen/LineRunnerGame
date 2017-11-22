using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LineRunnerShooter
{
    class LiftSide : Block
    {
        double slow; 
        public LiftSide(Texture2D texture, Vector2 pos, bool isRight) : base(texture, pos)
        {
            _texturePos.X = isRight ? 100 : 0;
            _texturePos.Height = 200;
            slow = 10;
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
