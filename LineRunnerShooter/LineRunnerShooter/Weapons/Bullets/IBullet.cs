using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineRunnerShooter.Weapons.Bullets
{
    interface IBullet
    {
        Rectangle CollisionRect { get; }
        int HitTarget();
        int HitTarget(Rectangle target);
    }
}
