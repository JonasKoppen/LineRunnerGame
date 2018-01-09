using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineRunnerShooter.Blocks
{
    abstract class BlockBlueprint
    {
        protected Vector2 _positie;
        protected int _textureNum;
        protected Rectangle _texturePos;

        public BlockBlueprint(int texture, Vector2 pos)
        {
            _textureNum = texture;
            _positie = pos;
            _texturePos = new Rectangle(0, 0, 100, 100);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(General._afbeeldingBlokken[_textureNum], _positie, _texturePos, Color.White);
            //spriteBatch.Draw(_texture, getCollisionRectagle(), Color.Red); //Check collision block locations
        }
    }
}
