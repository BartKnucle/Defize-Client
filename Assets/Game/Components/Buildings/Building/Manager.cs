using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FunkySheep.Procedural.Buildings;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

namespace Game.Building
{
    [RequireComponent(typeof(Floor))]
    public class Manager : MonoBehaviour
    {
        FunkySheep.Procedural.Buildings.Building building ;
        Floor floor;
        public Material material;
        // Start is called before the first frame update
        void Start()
        {
            this.floor = GetComponent<Floor>();
            this.floor.material = material;
            this.building = floor.building;
            this.floor.Create();
        }

        private void OnCollisionEnter(Collision other) {
            GetComponent<MeshRenderer>().material.color = Color.green;
            CreateWalls();
        }

        private void OnCollisionExit(Collision other) {
            GetComponent<MeshRenderer>().material.color = Color.white;
        }

        public void CreateWalls()
        {
            float wallThickness = 1f;
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

                Vector3 prevPoint = new Vector3(building.points[prevIndex].x, building.hightPoint.Value + 0.20f, building.points[prevIndex].y);
                Vector3 point = new Vector3(building.points[i].x, building.hightPoint.Value + 0.20f, building.points[i].y);
                Vector3 nextPoint = new Vector3(building.points[nextIndex].x, building.hightPoint.Value + 0.20f, building.points[nextIndex].y);
                Vector3 lastPoint = new Vector3(building.points[lastIndex].x, building.hightPoint.Value + 0.20f, building.points[lastIndex].y);

                float pointAngle = Mathf.Sign(Vector3.SignedAngle(nextPoint - point, prevPoint - point, point));
                if (pointAngle == 0)
                pointAngle = -1;
                float nextPointAngle = Mathf.Sign(Vector3.SignedAngle(lastPoint - nextPoint, point - nextPoint, nextPoint));
                if (nextPointAngle == 0)
                nextPointAngle = -1;

                points[0] = point;

                points[1] = nextPoint;
                
                points[2] = nextPoint + new Vector3(
                (((point - nextPoint).normalized + (lastPoint - nextPoint).normalized).normalized * wallThickness).x,
                0,
                (((point - nextPoint).normalized + (lastPoint - nextPoint).normalized).normalized * wallThickness).z) * nextPointAngle;

                points[3] = point + new Vector3(
                (((prevPoint - point).normalized + (nextPoint - point).normalized).normalized * wallThickness).x,
                0,
                (((prevPoint - point).normalized + (nextPoint - point).normalized).normalized * wallThickness).z) * pointAngle;

                GameObject go = new GameObject();
                go.name = i.ToString();
                go.transform.parent = this.transform;
                go.transform.localPosition = Vector3.zero;
                ProBuilderMesh mesh = go.AddComponent<ProBuilderMesh>();
                mesh.CreateShapeFromPolygon(points, 5, false);
                go.GetComponent<MeshRenderer>().material = this.GetComponent<MeshRenderer>().material;
            }

        }
    }    
}
