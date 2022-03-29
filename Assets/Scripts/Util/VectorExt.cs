using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SHMUP.Util
{
	public static class VectorExt
	{
		public static Vector2 Clamp(this Vector2 value, Vector2 min, Vector2 max)
		{
			return new Vector2(Mathf.Clamp(value.x, min.x, max.x), Mathf.Clamp(value.y, min.y, max.y));
		}

		public static Vector3 Clamp(this Vector3 value, Vector3 min, Vector3 max)
		{
			return new Vector3(Mathf.Clamp(value.x, min.x, max.x), Mathf.Clamp(value.y, min.y, max.y), Mathf.Clamp(value.z, min.z, max.z));
		}

		public static Vector4 Clamp(this Vector4 value, Vector4 min, Vector4 max)
		{
			return new Vector4(Mathf.Clamp(value.x, min.x, max.x), Mathf.Clamp(value.y, min.y, max.y), Mathf.Clamp(value.z, min.z, max.z), Mathf.Clamp(value.w, min.w, max.w));
		}

		public static Vector2 SetX(this Vector2 vector, float x)
		{
			vector.x = x;
			return vector;
		}

		public static Vector2 SetY(this Vector2 vector, float y)
		{
			vector.y = y;
			return vector;
		}

		public static Vector3 SetX(this Vector3 vector, float x)
		{
			vector.x = x;
			return vector;
		}

		public static Vector3 SetY(this Vector3 vector, float y)
		{
			vector.y = y;
			return vector;
		}

		public static Vector3 SetZ(this Vector3 vector, float z)
		{
			vector.z = z;
			return vector;
		}

		public static Vector4 SetX(this Vector4 vector, float x)
		{
			vector.x = x;
			return vector;
		}

		public static Vector4 SetY(this Vector4 vector, float y)
		{
			vector.y = y;
			return vector;
		}

		public static Vector4 SetZ(this Vector4 vector, float z)
		{
			vector.z = z;
			return vector;
		}

		public static Vector4 SetW(this Vector4 vector, float w)
		{
			vector.w = w;
			return vector;
		}
	}
}