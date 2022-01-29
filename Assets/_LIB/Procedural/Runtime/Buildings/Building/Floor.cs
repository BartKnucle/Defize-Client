using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

namespace FunkySheep.Procedural.Buildings
{
  [RequireComponent(typeof(ProBuilderMesh))]
  [RequireComponent(typeof(MeshCollider))]
  public class Floor : MonoBehaviour
  {
    public Material material;
    ProBuilderMesh mesh;
    private void Awake() {
      mesh = this.GetComponent<ProBuilderMesh>();
    }

    public void Create(Building building)
    {
      Vector3[] newPositions = new Vector3[building.points.Length];

      for (int i = 0; i < newPositions.Length; i++)
      {
        newPositions[i].x = building.points[i].x;
        newPositions[i].y = building.lowPoint.Value;
        newPositions[i].z = building.points[i].y;
      }
      
      mesh.CreateShapeFromPolygon(newPositions, building.hightPoint.Value - building.lowPoint.Value + 0.20f, false);
      mesh.SetMaterial(mesh.faces, material);
    }
  }  
}
