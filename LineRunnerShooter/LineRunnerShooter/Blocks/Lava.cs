using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineRunnerShooter.Blocks
{
    class Lava : BlockBlueprint, IUpdatetableBlock
    {

        public Lava(int texture, Vector2 pos) : base(texture, pos)
        {
        }

        public void Update(GameTime gameTime)
        {
            double shiftX = gameTime.ElapsedGameTime.TotalMilliseconds / 8;
            _texturePos.X = Convert.ToInt16((_texturePos.Size.X / 2) + Math.Sin(gameTime.TotalGameTime.TotalMilliseconds / 500) * (_texturePos.Size.X / 2));
        }

    }
}
