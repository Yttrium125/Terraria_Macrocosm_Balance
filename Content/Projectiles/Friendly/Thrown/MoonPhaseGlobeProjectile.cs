﻿using Macrocosm.Common.Utils;
using Macrocosm.Content.Subworlds;
using Terraria;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace Macrocosm.Content.Projectiles.Friendly.Thrown
{
    public class MoonPhaseGlobeProjectile : ModProjectile
    {
        public override string Texture => "Macrocosm/Content/Items/Consumables/Throwable/MoonPhaseGlobe";

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.aiStyle = 2;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item107, Projectile.position);

            for (int i = 0; i < 15; i++)
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Glass, 0f, -2f, 0, default, 1.5f);

            if (Main.netMode != NetmodeID.MultiplayerClient)
                Utility.ProgressMoonPhase();
        }
    }
}
