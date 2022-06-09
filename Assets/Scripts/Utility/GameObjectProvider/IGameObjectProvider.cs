using System.Collections.Generic;
using UnityEngine;

namespace MaulGrab.Gameplay.Utility
{
    public interface IGameObjectProvider
    {
        IEnumerable<GameObject> GetGameObjects();
    }
}
