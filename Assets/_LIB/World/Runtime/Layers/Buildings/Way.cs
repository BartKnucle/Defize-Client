using System;
using System.Collections.Generic;

namespace FunkySheep.World.Buildings
{
    [Serializable]
    public class Way : Item
    {
        public List<Point> points = new List<Point>();
        public List<Tag> tags = new List<Tag>();
        public Tile tile;
        public Way(int id, Tile tile)
        {
          this.id = id;
          this.tile = tile;
        }
    }
}