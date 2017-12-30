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

    class Lava : BlockBlueprint, IUpdatetableBlock
    {
 
        public Lava(int texture, Vector2 pos) : base(texture, pos)
        {
        }

        public void Update(GameTime gameTime)
        {
            double shiftX = gameTime.ElapsedGameTime.TotalMilliseconds / 8;
            _texturePos.X = Convert.ToInt16((_texturePos.Size.X / 2) + Math.Sin(gameTime.TotalGameTime.TotalMilliseconds/500)* (_texturePos.Size.X / 2));
        }

    }

    class Target : BlockBlueprint, IUpdatetableBlock
    {
        bool isShot;
        int _value;
        Rectangle hitBox;
        double updateTime;
        public Target(int texture, Vector2 pos, int value) : base(12, pos)
        {
            _texturePos.Size = new Point(100, 100);
            _value = value;
            isShot = false;
            hitBox = new Rectangle(pos.ToPoint(), _texturePos.Size);
            updateTime = 250;
        }

        public void Update(GameTime gameTime)
        {
            updateTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
            if(updateTime < 0)
            {
                _texturePos.X += _texturePos.Size.X;
                if(_texturePos.X >= General._afbeeldingBlokken[_textureNum].Width)
                {
                    _texturePos.X = 0;
                }
                updateTime = 250;
            }
        }

        public void HitTarget()
        {
            isShot = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!isShot)
            {
                base.Draw(spriteBatch);
            }
        }

        public int GetPoints()
        {
            int points = 0;
            if (isShot)
            {
                points = _value;
                _value = 0;
            }
            return points;
        }

        public Rectangle GetCollisionRectagle()
        {
            Rectangle rect = new Rectangle();
            if (!isShot)
            {
                rect = new Rectangle(_positie.ToPoint(), _texturePos.Size);
            }
            return rect;
        }
    }
}
