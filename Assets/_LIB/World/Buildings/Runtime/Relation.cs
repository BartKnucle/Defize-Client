using System;
using System.Collections.Generic;

namespace FunkySheep.World
{
    [Serializable]
    public class Relation : Item
    {
        public List<Item> members;
        public Relation(string id)
        {
            this.id = id;
        }
    }
}