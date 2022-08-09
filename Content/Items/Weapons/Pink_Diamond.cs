using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Fandomonium.Content.Projectiles;
using Terraria.GameContent.Creative;

namespace Fandomonium.Content.Items.Weapons
{
    internal class Pink_Diamond : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pink Diamond"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            Tooltip.SetDefault("It was a tragedy, what happened to her.");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;
            Item.damage = 165;
            Item.knockBack = 3.2f;
            Item.useTime = 10;
            Item.value = 10000;
            Item.useAnimation = 10;
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item11;
            Item.shoot = ProjectileID.CrystalBullet;
            Item.useAmmo = AmmoID.Bullet;
            Item.shootSpeed = 2;
            Item.autoReuse = true;
        }
    }
}
