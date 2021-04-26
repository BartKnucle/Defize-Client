using System;
using System.Collections.Generic;
using UnityEngine;
using FunkySheep.Variables;

namespace FunkySheep.Network {
    [CreateAssetMenu(menuName = "FunkySheep/Network/Service")]
    public class Service : ScriptableObject
    {
      public string api;
      public StringVariable _id;
      public List<Request> requests = new List<Request>();
    }
}

