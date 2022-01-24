using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

namespace FunkySheep.World
{
    [RequireComponent(typeof(ProBuilderMesh))]
    public class Floor : MonoBehaviour
    {
        Building building;
        ProBuilderMesh mesh;
        public float m_Height = 0.2f;
        public bool m_FlipNormals = false;

        /// <summary>
        /// Create the building roof
        /// </summary>
        /// <param name="building"></param>
        public void Create(Building building) {
          m_Height = building.terrainTop - building.terrainBottom;
          mesh = this.GetComponent<ProBuilderMesh>();
          
          Vector3[] points = new Vector3[building.points.Length];

          for (int i = 0, c = building.points.Length; i < c; i++)
          {
            float angle = Mathf.Deg2Rad * ((i / (float)c) * 360f);
            points[i] = new Vector3(building.points[i].x, building.terrainBottom, building.points[i].y);
          }

          // CreateShapeFromPolygon is an extension method that sets the pb_Object mesh data with vertices and faces
          // generated from a polygon path.
          mesh.CreateShapeFromPolygon(points, m_Height, m_FlipNormals);
        }
    }
}
