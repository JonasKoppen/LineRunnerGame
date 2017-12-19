using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineRunnerShooter
{
    static class General //general data that is needed everywhere, Deze klasse zorgt ervoor dat we niet 1 texture in 10 klassen steken (verminderd het geheugen)
    {
        public static Random r;
        public static SpriteFont font;
        public static SpriteFont fontBig;

        public static List<Texture2D> _afbeeldingBlokken;
        public static List<Texture2D> _afbeeldingEnemys;
        public static List<Texture2D> _levelMaps;


    }
}
