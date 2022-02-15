using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

namespace FunkySheep.Procedural.Buildings
{
  [RequireComponent(typeof(ProBuilderMesh))]
  [RequireComponent(typeof(MeshCollider))]
  public class Floor : MonoBehaviour
  {
    public Building building;
    public Material material;
    ProBuilderMesh mesh;
    private void Awake() {
      mesh = this.GetComponent<ProBuilderMesh>();
    }

    public void Create()
    {
      Vector3[] newPositions = new Vector3[building.points.Length];

      for (int i = 0; i < newPositions.Length; i++)
      {
        newPositions[i].x = building.points[i].x - building.center.x;
        newPositions[i].y = 0;
        newPositions[i].z = building.points[i].y - building.center.y;
      }
      
      mesh.CreateShapeFromPolygon(newPositions, building.hightPoint.Value - building.lowPoint.Value + 0.20f, false);
      GetComponent<MeshRenderer>().material = material;
      transform.position = new Vector3(building.center.x, building.lowPoint.Value, building.center.y);
    }
  }  
}
