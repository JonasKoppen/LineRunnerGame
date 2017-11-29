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
        public bool isHazard { get; private set; }

        public Block(Texture2D texture, Vector2 pos)
        {
            _texture = texture;
            Positie = pos;
            _texturePos = new Rectangle(0,0, 100,100);
        }

        public Block(Texture2D texture, Vector2 pos, bool hazard)
        {
            _texture = texture;
            Positie = pos;
            _texturePos = new Rectangle(0, 0, 100, 100);
            isHazard = hazard;
        }

        public Block(Texture2D texture, Vector2 pos, Vector2 textPos)
        {
            _texture = texture;
            Positie = pos;
            _texturePos = new Rectangle(textPos.ToPoint(), new Point(100,100));
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Positie ,_texturePos, Color.White);
            //spriteBatch.Draw(_texture, getCollisionRectagle(), Color.Red); //Check collision block locations
        }

        public void Draw(SpriteBatch spriteBatch, int i)
        {
            spriteBatch.Draw(_texture, Positie, Color.Yellow);
            spriteBatch.Draw(_texture, getCollisionRectagle(), Color.Red); //Check collision block locations

        }

        public virtual Rectangle getCollisionRectagle()
        {
            
            return new Rectangle(Positie.ToPoint(),_texturePos.Size); ;
        }
    }

    class Lava : Block
    {
        int dir = 1;
        public Lava(Texture2D texture, Vector2 pos) : base(texture, pos)
        {
        }

        public void Update(GameTime gameTime)
        {
            double shiftX = gameTime.ElapsedGameTime.TotalMilliseconds / 8;
            _texturePos.X += (int)(shiftX *dir);
            if(_texturePos.X > 180)
            {
                dir = -1;
            }
            if (_texturePos.X < 20)
            {
                dir = 1;
            }
        }

        public override Rectangle getCollisionRectagle()
        {
            return new Rectangle();
        }
    }
}
