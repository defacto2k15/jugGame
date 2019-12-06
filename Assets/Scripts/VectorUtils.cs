
using UnityEngine;

namespace Assets.Scripts
{
    public static class VectorUtils
    {
        public static Vector2 XZComponent(this Vector3 vec)
        {
            return new Vector2(vec.x, vec.z);
        }

        public static Vector2 XYComponent(this Vector3 vec)
        {
            return new Vector2(vec.x, vec.y);
        }

        public static float Project(Vector2 vector, Vector2 onNormal)
        {
            return Vector2.Dot(vector, onNormal) / onNormal.magnitude;
        }
    }
}
