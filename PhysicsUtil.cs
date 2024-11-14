using System.Collections.Generic;
using UnityEngine;

namespace Kutie
{
    public static class PhysicsUtil
    {
        public class RaycastDistanceComparer : IComparer<RaycastHit>
        {
            public int Compare(RaycastHit a, RaycastHit b)
            {
                return a.distance < b.distance ? -1 : 1;
            }
        }

        public static int RaycastNonAllocSorted(
            Vector3 origin,
            Vector3 direction,
            RaycastHit[] results,
            float maxDistance = float.PositiveInfinity,
            int layerMask = Physics.DefaultRaycastLayers,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
            IComparer<RaycastHit> comparer = null
        )
        {
            int nHits = Physics.RaycastNonAlloc(
                origin, direction,
                results,
                maxDistance,
                layerMask,
                queryTriggerInteraction
            );
            System.Array.Sort(
                results,
                0,
                nHits,
                comparer ?? new RaycastDistanceComparer()
            );

            return nHits;
        }

        public static RaycastHit[] RaycastAllSorted(
            Vector3 origin,
            Vector3 direction,
            float maxDistance = float.PositiveInfinity,
            int layerMask = Physics.DefaultRaycastLayers,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
            IComparer<RaycastHit> comparer = null
        )
        {
            var hits = Physics.RaycastAll(
                origin, direction,
                maxDistance,
                layerMask,
                queryTriggerInteraction
            );
            System.Array.Sort(
                hits,
                comparer ?? new RaycastDistanceComparer()
            );

            return hits;
        }

        public static int BoxCastNonAllocSorted(
            Vector3 origin,
            Vector3 halfExtents,
            Vector3 direction,
            RaycastHit[] results,
            Quaternion orientation,
            float maxDistance = float.PositiveInfinity,
            int layerMask = Physics.DefaultRaycastLayers,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
            IComparer<RaycastHit> comparer = null
        )
        {
            var nHits = Physics.BoxCastNonAlloc(
                origin,
                halfExtents,
                direction,
                results,
                orientation,
                maxDistance,
                layerMask,
                queryTriggerInteraction
            );

            System.Array.Sort(
                results,
                0,
                nHits,
                comparer ?? new RaycastDistanceComparer()
            );

            return nHits;
        }

        public static RaycastHit[] BoxCastAllSorted(
            Vector3 origin,
            Vector3 halfExtents,
            Vector3 direction,
            Quaternion orientation,
            float maxDistance = float.PositiveInfinity,
            int layerMask = Physics.DefaultRaycastLayers,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
            IComparer<RaycastHit> comparer = null
        )
        {
            var results = Physics.BoxCastAll(
                origin,
                halfExtents,
                direction,
                orientation,
                maxDistance,
                layerMask,
                queryTriggerInteraction
            );

            System.Array.Sort(
                results,
                comparer ?? new RaycastDistanceComparer()
            );

            return results;
        }
    }
}
