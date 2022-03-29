using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SHMUP.Util
{
	public static class ColorExt
	{
		public static Color SetRGB(this Color color, float r, float g, float b)
		{
			color.r = r;
			color.g = g;
			color.b = b;
			return color;
		}

		public static Color SetRGB(this Color color, Color rgb)
		{
			return SetRGB(color, rgb.r, rgb.g, rgb.b);
		}

		public static Color SetR(this Color color, float r)
		{
			color.r = r;
			return color;
		}

		public static Color SetG(this Color color, float g)
		{
			color.g = g;
			return color;
		}

		public static Color SetB(this Color color, float b)
		{
			color.b = b;
			return color;
		}

		public static Color SetA(this Color color, float a)
		{
			color.a = a;
			return color;
		}
	}
}