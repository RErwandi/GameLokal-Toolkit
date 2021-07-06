using UnityEngine;

namespace GameLokal.Toolkit
{
    public static class LayerHelper
    {
        public static bool LayerInLayerMask(int layer, LayerMask layerMask)
        {
            return ((1 << layer) & layerMask) != 0;
        }
    }
}