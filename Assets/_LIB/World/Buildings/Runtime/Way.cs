using System;
using System.Collections.Generic;

namespace FunkySheep.World
{
    [Serializable]
    public class Way : Item
    {
        public List<Node> nodes = new List<Node>();
        public List<Tag> tags = new List<Tag>();
        public Way(string id)
        {
            this.id = id;
        }
    }
}