using Fandomonium.Content.NPCs;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ModLoader.Utilities;
using Terraria.Enums;
using Terraria.ObjectData;
using Terraria.GameContent.ItemDropRules;

namespace Fandomonium.Content.NPCs
{
	// These three class showcase usage of the WormHead, WormBody and WormTail classes from Worm.cs
	internal class Intestiworm_Head : WormHead
	{
		public override int BodyType => ModContent.NPCType<Intestiworm_Body>();

		public override int TailType => ModContent.NPCType<Intestiworm_Tail>();

		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Intestiworm");

			var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers(0) { // Influences how the NPC looks in the Bestiary
				CustomTexturePath = "Fandomonium/Content/NPCs/Intestiworm_Bestiary", // If the NPC is multiple parts like a worm, a custom texture for the Bestiary is encouraged.
				Position = new Vector2(40f, 24f),
				PortraitPositionXOverride = 0f,
				PortraitPositionYOverride = 12f
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
		}

		public override void SetDefaults() {
			// Head is 10 defence, body 20, tail 30.
			NPC.CloneDefaults(NPCID.DevourerHead);
			NPC.aiStyle = -1;
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			// we would like this npc to spawn in the overworld.
			return SpawnCondition.Crimson.Chance * 0.1f;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("A disgusting, intestine-like worm of the Corrosion.")
			});
		}

		public override void Init() {
			// Set the segment variance
			// If you want the segment length to be constant, set these two properties to the same value
			MinSegmentLength = 6;
			MaxSegmentLength = 12;

			CommonWormInit(this);
		}

		// This method is invoked from ExampleWormHead, ExampleWormBody and ExampleWormTail
		internal static void CommonWormInit(Worm worm) {
			// These two properties handle the movement of the worm
			worm.MoveSpeed = 5.5f;
			worm.Acceleration = 0.045f;
		}

		private int attackCounter;
		public override void SendExtraAI(BinaryWriter writer) {
			writer.Write(attackCounter);
		}

		public override void ReceiveExtraAI(BinaryReader reader) {
			attackCounter = reader.ReadInt32();
		}

		public override void AI() {
			if (Main.netMode != NetmodeID.MultiplayerClient) {
				if (attackCounter > 0) {
					attackCounter--; // tick down the attack counter.
				}
			}
		}
	}

	internal class Intestiworm_Body : WormBody
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Intestiworm");

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0) {
				Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
		}

		public override void SetDefaults() {
			NPC.CloneDefaults(NPCID.DiggerBody);
			NPC.aiStyle = -1;
		}

		public override void Init() {
			Intestiworm_Head.CommonWormInit(this);
		}
	}

	internal class Intestiworm_Tail : WormTail
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Intestiworm");

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0) {
				Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
		}

		public override void SetDefaults() {
			NPC.CloneDefaults(NPCID.DiggerTail);
			NPC.aiStyle = -1;
		}

		public override void Init() {
			Intestiworm_Head.CommonWormInit(this);
		}
	}
}
