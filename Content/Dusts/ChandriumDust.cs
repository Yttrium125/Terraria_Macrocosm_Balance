﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Macrocosm.Content.Dusts
{
    public class ChandriumDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noLight = true;
            dust.scale = Main.rand.NextFloat(1, 1.35f);
        }

        public override bool Update(Dust dust)
        {
            if (!dust.noGravity)
                dust.velocity.Y += 0.025f;

            dust.position += dust.velocity;
            dust.scale -= 0.02f;
            dust.rotation += (dust.velocity.Y - dust.velocity.X) / 5;

            if (dust.scale < 0f)
                dust.active = false;

            return false;
        }

        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return new Color(lightColor.R, lightColor.G, lightColor.B, 255);
        }
    }
}