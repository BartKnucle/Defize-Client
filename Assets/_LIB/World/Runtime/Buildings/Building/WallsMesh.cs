using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.OldWorld
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class WallsMesh : MonoBehaviour
    {
        Building building;
        Mesh mesh;
        private void Awake() {
            mesh = new Mesh();
        }

        /// <summary>
        /// Create the building
        /// </summary>
        /// <param name="building">The building Scriptable object</param>
        public void Create(Building building) {
            // Define the bas mesh size
            // Number of floor point * 2 for the walls + 2 For the last loop vertices
            mesh.vertices = new Vector3[(building.points.Length * 2) + 2];
            // Number of points * (2 for the wall)
            mesh.triangles = new int[building.points.Length * 2 * 3];
            // Number of floor point * 2 for the walls + 2 for the last loop vertices
            mesh.uv = new Vector2[(building.points.Length * 2) + 2];


            this.building = building;
            this.name = building.id;

            for (int i = 0; i < building.points.Length; i++)
            {
                AddWall(i);
            }

            GenerateUvs();

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();

            GetComponent<MeshFilter>().mesh = mesh;
        }
    
        /// <summary>
        /// Create a wall
        /// </summary>
        /// <param name="index">The index of the wall</param>
        /// <returns>The length of the wall</returns>
        public void AddWall(int index) {
            // Get the vertice array since we have to fill vertices all at once
            Vector3[] vertices = mesh.vertices;
            //  The list of the 4 points we gonna use
            Vector3 p0 = new Vector3(building.points[index].x, 0, building.points[index].y);
            Vector3 p1 = new Vector3(building.points[index].x, building.height, building.points[index].y);

            // Fill the vertices anticlockwise
            vertices[index * 2] = p0;
            vertices[index * 2 + 1] = p1;

            //  We add 2 last vertices at the first point to end the loop
            if (index == building.points.Length - 1)
            {
                vertices[(index + 1) * 2] = new Vector3(building.points[0].x, 0, building.points[0].y);
                vertices[(index + 1) * 2 + 1] = new Vector3(building.points[0].x, building.height, building.points[0].y);
            }

            mesh.vertices = vertices;

            // Create the triangles
            int[] triangles = mesh.triangles;

            // Calculate the next point index.
            int nextPoint = (index + 1) * 2;

            // First one
            triangles[index * 6] = index * 2;
            triangles[index * 6 + 1] = nextPoint;
            triangles[index * 6 + 2] = nextPoint + 1;

            // Second one
            triangles[index * 6 + 3] = index * 2;
            triangles[index * 6 + 4] = nextPoint + 1;
            triangles[index * 6 + 5] = index * 2 + 1;

            mesh.triangles = triangles;
        }

        public void GenerateUvs()
        {
            Vector2[] Uvs = new Vector2[mesh.vertices.Length];
            float currentUvPos = 0;
            for (int i = 0; i < building.points.Length; i++)
            {
                float distance = Vector3.Distance(building.points[i], building.points[((i + 1) % building.points.Length)]);
                float uvYRatio = distance / building.area;
                
                Uvs[i * 2] = new Vector2(currentUvPos, 0);
                Uvs[i * 2 + 1] = new Vector2(currentUvPos, 1);
                currentUvPos += uvYRatio;

                // For the last loop point
                if (i == building.points.Length - 1) {
                    Uvs[(i + 1) * 2] = new Vector2(1, 0);
                    Uvs[(i + 1) * 2 + 1] = new Vector2(1, 1);
                }
            }
            mesh.uv = Uvs;
        }
    }
}