using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.Procedural.Roads
{
    public class Intersection: MonoBehaviour
    {

        private List<Node> nodes = new List<Node>();

        public void AddNode(Node node)
        {
            nodes.Add(node);
            Create();
        }

        public void Delete()
        {

        }

        public void Create()
        {
            Delete();

            foreach (Node node in nodes)
            {
                int indexInSegment = node.segment.nodes.IndexOf(node);

                if (indexInSegment != 0 && node.segment.nodes[indexInSegment - 1].positionned)
                {
                    Debug.DrawLine(this.transform.position, node.segment.nodes[indexInSegment - 1].transform.position, Color.green, 600); 
                }

                if (indexInSegment != node.segment.nodes.Count - 1 && node.segment.nodes[indexInSegment + 1].positionned)
                {
                    Debug.DrawLine(this.transform.position, node.segment.nodes[indexInSegment + 1].transform.position, Color.green, 600); 
                }
            }
        }



    }
}
