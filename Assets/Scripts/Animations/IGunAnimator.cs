using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaulGrab.Gameplay.Animation
{
    public interface IGunAnimator
    {
        void OnFiringStart();
        void OnFiringEnd();

        void OnFired();

        void OnReloaded();
    }
}
