using System;
using UnityEngine;
namespace FunkySheep.World
{
    [Serializable]
    public class Node : Item
    {
        public double latitude;
        public double longitude;
        public Vector2 position;
       public Node(string id)
       {
           this.id = id;
       }
    }
}