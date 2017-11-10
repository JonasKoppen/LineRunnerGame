using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineRunnerShooter
{
    class Block : ICollide
    {
        public Texture2D _texture;
        public Vector2 Positie;
        protected Rectangle collisionRect;
        protected Rectangle _texturePos;

        public Block(Texture2D texture, Vector2 pos)
        {
            _texture = texture;
            Positie = pos;
            collisionRect = new Rectangle(pos.ToPoint(), new Point(100,50));
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            
            spriteBatch.Draw(_texture, Positie, Color.Gold);
            
        }

        public void Draw(SpriteBatch spriteBatch, int i)
        {
            spriteBatch.Draw(_texture, Positie, Color.Yellow);
            spriteBatch.Draw(_texture, this.getCollisionRectagle(), Color.Red); //Check collision block locations

        }

        public virtual Rectangle getCollisionRectagle()
        {
            collisionRect.Location = Positie.ToPoint();
            return collisionRect;
        }
    }
}
