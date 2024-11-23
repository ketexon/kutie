using UnityEngine;

namespace Kutie.Extensions
{
    public static class LayerMaskExtensions
    {
        public static bool Contains(this LayerMask mask, int layer)
        {
            return (mask & 1 << layer) > 0;
        }
    }
}
