using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineRunnerShooter.Weapons
{
    abstract class ARMBluePrint
    {
        private Texture2D _texture;
        protected float angle;
        protected Vector2 _position;
        //public List<BulletBlueprint> bullets; // Dit is niet juist
        protected Rectangle sourceRectangle;
        private Vector2 size;

        public virtual List<BulletBlueprint> Bullets { get; protected set; }

        public ARMBluePrint(Texture2D texture)
        {
            _texture = texture;
            sourceRectangle = new Rectangle(0, 0, 80, 36);
        }

        public ARMBluePrint(Texture2D texture, Vector2 pos, Vector2 size) : this(texture)
        {
            this._position = pos;
            this.size = size;
            sourceRectangle = new Rectangle(0, 0, 80, 36);
        }

        public abstract void Update(GameTime gameTime, Vector2 position, Vector2 mouse);
        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2(5, 10);
            spriteBatch.Draw(_texture, _position, sourceRectangle, Color.White, -angle, origin, 1.0f, SpriteEffects.None, 1);
            foreach (BulletBlueprint b in Bullets)
            {
                b.Draw(spriteBatch);
            }
        }
        public abstract void Fire();

    }
    
}
