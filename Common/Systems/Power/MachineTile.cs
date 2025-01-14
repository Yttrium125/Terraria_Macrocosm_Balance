﻿using Macrocosm.Common.Bases.Tiles;
using Macrocosm.Common.Utils;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Macrocosm.Common.Systems.Power
{
    public abstract class MachineTile : ModTile
    {
        /// <summary> The Tile's width </summary>
        public abstract short Width { get; }

        /// <summary> The Tile's height </summary>
        public abstract short Height { get; }

        /// <summary> The Machine TileEntity template instance associated with this multitile </summary>
        public abstract MachineTE MachineTE { get; }

        /// <summary> 
        /// Used to determine if the machine is powered on, by using the Tile frame.
        /// <br/> Typically, <see cref="MachineTE.PoweredOn"/> is determined by this return value.
        /// </summary>
        public virtual bool IsPoweredOnFrame(int i, int j) => false;

        /// <summary>
        /// Implement here toggling of the tile's frame (what happens when wires or switches on the tile are hit) 
        /// <br/> For initiating the toggle, please call <see cref="Toggle"/>
        /// </summary>
        public virtual void OnToggleStateFrame(int i, int j, bool skipWire = false) { }

        public void Toggle(int i, int j, bool automatic, bool skipWire = false)
        {
            if (Utility.TryGetTileEntityAs(i, j, out MachineTE machineTE))
            {
                machineTE.Toggle(automatic, skipWire);
            }
        }

        public override void HitWire(int i, int j)
        {
            if (Utility.TryGetTileEntityAs(i, j, out BatteryTE _))
                return;

            Toggle(i, j, automatic: false, skipWire: true);
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (Width == 1 && Height == 1 && !effectOnly)
                MachineTE.Kill(i, j);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            if (Width > 1 || Height > 1)
                MachineTE.Kill(i, j);
        }

        // Runs for "block" tiles that function as machines, not multitiles
        public override void PlaceInWorld(int i, int j, Item item)
        {
            if (TileObjectData.GetTileData(Main.tile[i, j]) is null && Width == 1 && Height == 1)
            {
                MachineTE.BlockPlacement(i, j);
            }
        }

        // PlaceInWorld is NOT called on tile swap. 
        // As a temporary fix, tile swap is disabled entirely for machines.
        public override bool CanReplace(int i, int j, int tileTypeBeingPlaced) => false;
    }
}
