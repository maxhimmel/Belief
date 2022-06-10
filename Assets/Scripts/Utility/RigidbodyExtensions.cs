using System.Collections.Generic;
using UnityEngine;

namespace MaulGrab
{
    public static class RigidbodyExtensions
    {
        public static List<Collider> GetCompositeColliders( this Rigidbody body )
		{
            List<Collider> colliders = new List<Collider>();

            body.GetCompositeColliders( body.transform, ref colliders );

            return colliders;
		}

        private static void GetCompositeColliders( this Rigidbody root, Transform layer, ref List<Collider> colliders )
		{
            if ( layer.TryGetComponent<Collider>( out var piece ) )
            {
                var otherBody = layer != root.transform
                    ? layer.GetComponent<Rigidbody>()
                    : root;

                if ( otherBody != root && otherBody != null )
				{
                    return;
				}

                colliders.Add( piece );
            }

            foreach ( Transform subLayer in layer )
            {
                root.GetCompositeColliders( subLayer, ref colliders );
            }
        }
    }
}
