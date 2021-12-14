using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.World.Building
{
    [CreateAssetMenu(menuName = "FunkySheep/Building/Data")]
    public class Data : ScriptableObject
    {
       public List<string> cachedTiles;
    }
}