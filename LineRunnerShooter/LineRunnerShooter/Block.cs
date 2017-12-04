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
    class Block : ICollide
    {
        public Texture2D _texture;
        public Vector2 Positie;
        protected Rectangle _texturePos;
        public bool isHazard { get; private set; }

        public Block(Texture2D texture, Vector2 pos)
        {
            _texture = texture;
            Positie = pos;
            _texturePos = new Rectangle(0,0, 100,100);
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

    class Target : Block
    {
        bool isShot;
        int _value;
        public Target(Texture2D texture, Vector2 pos, int value) : base(texture, pos)
        {
            _value = value;
            isShot = false;
        }

        public void Update() //Collision with bullets
        {
            if (false)
            {
                isShot = true;
            }
        }

        public int getPoints()
        {
            int points = 0;
            if (isShot)
            {
                points = _value;
            }
            return points;
        }
    }
}
