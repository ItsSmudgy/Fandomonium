using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fandomonium.Content.Pets.SpiritOfSightPet
{
	// You can find a simple pet example in Fandomonium\Content\Pets\ExamplePet
	// This pet uses custom AI and drawing to make it more special (It's a Master Mode boss pet after all)
	// It behaves similarly to the Creeper Egg or Suspicious Grinning Eye pets, but takes some visual properties from Fandomonium's Spirit Of Might
	public class SpiritOfSightPetProjectile : ModProjectile
	{
		public ref float AlphaForVisuals => ref Projectile.ai[0];

		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Spirit Of Sight Pet");

			Main.projFrames[Projectile.type] = 6;
			Main.projPet[Projectile.type] = true;
		}

		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.EyeOfCthulhuPet); // Copy the stats of the Suspicious Grinning Eye projectile
			Projectile.aiStyle = -1; // Use custom AI
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White * AlphaForVisuals * Projectile.Opacity;
		}


		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			// For organization, the AI is split into several methods defined below
			// They are NOT part of the ModProjectile class!
			CheckActive(player);

			bool movesFast = Movement(player);

			Animate(movesFast);

			AlphaForVisuals = GetAlphaForVisuals(player);
		}

		private void CheckActive(Player player)
		{
			// Keep the projectile from disappearing as long as the player isn't dead and has the pet buff
			if (!player.dead && player.HasBuff(ModContent.BuffType<SpiritOfSightPetBuff>()))
			{
				Projectile.timeLeft = 2;
			}
		}

		private bool Movement(Player player)
		{
			// Handles movement, returns true if moving fast (used for animation)
			float velDistanceChange = 2f;

			// Calculates the desired resting position, aswell as some vectors used in velocity/rotation calculations
			int dir = player.direction;
			Projectile.direction = Projectile.spriteDirection = dir;

			Vector2 desiredCenterRelative = new Vector2(dir * 30, -30f);

			// Add some sine motion
			desiredCenterRelative.Y += (float)Math.Sin(Main.GameUpdateCount / 120f * MathHelper.TwoPi) * 5;

			Vector2 desiredCenter = player.MountedCenter + desiredCenterRelative;
			Vector2 betweenDirection = desiredCenter - Projectile.Center;
			float betweenSQ = betweenDirection.LengthSquared(); // It is recommended to operate on squares of distances, to save computing time on square-rooting

			if (betweenSQ > 1000f * 1000f || betweenSQ < velDistanceChange * velDistanceChange)
			{
				// Set position directly if too far away from the player, or when near the desired location
				Projectile.Center = desiredCenter;
				Projectile.velocity = Vector2.Zero;
			}

			if (betweenDirection != Vector2.Zero)
			{
				Projectile.velocity = betweenDirection * 0.1f * 2;
			}

			bool movesFast = Projectile.velocity.LengthSquared() > 6f * 6f;

			if (movesFast)
			{
				// If moving very fast, rotate the projectile towards it smoothly
				float rotationVel = Projectile.velocity.X * 0.08f + Projectile.velocity.Y * Projectile.spriteDirection * 0.02f;
				if (Math.Abs(Projectile.rotation - rotationVel) >= MathHelper.Pi)
				{
					if (rotationVel < Projectile.rotation)
					{
						Projectile.rotation -= MathHelper.TwoPi;
					}
					else
					{
						Projectile.rotation += MathHelper.TwoPi;
					}
				}

				float rotationInertia = 12f;
				Projectile.rotation = (Projectile.rotation * (rotationInertia - 1f) + rotationVel) / rotationInertia;
			}
			else
			{
				// If moving at regular speeds, rotate the projectile towards its default rotation (0) smoothly if necessary
				if (Projectile.rotation > MathHelper.Pi)
				{
					Projectile.rotation -= MathHelper.TwoPi;
				}

				if (Projectile.rotation > -0.005f && Projectile.rotation < 0.005f)
				{
					Projectile.rotation = 0f;
				}
				else
				{
					Projectile.rotation *= 0.96f;
				}
			}

			return movesFast;
		}

		private void Animate(bool movesFast)
		{
			int animationSpeed = 7;

			if (movesFast)
			{
				// Increase animation speed if projectile moves fast (less is faster)
				animationSpeed = 4;
			}

			// Animate all frames from top to bottom, going back to the first
			Projectile.frameCounter++;
			if (Projectile.frameCounter > animationSpeed)
			{
				Projectile.frameCounter = 0;
				Projectile.frame++;

				if (Projectile.frame >= Main.projFrames[Projectile.type])
				{
					Projectile.frame = 0;
				}
			}
		}

		private static float GetAlphaForVisuals(Player player)
		{
			// 0f on full life, 1f for below half life
			float lifeRatio = player.statLife / (float)player.statLifeMax2;
			return Utils.Clamp(2 * (1f - lifeRatio), 0f, 1f);
		}
	}
}
