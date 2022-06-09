using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaulGrab.Extensions
{
    public static class LayerMaskExtensions
    {
        public static bool HasFlag( this LayerMask self, LayerMask other )
		{
            return ((self & other) != 0);
		}
    }
}
