using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

namespace FunkySheep.World
{
  [RequireComponent(typeof(ProBuilderMesh))]
  public class Walls : MonoBehaviour
  {
    Building building;
    //ProBuilderMesh mesh;
    public float m_Height = 2.5f;
    public bool m_FlipNormals = true;

    public void Create(Building building)
    {
      this.building = building;
      //mesh = this.GetComponent<ProBuilderMesh>();
      
      // External shape

      for (int i = 0, c = building.points.Length; i < c; i++)
      {
        int prevIndex = i - 1;
        if (i == 0)
        {
          prevIndex = building.points.Length - 1;
        }
        int nextIndex = (i + 1) % building.points.Length;
        int lastIndex = (i + 2) % building.points.Length;

        Vector3[] points = new Vector3[4];

        Vector3 prevPoint = new Vector3(building.points[prevIndex].x, 0.1f, building.points[prevIndex].y);
        Vector3 point = new Vector3(building.points[i].x, 0.1f, building.points[i].y);
        Vector3 nextPoint = new Vector3(building.points[nextIndex].x, 0.1f, building.points[nextIndex].y);
        Vector3 lastPoint = new Vector3(building.points[lastIndex].x, 0.1f, building.points[lastIndex].y);

        float pointAngle = Vector3.Angle(nextPoint - point, lastPoint - point);
        float nextPointAngle = Vector3.Angle(lastPoint - nextPoint, point - nextPoint);

        points[0] = point;
        points[1] = nextPoint;
        if (nextPoint.x < 0){
          points[2].x = nextPoint.x + 0.2f;
        } else {
          points[2].x = nextPoint.x - 0.2f;
        }
        if (nextPoint.z < 0){
          points[2].z = nextPoint.z + 0.2f;
        } else {
          points[2].z = nextPoint.z - 0.2f;
        }
        points[2].y = nextPoint.y;

        if (point.x < 0){
          points[3].x = point.x + 0.2f;
        } else {
          points[3].x = point.x - 0.2f;
        }
        if (point.z < 0){
          points[3].z = point.z + 0.2f;
        } else {
          points[3].z = point.z - 0.2f;
        }
        points[3].y = point.y;

        /*points[2] = nextPoint - new Vector3(0.2f, 0, 0.2f);
        points[3] = point - new Vector3(0.2f, 0, 0.2f);*/

        GameObject go = new GameObject();
        go.name = i.ToString();
        go.transform.parent = this.transform;
        go.transform.localPosition = Vector3.zero;
        ProBuilderMesh mesh = go.AddComponent<ProBuilderMesh>();
        mesh.CreateShapeFromPolygon(points, m_Height, m_FlipNormals);
        go.GetComponent<MeshRenderer>().material = this.GetComponent<MeshRenderer>().material;
      }
    }
  }
}