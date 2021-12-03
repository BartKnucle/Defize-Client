using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Walls : MonoBehaviour
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
        // Number of floor point * 2 for the walls
        mesh.vertices = new Vector3[(building.points.Length * 2)];
        // Number of points * (2 for the wall)
        mesh.triangles = new int[building.points.Length * 2 * 3];
        // Number of floor point * 2 for the walls
        mesh.uv = new Vector2[(building.points.Length * 2)];


        this.building = building;
        this.name = building.Id.ToString();

        // Total distance of the walls on the floor (For UVs calculation)
        float wallsLength = 0;

        for (int i = 0; i < building.points.Length; i++)
        {
            AddWall(i);
            wallsLength += Vector2.Distance(building.points[i], building.points[(i + 1) % building.points.Length]);
        }
        GenerateUvs(wallsLength);

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

        mesh.vertices = vertices;

        // Create the triangles
        int[] triangles = mesh.triangles;

        // Calculate the next point index. Use the modulo to go back to 0 at the "Lenght + 1"
        int nextPoint = ((index + 1) * 2) % mesh.vertices.Length;

        // First one
        triangles[index * 6] = index * 2;
        triangles[index * 6 + 1] = nextPoint;
        triangles[index * 6 + 2] = nextPoint + 1;

        // Second one
        triangles[index * 6 + 3] = index * 2;
        triangles[index * 6 + 4] = nextPoint + 1;;
        triangles[index * 6 + 5] = index * 2 + 1;

        mesh.triangles = triangles;
    }

    public void GenerateUvs(float totalWallLength)
    {
         // Create the triangles
        Vector2[] Uvs = new Vector2[mesh.vertices.Length];
        for (int i = 0; i < building.points.Length; i++)
        {
            float uvYRatio = Vector3.Distance(building.points[i], building.points[((i + 1) % building.points.Length)]) / totalWallLength;
            Uvs[i * 2] = new Vector2(0, i * uvYRatio);
            Uvs[i * 2 + 1] = new Vector2(1, i * uvYRatio);
        }

        mesh.uv = Uvs;
    }
}
