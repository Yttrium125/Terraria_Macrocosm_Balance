using Macrocosm.Common.DataStructures;
using Macrocosm.Common.Players;
using Macrocosm.Common.Sets;
using Macrocosm.Common.Utils;
using Macrocosm.Content.Biomes;
using Macrocosm.Content.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Macrocosm.Content.NPCs.Enemies.Pollution
{
    public class TarBlob : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;

            NPC.ApplyImmunity
            (
                BuffID.Bleeding,
                BuffID.BloodButcherer,
                BuffID.Poisoned,
                BuffID.Venom
            );
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            NPC.width = 22;
            NPC.height = 18;
            NPC.damage = 5;
            NPC.defense = 1;
            NPC.lifeMax = 10;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 60f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 1;
            NPC.alpha=10;
            AIType = NPCID.BlueSlime;
            Banner = Item.NPCtoBanner(NPCID.BlueSlime);
            BannerItem = Item.BannerToItem(Banner);
            SpawnModBiomes = [ModContent.GetInstance<PollutionBiome>().Type];
        }
        public override void FindFrame(int frameHeight)
        {
            int frameSpeed = 8;

            NPC.frameCounter++;

            if (NPC.frameCounter >= frameSpeed)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;

                if (NPC.frame.Y >= Main.npcFrameCount[Type] * frameHeight)
                {
                    NPC.frame.Y = 0;
                }
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0f;
        }

        public override void ModifyNPCLoot(NPCLoot loot)
        {

        }

        public override bool PreAI()
        { 
            return true;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int i = 0; i < 5; i++)
            {
                int dustIndex = Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<CoalDust>());
                Dust dust = Main.dust[dustIndex];
                dust.velocity.X *= dust.velocity.X * 1.25f * hit.HitDirection + Main.rand.Next(0, 100) * 0.015f;
                dust.velocity.Y *= dust.velocity.Y * 0.25f + Main.rand.Next(-50, 51) * 0.01f;
                dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
            }
        }


    }
}