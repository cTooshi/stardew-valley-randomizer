using System;
using System.Collections.Generic;

namespace Randomizer
{
	public class FishTankBundle : Bundle
	{
		public static List<BundleTypes> RoomBundleTypes { get; set; }

		/// <summary>
		/// Populates the bundle with the name, required items, minimum required, and color
		/// </summary>
		protected override void Populate()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Generates the reward for the bundle
		/// </summary>
		protected override void GenerateReward()
		{
			throw new NotImplementedException();
		}
	}
}
