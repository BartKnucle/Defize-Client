using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

namespace Game.Building.Walls.Wall
{
    public class Manager : MonoBehaviour
    {
      public Walls.Manager walls;
      public int id;
      public Vector3 start;
      public Vector3 startInside;
      public Vector3 end;
      public Vector3 endInside;
      GameObject _door;

    public bool AddDoor(GameObject doorPrefab)
    {
        float wallWidth = Vector3.Distance(start, end);
        this._door = GameObject.Instantiate(doorPrefab);

        if (wallWidth < 2f)
        {
            return false;
        }

        this._door.transform.position = (start + end) * 0.5f;
        this._door.transform.rotation = Quaternion.FromToRotation(Vector3.right, end - start);
        this._door.transform.parent = transform;
        if (this._door.GetComponent<Game.Building.Door.Manager>().Create(Random.Range(0, 1000), wallWidth))
        {
          ProBuilderMesh mesh = GetComponent<ProBuilderMesh>();
          Vector3[] points = new Vector3[4];
          points[0] = start;
          points[1] = end;
          points[2] = endInside;
          points[3] = startInside;

          mesh.CreateShapeFromPolygon(points, 8, false);

          IList<Vector3> door = new List<Vector3> {
            Vector3.Lerp(start, end, 0.25f) ,
            Vector3.Lerp(start, end, 0.75f),
            Vector3.Lerp(endInside, startInside, 0.25f),
            Vector3.Lerp(endInside, startInside, 0.75f)
          };

          ProBuilderMesh mesh1 = ProBuilderMesh.Create();
          mesh1.CreateShapeFromPolygon(door, 10, false); 

          List<ProBuilderMesh> meshes = new List<ProBuilderMesh>();
          meshes.Add(mesh);
          meshes.Add(mesh1);

          CombineMeshes.Combine(meshes, mesh);
          Destroy(mesh1.gameObject);

          return true;
        } else {
          return false;
        }
        
    }

    public void RemoveDoor()
    {
        Destroy(this._door);
    }
  }    
}
