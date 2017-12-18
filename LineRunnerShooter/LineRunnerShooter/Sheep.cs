using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace LineRunnerShooter
{
    /*
     * Sheep, sheep, sheep, It is so fluffy, I want it
     * 
     * 
     */ 
    class Sheep : User
    {
        public Sheep(Texture2D textureL, Texture2D textureR, MoveMethod move, Texture2D bullet) : base(textureL, move, bullet) 
        {
        }


    }
}
