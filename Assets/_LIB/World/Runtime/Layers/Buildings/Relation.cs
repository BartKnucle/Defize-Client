using System;
using System.Collections.Generic;

namespace FunkySheep.World.Buildings
{
    [Serializable]
    public class Relation : Item
    {
        public List<Way> ways = new List<Way>();
        public List<Tag> tags = new List<Tag>();
        public Relation(int id)
        {
            this.id = id;
        }
    }
}