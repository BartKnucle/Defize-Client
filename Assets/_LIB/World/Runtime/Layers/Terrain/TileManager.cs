using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.World.Terrain
{
  [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
  [RequireComponent(typeof(UnityEngine.Terrain))]
  public class TileManager : MonoBehaviour
  {
    public LayerSO layerSO;
    public Tile tile;
    
    public UnityEngine.Terrain terrain;
    public UnityEngine.TerrainData terrainData;

    private void Awake() {
      terrain = GetComponent<UnityEngine.Terrain>();
      terrainData = new TerrainData();
      terrain.terrainData = terrainData;
    }
    private void Start() {
      // Set the scale depending on the zoom
      this.transform.localScale = tile.world.worldSO.tileRealSize;

      this.transform.localPosition = new Vector3(
        (-(tile.world.worldSO.initialOffset.x) * tile.world.worldSO.tileRealSize.x) + tile.gridPosition.x * tile.world.worldSO.tileRealSize.x,
          0,
        ((tile.world.worldSO.initialOffset.z - 1) * tile.world.worldSO.tileRealSize.z) + tile.gridPosition.y * tile.world.worldSO.tileRealSize.z
      );

      terrainData.size = tile.world.worldSO.tileRealSize;
      applyHeight();
    }
    
    public void SetuseNormalMap(bool use)
    {
        // Update the mesh to use or disable the normal map.
        layerSO.useNormalMap = use;
        CreateMesh();
    }

    public void SetMeshResolution(float resolution)
    {
        // Update the mesh to the nearest power-of-two resolution.
        int r = (int)Math.Pow(2.0f, Math.Floor(Math.Log(resolution, 2.0f)));
        if (layerSO.resolution != r)
        {
            resolution = r;
            CreateMesh();
        }
    }

    // Generate a uniform grid of vertices, elevate them according to a texture, and apply a normal map.
    public void CreateMesh()
    {
        // Get the mesh for this GameObject.
        var mesh = GetComponent<MeshFilter>().mesh;

        // Generate a vertex grid for the mesh. The offset vector makes the final mesh centered on 0 in X and Z.
        GenerateElevationGrid(mesh, layerSO.resolution, Vector3.zero);

        // Elevate the vertices of the mesh.
        ApplyElevation(mesh);

        // Apply or remove the normal map from our material, based on the option set in the editor.
        var material = GetComponent<MeshRenderer>().material;
        if (layerSO.useNormalMap)
        {
            ApplyNormalTexture(material);
        }
        else
        {
            RemoveNormalTexture(material);
            // When the normal map is not used, use approximate normals calculated at vertices.
            mesh.RecalculateNormals();
        }
    }

    // Generate a uniform 2D grid of vertices for the mesh, with resolution+1 vertices on each side.
    // The vertices span 0.0-1.0 in X and Z by default. The offset vector is added to every vertex position.
    public void GenerateElevationGrid(Mesh mesh, int resolution, Vector3 offset)
    {
        // Create fresh, empty lists for all of the vertex values we need to set.
        var vertices = new List<Vector3>();
        var indices = new List<int>();
        var colors = new List<Color>();
        var uvs = new List<Vector2>();
        var normals = new List<Vector3>();
        var tangents = new List<Vector4>();

        // Iterate over the rows and columns of a grid in X and Z.
        int index = 0;
        for (int col = 0; col <= resolution; col++)
        {
            float y = (float)col / resolution;
            for (int row = 0; row <= resolution; row++)
            {
                float x = (float)row / resolution;

                // Add the values for a new vertex.
                vertices.Add(new Vector3(x, 0, y) + offset);
                colors.Add(Color.white);
                uvs.Add(new Vector2(x, y));

                // We add the 'up' vector as the normal for every vertex so that when we apply our normal map,
                // it will be effectively interpreted as 'object space' normals rather than 'tangent space'.
                normals.Add(Vector3.up);

                // Similar to the normals, we add the 'right' vector as the tangent for every vertex so that
                // our added normal map will be treated like 'object space'. The 4th value determines the sign
                // of cross product used to calculate the binormal, for us it happens to be -1.
                tangents.Add(new Vector4(1, 0, 0, -1));

                // Add indices for form triangles between this vertex and its neighbors left and down, unless
                // we're at the end of a column or row.
                if (row < resolution && col < resolution)
                {
                    indices.Add(index);
                    indices.Add(index + resolution + 1);
                    indices.Add(index + 1);

                    indices.Add(index + 1);
                    indices.Add(index + resolution + 1);
                    indices.Add(index + resolution + 2);
                }

                index++;
            }
        }

        // Clear the previous values from the mesh and set our newly made values.
        mesh.Clear();
        mesh.SetVertices(vertices);
        mesh.SetTriangles(indices, 0);
        mesh.SetColors(colors);
        mesh.SetNormals(normals);
        mesh.SetTangents(tangents);
        mesh.SetUVs(0, uvs);
    }

    public void ApplyElevation(Mesh mesh)
    {
        // Unity terrain heights
        

        // Iterate over the vertex positions and UVs of the mesh.
        var vertices = mesh.vertices;
        var uvs = mesh.uv;
        for (int i = 0; i < vertices.Length; i++)
        {
            // At each vertex, sample the elevation texture at the corresponding UV coordinate.
            int x = Convert.ToInt32(uvs[i].x * tile.heightTexture.width);
            int y = Convert.ToInt32(uvs[i].y * tile.heightTexture.height);
            Color color = tile.heightTexture.GetPixel(x, y);

            // Convert the resulting color value to an elevation in meters.
            float elevation = ColorToElevation(color);

            // Use the tile size in meters at the given zoom level to determine the relative
            // scale of elevation values in the mesh.
            double height = elevation / tile.world.worldSO.tileRealSize.x;
            vertices[i].y = (float)height;
        }
        // Assign the new vertex positions to the mesh.
        mesh.vertices = vertices;
    }

    public void applyHeight()
    {
      float[,] heights = new float[terrainData.heightmapResolution, terrainData.heightmapResolution];
      for (int x = 0; x < terrainData.heightmapResolution; x++)
      {
          for (int y = 0; y < terrainData.heightmapResolution; y++)
          {
            Color color = tile.heightTexture.GetPixel(
              Convert.ToInt32(tile.heightTexture.height / (terrainData.heightmapResolution - 1) * y),
              Convert.ToInt32(tile.heightTexture.width / (terrainData.heightmapResolution -1) * x)
            );

            // Convert the resulting color value to an elevation in meters.
            float elevation = ColorToElevation(color);

            // Use the tile size in meters at the given zoom level to determine the relative
            // scale of elevation values in the mesh.
            double height = elevation / tile.world.worldSO.tileRealSize.x;
            heights[x, y] = (float)height;
          }
      }

      terrainData.SetHeights(0, 0, heights);
    }

    public void ApplyNormalTexture(Material material)
    {
        // https://docs.unity3d.com/Manual/MaterialsAccessingViaScript.html
        material.EnableKeyword("_NORMALMAP");
        material.SetTexture("_BumpMap", tile.normalTexture);
    }

    public void RemoveNormalTexture(Material material)
    {
        material.DisableKeyword("_NORMALMAP");
    }

    public static float ColorToElevation(Color color)
    {
        // Convert from color channel values in 0.0-1.0 range to elevation in meters:
        // https://mapzen.com/documentation/terrain-tiles/formats/#terrarium
        return (color.r * 256.0f * 256.0f + color.g * 256.0f + color.b) - 32768.0f;
    }
  }
}