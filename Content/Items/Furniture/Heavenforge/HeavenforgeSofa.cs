﻿using Macrocosm.Common.Enums;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Macrocosm.Content.Items.Furniture.Heavenforge
{
    public class HeavenforgeSofa : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.Luminite.LuminiteSofa>(), (int)LuminiteStyle.Heavenforge);
            Item.width = 34;
            Item.height = 24;
            Item.value = 150;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HeavenforgeBrick, 5)
                .AddIngredient(ItemID.Silk, 2)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
