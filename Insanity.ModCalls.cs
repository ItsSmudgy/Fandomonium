using Insanity.Common.Systems;
using Insanity.Content;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Insanity
{
	// This is a partial class, meaning some of its parts were split into other files. See ExampleMod.*.cs for other portions.
	partial class Insanity
	{
		// The following code allows other mods to "call" Example Mod data.
		// This allows mod developers to access Example Mod's data without having to set it a reference.
		// Mod calls are not exposed by default, so it will be up to you to publish appropriate calls for your mod, and what values they return.
		public override object Call(params object[] args) {
			// Make sure the call doesn't include anything that could potentially cause exceptions.
			if (args is null) {
				throw new ArgumentNullException(nameof(args), "Arguments cannot be null!");
			}

			if (args.Length == 0) {
				throw new ArgumentException("Arguments cannot be empty!");
			}

			// This check makes sure that the argument is a string using pattern matching.
			// Since we only need one parameter, we'll take only the first item in the array..
			if (args[0] is string content) {
				// ..And treat it as a command type.
				switch (content) {
					case "downedMinionBoss":
						// Returns the value provided by downedMinionBoss, if the argument calls for it.
						return DownedBossSystem.downedMinionBoss;
				}
			}
			return false;
		}
	}
}