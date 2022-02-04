using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FunkySheep.OSM;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

namespace FunkySheep.Procedural.Roads
{
    [RequireComponent(typeof(ProBuilderMesh))]
    public class Road : MonoBehaviour
    {
        public Way way;
        Segment segment;
        public List<Intersection> intersections = new List<Intersection>();

        public Material material;
        public float size = 4;
        ProBuilderMesh mesh;

        private void Awake() {
            mesh = GetComponent<ProBuilderMesh>();
        }

        public void Create()
        {
            GetComponent<MeshRenderer>().material = material;
            this.name = way.id.ToString();
            segment = new Segment(this);
            foreach (OSM.Node osmNode in way.nodes)
            {
                GameObject nodeGO = new GameObject();
                nodeGO.transform.parent = this.transform;
                Node node = nodeGO.AddComponent<Node>();
                node.material = material;
                node.segment = segment;
                segment.nodes.Add(node);

                node.Create(osmNode);
            }

            Build();
        }

        public void Build()
        {
            List<Vector3> vertices = new List<Vector3>();
            foreach (Node node in segment.nodes)
            {
                List<Vector3> newVertices = node.GetVertices();
                vertices.InsertRange((int)vertices.Count / 2, newVertices);
            }
            mesh.CreateShapeFromPolygon(vertices, 0.2f, false);
        }
    }
}
