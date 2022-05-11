using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaulGrab.Gameplay.Pickups
{
    public interface ICollector<TPickup>
    {
        void Collect( TPickup pickup );
    }
}
