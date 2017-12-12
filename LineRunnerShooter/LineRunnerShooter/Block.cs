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
        public Vector2 Positie; //Make property
        protected Rectangle _texturePos;
        protected int _textureNum;
        public bool isHazard { get; private set; }

        public Block(int texture, Vector2 pos)
        {
            _textureNum = texture;
            Positie = pos;
            _texturePos = new Rectangle(0, 0, 100, 100);
        }

        public Block(int texture, Vector2 pos, Vector2 textPos)
        {
            _textureNum = texture;
            Positie = pos;
            _texturePos = new Rectangle(textPos.ToPoint(), new Point(100,100));
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(General._afbeeldingBlokken[_textureNum], Positie ,_texturePos, Color.White);
            //spriteBatch.Draw(_texture, getCollisionRectagle(), Color.Red); //Check collision block locations
        }

        public void Draw(SpriteBatch spriteBatch, int i)
        {
            spriteBatch.Draw(General._afbeeldingBlokken[_textureNum], Positie, Color.Yellow);
            spriteBatch.Draw(General._afbeeldingBlokken[_textureNum], getCollisionRectagle(), Color.Red); //Check collision block locations

        }

        public virtual Rectangle getCollisionRectagle()
        {
            return new Rectangle(Positie.ToPoint(),_texturePos.Size); ;
        }
    }

    class Lava : Block
    {
        int dir = 1;
        public Lava(int texture, Vector2 pos) : base(texture, pos)
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
        Rectangle hitBox;
        public Target(int texture, Vector2 pos, int value) : base(texture, pos)
        {
            _texturePos.Size = new Point(100, 100);
            _value = value;
            isShot = false;
            hitBox = new Rectangle(pos.ToPoint(), _texturePos.Size);
        }

        public void Update(List<BulletBlueprint> bullets) //Collision with bullets
        {
            if (!isShot) //als het object al neergeschoten is moeten we niet nog is controleren op collisie
            {
                foreach (BulletBlueprint B in bullets)
                {
                    if (B.hitTarget(hitBox) > 0)
                    {
                        isShot = true;
                    }
                }
            }
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!isShot)
            {
                base.Draw(spriteBatch);
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
