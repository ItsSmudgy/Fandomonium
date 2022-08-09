using Terraria.ModLoader;

namespace Fandomonium.Backgrounds
{
	public class MenuBackgroundStyle : ModSurfaceBackgroundStyle
	{
		// Use this to keep far Backgrounds like the mountains.
		public override void ModifyFarFades(float[] fades, float transitionSpeed) {
			for (int i = 0; i < fades.Length; i++) {
				if (i == Slot) {
					fades[i] += transitionSpeed;
					if (fades[i] > 1f) {
						fades[i] = 1f;
					}
				}
				else {
					fades[i] -= transitionSpeed;
					if (fades[i] < 0f) {
						fades[i] = 0f;
					}
				}
			}
		}

		public override int ChooseFarTexture() {
			return BackgroundTextureLoader.GetBackgroundSlot("Terraria/Images/MagicPixel");
		}

		private static int SurfaceFrameCounter;
		private static int SurfaceFrame;
		public override int ChooseMiddleTexture() {
			if (++SurfaceFrameCounter > 12) {
				SurfaceFrame = (SurfaceFrame + 1) % 4;
				SurfaceFrameCounter = 0;
			}
			switch (SurfaceFrame) {
				case 0:
					return BackgroundTextureLoader.GetBackgroundSlot("Terraria/Images/MagicPixel");
				case 1:
					return BackgroundTextureLoader.GetBackgroundSlot("Terraria/Images/MagicPixel");
				case 2:
					return BackgroundTextureLoader.GetBackgroundSlot("Terraria/Images/MagicPixel");
				case 3:
					return BackgroundTextureLoader.GetBackgroundSlot("Terraria/Images/MagicPixel"); // You can use the full path version of GetBackgroundSlot too
				default:
					return -1;
			}
		}

		public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b) {
			return BackgroundTextureLoader.GetBackgroundSlot("Terraria/Images/MagicPixel");
		}
	}
}