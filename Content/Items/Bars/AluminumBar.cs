﻿using Macrocosm.Content.Items.Ores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Macrocosm.Content.Items.Bars
{
    public class AluminumBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = Item.CommonMaxStack;
            Item.value = 750;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.createTile = TileType<Tiles.Bars.AluminumBar>();
            Item.placeStyle = 0;
            Item.rare = ItemRarityID.White;
            
            // Set other Item.X values here
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient<AluminumOre>(4)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}