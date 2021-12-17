using System;
using System.Collections.Generic;

namespace FunkySheep.World
{
    [Serializable]
    public class Way : Item
    {
        public List<Point> points = new List<Point>();
        public List<Tag> tags = new List<Tag>();
        public Way(int id)
        {
            this.id = id;
        }
    }
}