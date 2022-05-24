using UnityEngine;

namespace MaulGrab.Extensions
{
    public static class RandomExtensions
    {
        public static float RandomRange( this Vector2 range )
		{
            return Random.Range( range.x, range.y );
        }
        public static int RandomRange( this Vector2Int range )
        {
            return Random.Range( range.x, range.y );
        }
    }
}
