using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Fandomonium.Content.Items;
using Microsoft.Xna.Framework;

namespace Fandomonium.Content.Items.Weapons
{
	public class Arcane_WandT5 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arcane Wand");
			Tooltip.SetDefault("Tier 5");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 222;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 10;
			Item.width = 4;
			Item.height = 4;
			Item.useTime = 10;
			Item.useAnimation = 5;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 0;
			Item.value = 10000;
			Item.rare = ItemRarityID.Expert;
			Item.UseSound = SoundID.Item9;
			Item.autoReuse = true;
			Item.shoot = ProjectileID.ChlorophyteBullet;
            Item.shootSpeed = 15;
        }

        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Arcane_WandT4>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Celestial_Arcane_Dust>(), 20);
			recipe.AddIngredient(ModContent.ItemType<Essence_of_Light>(), 10);
			recipe.AddIngredient(ModContent.ItemType<Essence_of_Night>(), 10);
			recipe.AddIngredient(ModContent.ItemType<Essence_of_Sight>(), 10);
			recipe.AddIngredient(ModContent.ItemType<Essence_of_Might>(), 10);
            recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}