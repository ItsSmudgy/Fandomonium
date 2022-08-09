using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Fandomonium.Content.Dusts;

namespace Fandomonium.Content.Projectiles
{
    internal class Crystal_Spike : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.aiStyle = ProjectileID.CrystalDart;
            Projectile.penetrate = 5;
        }
    }
}
