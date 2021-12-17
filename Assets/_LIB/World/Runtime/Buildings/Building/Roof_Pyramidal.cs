using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.World
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class Roof_Pyramidal : MonoBehaviour
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
            // Define the bas mesh size
            // Number of floor point + 1 for the roof center + 1 for the last vertice
            mesh.vertices = new Vector3[building.points.Length + 2];
            // Number of points
            mesh.triangles = new int[building.points.Length * 3];

            this.building = building;
            this.name = building.id;

            //  Set the center of the roof from the center of the building
            Vector2 center = building.Center();
            //  Set it to the last vertice in the array
            Vector3[] vertices = mesh.vertices;
            //  Set the center vertice
            vertices[mesh.vertices.Length - 1] = new Vector3(center.x, building.height + 2.5f, center.y);
            mesh.vertices = vertices;

            for (int i = 0; i < building.points.Length; i++)
            {
                AddRoof(i);
            }
            GenerateUvs();

            GetComponent<MeshFilter>().mesh = mesh;
        }

        /// <summary>
        /// Create a roof
        /// </summary>
        /// <param name="index">The current point index</param>
        public void AddRoof(int index)
        {
            // Get the vertice array since we have to fill vertices all at once
            Vector3[] vertices = mesh.vertices;
            //  The list of the 4 points we gonna use
            Vector3 p0 = new Vector3(building.points[index].x, building.height, building.points[index].y);

            // Fill the vertices anticlockwise
            vertices[index] = p0;

            // Create the triangles
            int[] triangles = mesh.triangles;

            // Calculate the next point index. Use the modulo to go back to 0 at the "Lenght" because we keep the laste vertice to the top of the roof
            int nextPoint = (index + 1) % (mesh.vertices.Length - 2);

            // First one
            triangles[index * 3] = index;
            triangles[index * 3 + 1] = nextPoint;

            // If we are at the last point we add one
            if (index == building.points.Length - 1)
            {
                vertices[index + 1] = new Vector3(building.points[nextPoint].x, building.height, building.points[nextPoint].y);
                triangles[index * 3 + 1] = index + 1;
            }
            triangles[index * 3 + 2] = mesh.vertices.Length - 1;

            mesh.vertices = vertices;
            mesh.triangles = triangles;
        }

        /// <summary>
        /// Generate the UVs 
        /// </summary>
        public void GenerateUvs()
        {
            Vector2[] Uvs = new Vector2[mesh.vertices.Length];
            for (int i = 0; i < mesh.vertices.Length - 1; i++)
            {
                float distance = Vector3.Distance( mesh.vertices[i],  mesh.vertices[i + 1]);
                float uvYRatio = distance / building.area;

                if (i % 2 == 0) {
                    Uvs[i] = new Vector2(0.5f - uvYRatio, 0);
                } else {
                    Uvs[i] = new Vector2(0.5f + uvYRatio, 0);
                }
            }
            // The center point
            Uvs[mesh.vertices.Length - 1] = new Vector2(0.5f, 1);
            mesh.uv = Uvs;
        }
    }
}
