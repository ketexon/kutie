using UnityEngine;

namespace Kutie
{
    public static class KRandom
    {
        /// <summary>
        /// Returns a random vector between two vectors.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static Vector3 Range(Vector3 min, Vector3 max) => new Vector3(
            (max - min).x * UnityEngine.Random.value,
            (max - min).y * UnityEngine.Random.value,
            (max - min).z * UnityEngine.Random.value
        ) + min;
    }
}
