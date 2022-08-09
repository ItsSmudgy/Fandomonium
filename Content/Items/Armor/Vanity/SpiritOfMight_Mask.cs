using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fandomonium.Content.Items.Armor.Vanity
{
	[AutoloadEquip(EquipType.Head)]
	public class SpiritOfMight_Mask : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Spirit Of Might Mask");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 22;
			Item.height = 28;

			// Common values for every boss mask
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(silver: 75);
			Item.vanity = true;
			Item.maxStack = 1;
		}
	}
}
