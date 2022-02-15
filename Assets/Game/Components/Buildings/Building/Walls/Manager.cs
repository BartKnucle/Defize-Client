using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

namespace Game.Building.Walls
{
    public class Manager : MonoBehaviour
    {
        float thickness = 0.5f;
        float height = 5f;
        public Material material;
        FunkySheep.Procedural.Buildings.Building building;
        public void Create(FunkySheep.Procedural.Buildings.Building building)
        {
            this.building = building;
            for (int i = 0, c = building.points.Length; i < c; i++)
            {
                AddWall(i);
            }
        }

        public void AddWall(int i)
        {
          int prevIndex = i - 1;
          if (i == 0)
          {
          prevIndex = building.points.Length - 1;
          }
          int nextIndex = (i + 1) % building.points.Length;
          int lastIndex = (i + 2) % building.points.Length;

          Vector3[] points = new Vector3[4];

          Vector3 prevPoint = new Vector3(building.points[prevIndex].x - building.center.x, (building.hightPoint.Value- building.lowPoint.Value) + 0.20f, building.points[prevIndex].y - building.center.y);
          Vector3 point = new Vector3(building.points[i].x - building.center.x, (building.hightPoint.Value- building.lowPoint.Value) + 0.20f, building.points[i].y - building.center.y);
          Vector3 nextPoint = new Vector3(building.points[nextIndex].x - building.center.x, (building.hightPoint.Value- building.lowPoint.Value) + 0.20f, building.points[nextIndex].y - building.center.y);
          Vector3 lastPoint = new Vector3(building.points[lastIndex].x - building.center.x, (building.hightPoint.Value- building.lowPoint.Value) + 0.20f, building.points[lastIndex].y - building.center.y);

          float pointAngle = Mathf.Sign(Vector3.SignedAngle(nextPoint - point, prevPoint - point, point));
          if (pointAngle == 0)
          pointAngle = -1;
          float nextPointAngle = Mathf.Sign(Vector3.SignedAngle(lastPoint - nextPoint, point - nextPoint, nextPoint));
          if (nextPointAngle == 0)
          nextPointAngle = -1;

          points[0] = Vector3.zero;

          points[1] = nextPoint - point;
          
          points[2] = nextPoint  - point + new Vector3(
          (((point - nextPoint).normalized + (lastPoint - nextPoint).normalized).normalized * thickness).x,
          0,
          (((point - nextPoint).normalized + (lastPoint - nextPoint).normalized).normalized * thickness).z) * nextPointAngle;

          points[3] = Vector3.zero + new Vector3(
          (((prevPoint - point).normalized + (nextPoint - point).normalized).normalized * thickness).x,
          0,
          (((prevPoint - point).normalized + (nextPoint - point).normalized).normalized * thickness).z) * pointAngle;

          GameObject go = new GameObject();
          go.name = i.ToString();
          go.layer = 21;
          go.transform.parent = this.transform;
          go.transform.localPosition = point;
          ProBuilderMesh mesh = go.AddComponent<ProBuilderMesh>();
          Walls.Wall.Manager wall = go.AddComponent<Walls.Wall.Manager>();
          wall.walls = this;
          wall.id = i;
          wall.start = points[0];
          wall.startInside = points[3];
          wall.end = points[1];
          wall.endInside = points[2];
          wall.height = height;
          go.AddComponent<MeshCollider>();
          mesh.CreateShapeFromPolygon(points, height, false);
          go.GetComponent<MeshRenderer>().material = this.material;
        }
    }    
}
