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
    public float m_Height = 5f;
    
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

        Vector3 prevPoint = new Vector3(building.points[prevIndex].x, building.terrainTop, building.points[prevIndex].y);
        Vector3 point = new Vector3(building.points[i].x, building.terrainTop, building.points[i].y);
        Vector3 nextPoint = new Vector3(building.points[nextIndex].x, building.terrainTop, building.points[nextIndex].y);
        Vector3 lastPoint = new Vector3(building.points[lastIndex].x, building.terrainTop, building.points[lastIndex].y);

        float pointAngle = Mathf.Sign(Vector3.SignedAngle(nextPoint - point, prevPoint - point, point));
        if (pointAngle == 0)
          pointAngle = -1;
        float nextPointAngle = Mathf.Sign(Vector3.SignedAngle(lastPoint - nextPoint, point - nextPoint, nextPoint));
        if (nextPointAngle == 0)
          nextPointAngle = -1;

        points[0] = point;
        points[1] = nextPoint;
        points[2] = nextPoint + new Vector3(
          (((point - nextPoint).normalized + (lastPoint - nextPoint).normalized).normalized * 0.01f * building.area).x,
          0,
          (((point - nextPoint).normalized + (lastPoint - nextPoint).normalized).normalized * 0.01f * building.area).z) * nextPointAngle;
        points[3] = point + new Vector3(
          (((prevPoint - point).normalized + (nextPoint - point).normalized).normalized * 0.01f * building.area).x,
          0,
          (((prevPoint - point).normalized + (nextPoint - point).normalized).normalized * 0.01f * building.area).z) * pointAngle;

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