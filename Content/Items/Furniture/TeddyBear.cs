﻿using Macrocosm.Common.Sets;
using Terraria;
using Terraria.ModLoader;

namespace Macrocosm.Content.Items.Furniture
{
    public class TeddyBear : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemSets.UnobtainableItem[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.TeddyBear>(), tileStyleToPlace: 0);
            Item.width = 20;
            Item.height = 28;
            Item.value = 500;
        }
    }
}