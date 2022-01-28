using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

namespace FunkySheep.Procedural.Buildings
{
  [RequireComponent(typeof(ProBuilderMesh))]
  public class Floor : MonoBehaviour
  {
    public Material material;
    ProBuilderMesh mesh;
    private void Awake() {
      mesh = this.GetComponent<ProBuilderMesh>();
    }

    public void Create(Building building)
    {
      Vector3[] newPositions = new Vector3[building.points.Count];
      building.points.CopyTo(newPositions, 0);

      for (int i = 0; i < newPositions.Length; i++)
      {
        newPositions[i].y = building.lowPoint;
      }
      
      mesh.CreateShapeFromPolygon(newPositions, building.hightPoint - building.lowPoint, false);
      mesh.SetMaterial(mesh.faces, material);
    }
  }  
}
