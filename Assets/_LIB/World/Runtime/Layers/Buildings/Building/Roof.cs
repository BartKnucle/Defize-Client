using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

namespace FunkySheep.World
{
    [RequireComponent(typeof(ProBuilderMesh))]
    public class Roof : MonoBehaviour
    {
        Building building;
        ProBuilderMesh mesh;
        public float m_Height = 0.1f;
        public bool m_FlipNormals = false;

        /// <summary>
        /// Create the building roof
        /// </summary>
        /// <param name="building"></param>
        public void Create(Building building) {
          mesh = this.GetComponent<ProBuilderMesh>();
          
          Vector3[] points = new Vector3[building.points.Length];

          for (int i = 0, c = building.points.Length; i < c; i++)
          {
            float angle = Mathf.Deg2Rad * ((i / (float)c) * 360f);
            points[i] = new Vector3(building.points[i].x, building.terrainTop + 5f, building.points[i].y);
          }

          // CreateShapeFromPolygon is an extension method that sets the pb_Object mesh data with vertices and faces
          // generated from a polygon path.
          mesh.CreateShapeFromPolygon(points, m_Height, m_FlipNormals);
        }
    }
}
