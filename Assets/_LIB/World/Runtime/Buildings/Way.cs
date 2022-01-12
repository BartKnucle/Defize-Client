using System;
using System.Collections.Generic;

namespace FunkySheep.OldWorld.Buildings
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