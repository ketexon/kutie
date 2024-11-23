using System.Collections.Generic;
using UnityEngine;

namespace Kutie
{
    public class ObjectComparer : IComparer<Object>
    {
        public int Compare(Object x, Object y)
        {
            return x.GetInstanceID().CompareTo(y.GetInstanceID());
        }
    }
}
