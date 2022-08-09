﻿using Fandomonium.Content.Pets.SpiritOfMightPet;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace Fandomonium.Content.Pets.SpiritOfLightPet
{
	public class SpiritOfLightPetItem : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Crystalline Shard");
			Tooltip.SetDefault("Summons a miniature Spirit Of Light to follow you");
			
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.CloneDefaults(ItemID.ZephyrFish); // Copy the Defaults of the Zephyr Fish Item.
			Item.rare = ItemRarityID.Master;
			Item.master = true;
			Item.shoot = ModContent.ProjectileType<SpiritOfLightPetProjectile>(); // "Shoot" your pet projectile.
			Item.buffType = ModContent.BuffType<SpiritOfLightBuff>(); // Apply buff upon usage of the Item.
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame) {
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0) {
				player.AddBuff(Item.buffType, 3600);
			}
		}
	}
}
