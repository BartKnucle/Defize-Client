using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.World
{
    public class Walls : MonoBehaviour
    {
        Building building;
        

      public void Create(Building building)
      {
        this.building = building;

        AddCenter();

        for (int i = 0; i < building.points.Length; i++)
          {
            AddPoint(building.points[i], i);
            AddWall(building.points[i], building.points[(i + 1) % building.points.Length], i, 0.2f, 2.5f);
          }
      }

      void AddCenter()
      {
        GameObject _center = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _center.name = "Center";
        _center.GetComponent<MeshRenderer>().material.color = Color.blue;
        _center.transform.parent = this.transform;
        _center.transform.localPosition = new Vector3(building.center.x, 0, building.center.y);
      }

      void AddPoint(Vector2 position, int index)
      {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = "Point-" + index.ToString();
        go.transform.parent = this.transform;
        go.transform.localPosition = new Vector3(position.x, 0, position.y);
      }

      void AddWall(Vector2 start, Vector2 end, int index, float thickness, float height)
      {
        Vector2 center = ((end + start) / 2);
        float angle = Vector2.SignedAngle(end - start, Vector2.up);

        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = "Wall-" + index.ToString();
        go.transform.parent = this.transform;
        go.GetComponent<MeshRenderer>().material.color = Color.red;
        go.transform.localRotation = Quaternion.Euler(0, angle, 0);
        go.transform.localPosition = new Vector3(center.x, height / 2, center.y); // - new Vector3(thickness, 0, 0);
        go.transform.Translate(new Vector3(thickness / 2, 0, 0));
        go.transform.localScale = new Vector3(thickness, height, Vector2.Distance(start, end));
      }
    }
}