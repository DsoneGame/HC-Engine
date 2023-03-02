using UnityEngine;
using Object = UnityEngine.Object;

namespace Engine.Linq
{
    public static class EngineExtension
    {
        public static bool IsNull(this Object obj)
        {
            return obj == null || obj.Equals(null);
        }

        public static bool IsInLayerMask(this int layer, LayerMask layerMask)
        {
            return ((layerMask.value & (1 << layer)) > 0);
        }
    }
}