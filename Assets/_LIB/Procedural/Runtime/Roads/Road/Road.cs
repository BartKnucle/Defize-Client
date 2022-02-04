using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FunkySheep.OSM;

namespace FunkySheep.Procedural.Roads
{
    public class Road : MonoBehaviour
    {
        public Way way;
        public List<Segment> segments = new List<Segment>();
        public List<Intersection> intersections = new List<Intersection>();
        public float size = 2;

        public void Create()
        {
            this.name = way.id.ToString();
            Segment segment = new Segment(this, 0);
            segments.Add(segment);
            foreach (OSM.Node osmNode in way.nodes)
            {
                GameObject nodeGO = new GameObject();
                nodeGO.transform.parent = this.transform;
                Node node = nodeGO.AddComponent<Node>();
                node.segment = segment;
                segment.nodes.Add(node);

                node.Create(osmNode);
            }
        }
    }
}
