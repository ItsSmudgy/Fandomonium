using Microsoft.Xna.Framework;
using Fandomonium.Common.Systems;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Fandomonium.Content.Pets.SpiritOfMightPet;
using Fandomonium.Content.Items.Consumables;
using Fandomonium.Content.Items.Placeable.Relics;
using Fandomonium.Content.Items;
using Fandomonium.Content.Items.Armor.Vanity;
using Fandomonium.Content.BossBars;

namespace Fandomonium.Content.NPCs.SpiritOfMight
{
    [AutoloadBossHead]
    internal class SpiritOfMight_Head : WormHead
    {
        public override int BodyType => ModContent.NPCType<SpiritOfMight_Body>();

        public override int TailType => ModContent.NPCType<SpiritOfMight_Tail>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spirit Of Might");

            NPCID.Sets.TrailCacheLength[NPC.type] = 5;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPCID.Sets.NoMultiplayerSmoothingByType[NPC.type] = true;

            NPCID.Sets.BossBestiaryPriority.Add(NPC.type);
            NPCID.Sets.DebuffImmunitySets.Add(NPC.type, new Terraria.DataStructures.NPCDebuffImmunityData
            {
                ImmuneToAllBuffsThatAreNotWhips = true
            });

            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                CustomTexturePath = "Fandomonium/Content/NPCs/SpiritOfMight/SpiritOfMight_Still",
                Scale = 0.1f,
                Position = new Vector2(0 * 0 * 0, 0),
                PortraitScale = 0.1f,
            });
        }
        public override void Load()
        {
        }

        public override void BossHeadSlot(ref int index)
        {
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
                new FlavorTextBestiaryInfoElement("A worm made out of only the strongest of stones, powered by Souls of Might!")
            });
        }
        public override void Init()
        {
            // Set the segment variance
            // If you want the segment length to be constant, set these two properties to the same value
            MinSegmentLength = 6;
            MaxSegmentLength = 12;

            CommonWormInit(this);
        }

        // This method is invoked from ExampleWormHead, ExampleWormBody and ExampleWormTail
        internal static void CommonWormInit(Worm worm)
        {
            // These two properties handle the movement of the worm
            worm.MoveSpeed = 5.5f;
            worm.Acceleration = 0.125f;
        }

        private int attackCounter;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(attackCounter);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            attackCounter = reader.ReadInt32();
        }

    public override void SetDefaults()
        {
            NPC.width = 159;
            NPC.height = 100;
            NPC.damage = 75;
            NPC.defense = 250;
            NPC.lifeMax = 61000;
            NPC.HitSound = SoundID.NPCHit41;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0f;
            NPC.lavaImmune = true;
            NPC.aiStyle = 6;
            NPC.boss = true;
            NPC.BossBar = ModContent.GetInstance<SpiritsBossBar>();
            Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Mighty-Worm");
            SceneEffectPriority = SceneEffectPriority.BossLow;
            NPC.behindTiles = true;
            NPC.trapImmune = true;
            NPC.boss = true;
            NPC.scale *= 1.5f;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            // If the NPC dies, spawn gore and play a sound
            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }

            if (NPC.life <= 0)
            {
                // These gores work by simply existing as a texture inside any folder which path contains "Gores/"
                int backGoreType = Mod.Find<ModGore>("SpiritOfMight_Head-Gore").Type;

                var entitySource = NPC.GetSource_Death();

                for (int i = 0; i < 1; i++)
                {
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), backGoreType);
                }

                SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
            }
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }

        public override void OnKill()
        {
            NPC.SetEventFlagCleared(ref DownedBossSystem.downedMinionBoss, -1);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Essence_of_Might>()));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SpiritOfMightTrophy>(), 10));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SpiritOfMight_Mask>(), 15));
            if (Main.expertMode)
            {
                ;
            }

            {
                npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<SpiritOfMightBag>()));
            }
            if (Main.masterMode)
            {
                ;
            }

            {
                npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<SpiritOfMightBag>()));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SpiritOfMightPetItem>(), 33));
                npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<SpiritOfMightRelic>()));
            }
        }

        public override void BossHeadRotation(ref float rotation)
        {
            rotation = NPC.rotation;
        }
    }
    internal class SpiritOfMight_Body : WormBody
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spirit Of Might");

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.width = 159;
            NPC.height = 100;
            NPC.damage = 75;
            NPC.defense = 80;
            NPC.lifeMax = 70000;
            NPC.HitSound = SoundID.NPCHit41;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0f;
            NPC.lavaImmune = true;
            NPC.aiStyle = 6;
            
            Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Mighty-Worm");
            NPC.boss = true;
            SceneEffectPriority = SceneEffectPriority.BossLow;

            NPC.behindTiles = true;
            NPC.trapImmune = true;

            NPC.scale *= 1.5f;
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            // If the NPC dies, spawn gore and play a sound
            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }

            if (NPC.life <= 0)
            {
                // These gores work by simply existing as a texture inside any folder which path contains "Gores/"
                int backGoreType = Mod.Find<ModGore>("SpiritOfMight_Body-Gore").Type;

                var entitySource = NPC.GetSource_Death();

                for (int i = 0; i < 1; i++)
                {
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), backGoreType);
                }

                SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
            }
        }

        public override void Init()
        {
            SpiritOfMight_Head.CommonWormInit(this);
        }
    }

    internal class SpiritOfMight_Tail : WormTail
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spirit Of Might");

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.width = 159;
            NPC.height = 100;
            NPC.damage = 75;
            NPC.defense = 80;
            NPC.lifeMax = 70000;
            NPC.HitSound = SoundID.NPCHit41;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0f;
            NPC.lavaImmune = true;
            NPC.aiStyle = 6;
            NPC.despawnEncouraged = false;
            Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Mighty-Worm");
            NPC.boss = true;
            SceneEffectPriority = SceneEffectPriority.BossLow;

            NPC.behindTiles = true;
            NPC.trapImmune = true;

            NPC.scale *= 1.5f;
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            // If the NPC dies, spawn gore and play a sound
            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }

            if (NPC.life <= 0)
            {
                // These gores work by simply existing as a texture inside any folder which path contains "Gores/"
                int backGoreType = Mod.Find<ModGore>("SpiritOfMight_Tail-Gore").Type;

                var entitySource = NPC.GetSource_Death();

                for (int i = 0; i < 1; i++)
                {
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), backGoreType);
                }

                SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
            }
        }

        public override void Init()
        {
            SpiritOfMight_Head.CommonWormInit(this);
        }
    }
}