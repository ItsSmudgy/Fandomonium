using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader.Utilities;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Enums;
using Terraria.ObjectData;
using Terraria.GameContent.ItemDropRules;

namespace Fandomonium.Content.NPCs
{
	// The minions spawned when the body spawns
	// Please read MinionBossBody.cs first for important comments, they won't be explained here again
	public class Cockroach : ModNPC
	{
		// This is a neat trick that uses the fact that NPCs have all NPC.ai[] values set to 0f on spawn (if not otherwise changed).
		// We set ParentIndex to a number in the body after spawning it. If we set ParentIndex to 3, NPC.ai[0] will be 4. If NPC.ai[0] is 0, ParentIndex will be -1.
		// Now combine both facts, and the conclusion is that if this NPC spawns by other means (not from the body), ParentIndex will be -1, allowing us to distinguish
		// between a proper spawn and an invalid/"cheated" spawn
		public int ParentIndex {
			get => (int)NPC.ai[0] - 1;
			set => NPC.ai[0] = value + 1;
		}

		public bool HasParent => ParentIndex > -1;

		public int PositionIndex {
			get => (int)NPC.ai[1] - 1;
			set => NPC.ai[1] = value + 1;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cockroach");
			Main.npcFrameCount[Type] = 5;
			// Specify the debuffs it is immune to
			NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData
			{
				SpecificallyImmuneTo = new int[] {
					BuffID.Confused // Most NPCs have this
				}
			};
			NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);
		}

		public override void SetDefaults() {
			NPC.width = 30;
			NPC.height = 30;
			NPC.damage = 50;
			NPC.defense = 75;
			NPC.lifeMax = 200;
			NPC.HitSound = SoundID.NPCHit31;
			NPC.DeathSound = SoundID.NPCDeath34;
			NPC.noGravity = false;
			NPC.noTileCollide = false;
			NPC.knockBackResist = 0.8f;
			NPC.aiStyle = 41;
			AnimationType = NPCID.Herpling;
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
            // we would like this npc to spawn in the overworld.
            if (NPC.downedMechBossAny)
            {
                ;
            }

            {
				return SpawnCondition.Underground.Chance * 0.5f;
			}
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			int itemType = ItemID.CursedFlame;
			var parameters = new DropOneByOne.Parameters()
			{
				ChanceNumerator = 1,
				ChanceDenominator = 1,
				MinimumStackPerChunkBase = 1,
				MaximumStackPerChunkBase = 1,
				MinimumItemDropsCount = 1,
				MaximumItemDropsCount = 3,
			};
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption,
				new FlavorTextBestiaryInfoElement("A disgusting cockroach.")
			});
		}

		public override Color? GetAlpha(Color drawColor) {
			if (NPC.IsABestiaryIconDummy) {
			}
			return Color.White * NPC.Opacity;
		}
		public override void HitEffect(int hitDirection, double damage) {
			if (NPC.life <= 0) {
				// If this NPC dies, spawn some visuals

				int dustType = 5; // Some blue dust, read the dust guide on the wiki for how to find the perfect dust

				for (int i = 0; i < 20; i++) {
					Vector2 velocity = NPC.velocity + new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
					Dust dust = Dust.NewDustPerfect(NPC.Center, dustType, velocity, 26, Color.White, Main.rand.NextFloat(1.5f, 2.4f));

					dust.noLight = true;
					dust.noGravity = true;
					dust.fadeIn = Main.rand.NextFloat(0.3f, 0.8f);
				}
			}
		}
	}
}
