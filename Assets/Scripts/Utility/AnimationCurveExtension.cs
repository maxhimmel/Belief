using UnityEngine;

namespace MaulGrab.Extensions
{
    public static class AnimationCurveExtension
    {
        public static float GetDuration( this AnimationCurve self )
		{
            int lastKeyIndex = self.length - 1;
            Keyframe lastKey = self.keys[lastKeyIndex];

            return lastKey.time;
		}
    }
}
