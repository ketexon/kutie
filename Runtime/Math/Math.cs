using System.Collections.Generic;
using UnityEngine;

namespace Kutie
{
    public static partial class KMath {
        public static Vector3 Min(Vector3 v1, Vector3 v2) => new(
            Mathf.Min(v1.x, v2.x),
            Mathf.Min(v1.y, v2.y),
            Mathf.Min(v1.z, v2.z)
        );

        public static Vector3 Max(Vector3 v1, Vector3 v2) => new(
            Mathf.Max(v1.x, v2.x),
            Mathf.Max(v1.y, v2.y),
            Mathf.Max(v1.z, v2.z)
        );

        public static Vector3 RandomUnitVector3() {
            float polar = Random.Range(0, 2 * Mathf.PI);
            float elevation = Random.Range(0, Mathf.PI);
            return new Vector3(
                Mathf.Cos(polar) * Mathf.Sin(elevation),
                Mathf.Sin(polar) * Mathf.Sin(elevation),
                Mathf.Cos(elevation)
            );
        }

        public static Vector2 RandomUnitVector2() {
            float angle = Random.Range(0, 2 * Mathf.PI);
            return new Vector2(
                Mathf.Cos(angle),
                Mathf.Sin(angle)
            );
        }

        public static float Rem(float a, float b) => (a % b + b) % b;
        public static int Rem(int a, int b) => (a % b + b) % b;
        public static float NormalizeAngle360(float angle) => Rem(angle, 360);
        public static float NormalizeAngle180(float angle) => NormalizeAngle360(angle + 180) - 180;

        static public float ClampAngle(float angle, float min, float max)
        {
            float minNormalized = NormalizeAngle180(min - angle);
            float maxNormalized = NormalizeAngle180(max - angle);

            if (minNormalized <= 0 && maxNormalized >= 0)
            {
                return angle;
            }
            if (Mathf.Abs(minNormalized) <= Mathf.Abs(maxNormalized))
                return min;
            return max;
        }

        public static List<Vector2Int> Directions4 = new() {
            Vector2Int.up,
            Vector2Int.left,
            Vector2Int.right,
            Vector2Int.down,
        };
    }
}
