﻿using Macrocosm.Content.Subworlds;
using SubworldLibrary;

namespace Macrocosm.Common.Subworlds
{
	public enum MapColorType
	{
		SkyUpper,
		SkyLower,
		UndergroundUpper,
		UndergroundLower,
		CavernUpper,
		CavernLower,
		Underworld
	}

	public partial class MacrocosmSubworld 
	{
		/// <summary> Get the current <c>MacrocosmSubworld</c> active instance. 
		/// Earth returns null! You should check for <see cref="AnyActive"/> before accessing this. </summary>
		public static MacrocosmSubworld Current => SubworldSystem.AnyActive<Macrocosm>() ? SubworldSystem.Current as MacrocosmSubworld : null;

		/// <summary> 
		/// Get the current active Macrocosm subworld string ID, matching the subworld class name. 
		/// Returns <c>Earth</c> if none active. 
		/// Use this for situations where we want other mods subworlds to behave like Earth.
		/// </summary>
		public static string CurrentPlanet => SubworldSystem.AnyActive<Macrocosm>() ? Current.Name : "Earth";

		/// <summary>
		/// Get the current active subworld string ID, matching the subworld class name. 
		/// If it's from another mod, not Macrocosm, returns the subworld name with the mod name prepended. 
		/// Returns <c>Earth</c> if none active.
		/// Use this for situations where other mods' subworlds will behave differently from Earth (the main world).
		/// </summary>
		public static string CurrentWorld { 
			get 
			{
				if (SubworldSystem.AnyActive<Macrocosm>())
					return Current.Name;
				else if (SubworldSystem.AnyActive())
					return SubworldSystem.Current.Mod.Name + "/" + SubworldSystem.Current.Name;
				else
					return "Earth";
			} 
		}

		// TODO: We could protect the original properties get them only via statics?
		public static double CurrentTimeRate => SubworldSystem.AnyActive<Macrocosm>() ? Current.TimeRate : Earth.TimeRate;
		public static double CurrentDayLenght => SubworldSystem.AnyActive<Macrocosm>() ? Current.DayLenght : Earth.DayLenght;
		public static double CurrentNightLenght => SubworldSystem.AnyActive<Macrocosm>() ? Current.NightLenght : Earth.NightLenght;
		public static float CurrentGravityMultiplier => SubworldSystem.AnyActive<Macrocosm>() ? Current.GravityMultiplier : Earth.GravityMultiplier;
	}
}