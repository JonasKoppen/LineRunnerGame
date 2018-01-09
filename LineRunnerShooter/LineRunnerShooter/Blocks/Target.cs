using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineRunnerShooter.Blocks
{
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
            if (updateTime < 0)
            {
                _texturePos.X += _texturePos.Size.X;
                if (_texturePos.X >= General._afbeeldingBlokken[_textureNum].Width)
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
