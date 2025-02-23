﻿using Macrocosm.Common.Bases.Tiles;
using Macrocosm.Common.DataStructures;
using Macrocosm.Common.Drawing;
using Macrocosm.Common.Systems.Power;
using Macrocosm.Common.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Macrocosm.Content.Machines
{
    public class WindTurbineLarge : MachineTile
    {
        public override short Width => 3;
        public override short Height => 11;

        public override MachineTE MachineTE => ModContent.GetInstance<WindTurbineLargeTE>();

        private static Asset<Texture2D> bladesTexture;
        private static Asset<Texture2D> turbineTexture;

        private static float rotationCounter;

        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileWaterDeath[Type] = true;
            Main.tileLavaDeath[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
            TileObjectData.newTile.Width = Width;
            TileObjectData.newTile.Height = Height;

            TileObjectData.newTile.CoordinateHeights = [16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16];
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;

            TileObjectData.newTile.DrawYOffset = 2;

            TileObjectData.newTile.Origin = new Point16(1, Height - 1);
            TileObjectData.newTile.AnchorTop = new AnchorData();
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, Width, 0);

            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(MachineTE.Hook_AfterPlacement, -1, 0, false);
            TileObjectData.newTile.UsesCustomCanPlace = true;

            TileObjectData.addTile(Type);

            AddMapEntry(new Color(134, 137, 139), CreateMapEntryName());
            DustType = -1;
        }

        // Called once an update, can use for the rotation animation
        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            if (Math.Abs(Utility.WindSpeedScaled) > 0.1f)
                rotationCounter += (MathHelper.Pi * 0.04f) * Utility.WindSpeedScaled;

            rotationCounter = MathHelper.WrapAngle(rotationCounter);
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];

            if (TileObjectData.IsTopLeft(tile))
                TileRendering.AddCustomSpecialPoint(i, j, CustomSpecialDraw);

            return false; // We must return false here to prevent the normal tile drawing code from drawing the default static tile. Without this a duplicate tile will be drawn.
        }

        private SpriteBatchState state;

        public void CustomSpecialDraw(int i, int j, SpriteBatch spriteBatch)
        {
            TileRendering.DrawMultiTileInWindBottomAnchor(i, j, windSensitivity: 0.02f, rowsToIgnore: 2);

            turbineTexture ??= ModContent.Request<Texture2D>(Texture + "_Turbine");
            bladesTexture ??= ModContent.Request<Texture2D>(Texture + "_Blades");

            Texture2D blades = TileRendering.GetOrPreparePaintedExtraTexture(Main.tile[i, j], bladesTexture);
            Texture2D turbine = TileRendering.GetOrPreparePaintedExtraTexture(Main.tile[i, j], turbineTexture);

            Vector2 zero = Vector2.Zero;
            Vector2 tileDrawPosition = new Vector2(i, j) * 16f + zero - Main.screenPosition;

            Color drawColor = Lighting.GetColor(i, j);

            float windCycle = TileRendering.TileRenderer.GetWindCycle(i, j, TileRendering.SunflowerWindCounter); // Assuming SunflowerWindCounter is a global timer
            float windSwayX = windCycle * 3.3f;

            float turbineScale = 1f;
            Vector2 turbineOffset = new(16 + 8 + windSwayX, 11);
            Matrix turbineMatrix = GetTurbineMatrix(tileDrawPosition + turbineOffset, state.Matrix);
            SpriteEffects turbineEffects = Utility.WindSpeedScaled > 0f ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Vector2 bladeOffset = new(16 + windSwayX + (Utility.WindSpeedScaled < 0f ? (6 + 9.6f * -Utility.WindSpeedScaled) : (6f * (1f - Utility.WindSpeedScaled))), y: 11);
            Matrix bladeMatrix = GetBladesMatrix(tileDrawPosition + bladeOffset, state.Matrix);
            float bladeRotation = rotationCounter - (MathHelper.Pi / 32 * (i / 4));

            state.SaveState(spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, state.DepthStencilState, state.RasterizerState, null, turbineMatrix);

            spriteBatch.Draw(turbine, tileDrawPosition + turbineOffset, null, drawColor, 0f, turbineTexture.Size() / 2f, turbineScale, turbineEffects, 0f);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, state.DepthStencilState, state.RasterizerState, null, bladeMatrix);

            spriteBatch.Draw(blades, tileDrawPosition + bladeOffset, null, drawColor, bladeRotation, bladesTexture.Size() / 2f + new Vector2(-0.11f, 0f), 1f, SpriteEffects.None, 0f);

            spriteBatch.End();
            spriteBatch.Begin(state);
        }


        private static Matrix GetTurbineMatrix(Vector2 drawPosition, Matrix baseMatrix)
        {
            float radians = MathHelper.Clamp(MathHelper.PiOver2 * (1f - Math.Abs(Utility.WindSpeedScaled)), 0f, 1f);

            // Apply the transformations relative to the world position
            Matrix transformationMatrix =
                Matrix.CreateTranslation(-drawPosition.X, -drawPosition.Y, 0f) * // Translate to origin
                Matrix.CreateRotationY(radians) *                                // Apply Y skew
                Matrix.CreateTranslation(drawPosition.X, drawPosition.Y, 0f) *   // Translate back
                Matrix.CreateScale(1f, 1f, 0f);                                  // Keep scale consistent
            return transformationMatrix * baseMatrix;
        }

        private static Matrix GetBladesMatrix(Vector2 drawPosition, Matrix baseMatrix)
        {
            float radians = MathHelper.PiOver2 * Utility.WindSpeedScaled * 0.66f;

            // Apply the transformations relative to the world position
            Matrix transformationMatrix =
                Matrix.CreateTranslation(-drawPosition.X, -drawPosition.Y, 0f) * // Translate to origin
                Matrix.CreateRotationY(radians) *                                // Apply Y skew
                Matrix.CreateTranslation(drawPosition.X, drawPosition.Y, 0f) *   // Translate back
                Matrix.CreateScale(1f, 1f, 0f);                                  // Keep scale consistent
            return transformationMatrix * baseMatrix;
        }

    }
}
