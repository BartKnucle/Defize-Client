using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FunkySheep.Habrador;

namespace FunkySheep.World
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class Roof : MonoBehaviour
    {
        Building building;
        Mesh mesh;
        private void Awake() {
            mesh = new Mesh();
        }

        /// <summary>
        /// Create the building roof
        /// </summary>
        /// <param name="building"></param>
        public void Create(Building building) {
            this.building = building;
            MyMesh myMesh = new MyMesh();
            List<MyVector2> vertices = new List<MyVector2>();

            for (int i = 0; i < building.points.Length; i++)
            {
                vertices.Add(new MyVector2(building.points[building.points.Length - i -1].x, building.points[building.points.Length - i -1].y));
            }

            Normalizer2 normalizer2 = new Normalizer2(vertices);

            vertices = normalizer2.Normalize(vertices);
          
            HashSet<Triangle2> triangles = _EarClipping.Triangulate(vertices);

            vertices = normalizer2.UnNormalize(vertices);
            triangles =  normalizer2.UnNormalize(triangles);

            AddRoof(triangles);
            GenerateUvs();

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();

            GetComponent<MeshFilter>().mesh = mesh;
        }

        /// <summary>
        /// Create a roof
        /// </summary>
        /// <param name="index">The list of triangles to create</param>
        public void AddRoof(HashSet<Triangle2> triangles)
        {
            Vector3[] meshVertices = new Vector3[triangles.Count * 3];
            int[] meshTriangles = new int[triangles.Count * 3];

            for (int i = 0; i < triangles.Count; i++)
            {
                meshVertices[i * 3] = new Vector3(triangles.ToArray()[i].p1.x, building.height, triangles.ToArray()[i].p1.y);
                meshVertices[i * 3 + 1] = new Vector3(triangles.ToArray()[i].p2.x, building.height, triangles.ToArray()[i].p2.y);
                meshVertices[i * 3 + 2] = new Vector3(triangles.ToArray()[i].p3.x, building.height, triangles.ToArray()[i].p3.y);

                meshTriangles[i *  3] = i * 3;
                meshTriangles[i * 3 + 1] = i * 3 + 1;
                meshTriangles[i * 3 + 2] = i * 3 + 2;
            }
            
            mesh.vertices = meshVertices;
            mesh.triangles = meshTriangles;
        }

        /// <summary>
        /// Generate the UVs 
        /// </summary>
        public void GenerateUvs()
        {
            Vector2[] Uvs = new Vector2[mesh.vertices.Length];
            for (int i = 0; i < mesh.vertices.Length; i++)
            {
                Uvs[i] = new Vector2(mesh.vertices[i].x, mesh.vertices[i].z);
            }
            mesh.uv = Uvs;
        }
    }
}
