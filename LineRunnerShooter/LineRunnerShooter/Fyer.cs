﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LineRunnerShooter
{
    class Fyer : User //TODO: flyer of turret zodat er een challenge is om op meerdere nivea's te schieten
    {
        public Fyer(Texture2D textureL, Texture2D textureR, MoveMethod move, Texture2D bullet) : base(textureL, textureR, move, bullet)
        {

        }

        public void Update(GameTime gameTime, KeyboardState stateKey, Vector2 locHeld)
        {
            Update(gameTime, stateKey);
        }

        public override void MoveVertical(double time)
        {
            
        }
    }

    class FlyerMethod : MoveMethod
    {
        public override void Update(KeyboardState stateKey, bool canLeft, bool canRight)
        {
            throw new NotImplementedException();
        }
    }
}
