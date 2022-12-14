using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.Net;
using Terraria.GameContent.NetModules;

namespace Fandomonium.Content.Items
{
	public class Essence_of_Light : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Essence of Light"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("It emits a warmth.");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;

			ItemID.Sets.ItemNoGravity[Item.type] = true;
			ItemID.Sets.ItemIconPulse[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 64;
			Item.height = 64;
			Item.maxStack = 999; 
			Item.value = Item.buyPrice(silver: 25);
			Item.rare = ItemRarityID.Yellow;
		}
	}
}