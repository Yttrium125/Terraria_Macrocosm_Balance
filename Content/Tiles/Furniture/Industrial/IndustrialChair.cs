﻿using Macrocosm.Common.Sets;
using Macrocosm.Content.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Macrocosm.Content.Tiles.Furniture.Industrial
{
    [LegacyName("MoonBaseChair")]
    public class IndustrialChair : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;
            TileID.Sets.CanBeSatOnForNPCs[Type] = true;
            TileID.Sets.CanBeSatOnForPlayers[Type] = true;

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair);

            DustType = ModContent.DustType<IndustrialPlatingDust>();
            AdjTiles = [TileID.Chairs];

            AddMapEntry(new Color(200, 200, 200), Language.GetText("MapObject.Chair"));

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
            TileObjectData.newTile.CoordinateHeights = [16, 16];
            TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.newTile.StyleMultiplier = 2;
            TileObjectData.newTile.StyleWrapLimit = 2;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
            TileObjectData.addAlternate(1);
            TileObjectData.addTile(Type);

            TileSets.RandomStyles[Type] = 4;

            // Including broken styles
            RegisterItemDrop(ModContent.ItemType<Items.Furniture.Industrial.IndustrialChair>(), 0, 1);
            RegisterItemDrop(ModContent.ItemType<Items.Furniture.Industrial.IndustrialChairSleek>(), 2, 3);
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return settings.player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance);
        }

        public override void ModifySittingTargetInfo(int i, int j, ref TileRestingInfo info)
        {
            Tile tile = Framing.GetTileSafely(i, j);

            info.VisualOffset.Y -= 1f;

            info.TargetDirection = -1;
            if (tile.TileFrameX / 18 % 2 == 1)
                info.TargetDirection = 1;

            info.AnchorTilePosition = new(i, j);

            if (tile.TileFrameY == 0)
                info.AnchorTilePosition.Y += 1;
        }

        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;

            if (player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance))
            {
                player.GamepadEnableGrappleCooldown();
                player.sitting.SitDown(player, i, j);
            }

            return true;
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            if (!player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance))
                return;

            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = TileLoader.GetItemDropFromTypeAndStyle(Type, TileObjectData.GetTileStyle(Main.tile[i, j]));

            if (Main.tile[i, j].TileFrameX / 18 % 2 == 0)
                player.cursorItemIconReversed = true;
        }
    }
}