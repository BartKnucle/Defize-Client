using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

namespace FunkySheep.Procedural.Roads
{
  [RequireComponent(typeof(BoxCollider))]
  [RequireComponent(typeof(Rigidbody))]
  public class Node : MonoBehaviour
  {
    public List<Node> nodes = new List<Node>();
    public Material material;
    public Vector2 insideCellPosition;
    public TerrainData terrainData;

    private void Awake() {
      GetComponent<Rigidbody>().useGravity = false;
      GetComponent<Rigidbody>().isKinematic = false;
      GetComponent<Rigidbody>().freezeRotation = true;
      GetComponent<BoxCollider>().isTrigger = true;
    }

    public void Create()
    {
      List<Vector3> centerVertices = new List<Vector3>();
      List<ProBuilderMesh> final = new List<ProBuilderMesh>();
      
      for (int i = 0; i < nodes.Count; i++)
      {
        float roadSize = 2;
        List<Vector3> vertices = new List<Vector3>();

        Vector3 node = this.transform.position;
        Vector3 nextNode = nodes[i].transform.position;
        Vector3 junctionNode = Vector3.Lerp(node, nextNode, 0.25f );
        Vector3 middleNode = (node + nextNode) / 2;
        Vector3 normalNode = Vector3.Cross(nodes[i].transform.position, this.transform.position).normalized * roadSize;
        normalNode.y = 0;

        Vector3 junctionNodeLeftVertice = junctionNode + normalNode;
        vertices.Add(junctionNodeLeftVertice);

        float junctionHeight = nodes[i].terrainData.GetInterpolatedHeight(
          Mathf.Lerp(insideCellPosition.x, nodes[i].insideCellPosition.x, 0.25f),
          Mathf.Lerp(insideCellPosition.y, nodes[i].insideCellPosition.y, 0.25f)
        );
        junctionNode.y = junctionHeight;

        float middleHeight = nodes[i].terrainData.GetInterpolatedHeight(
          (nodes[i].insideCellPosition.x + insideCellPosition.x) / 2,
          (nodes[i].insideCellPosition.y + insideCellPosition.y) /2
        );
        middleNode.y = middleHeight;
                
        Vector3 middleNodeLeftVertice = middleNode + normalNode;
        middleNodeLeftVertice.y = middleHeight;
        vertices.Add(middleNodeLeftVertice);
        Vector3 middleNodeRightVertice = middleNode - normalNode;
        middleNodeRightVertice.y = middleHeight;
        vertices.Add(middleNodeRightVertice);

        Vector3 junctionNodeRightVertice = junctionNode - normalNode;
        vertices.Add(junctionNodeRightVertice);

        GameObject nodeGo = new GameObject();
        ProBuilderMesh mesh = nodeGo.AddComponent<ProBuilderMesh>();
        mesh.CreateShapeFromPolygon(vertices, 0.20f, false);
        mesh.SetMaterial(mesh.faces, material);
        nodeGo.transform.parent = this.transform;

        centerVertices.Add(junctionNodeLeftVertice);
        centerVertices.Add(junctionNodeRightVertice);
        if (nodes.Count == 1)
        {
          centerVertices.Add(node - normalNode);
          centerVertices.Add(node + normalNode);          
        }
        final.Add(mesh);
      }

      GameObject nodeCenterGo = new GameObject();
      nodeCenterGo.name = "center";
      ProBuilderMesh meshCenter = nodeCenterGo.AddComponent<ProBuilderMesh>();
      meshCenter.CreateShapeFromPolygon(centerVertices, 0.20f, false);
      meshCenter.SetMaterial(meshCenter.faces, material);
      nodeCenterGo.transform.parent = this.transform;
     
      final.Add(meshCenter);

      CombineMeshes.Combine(final, meshCenter);
      MeshUtility.CollapseSharedVertices(meshCenter.GetComponent<MeshFilter>().sharedMesh);
      meshCenter.Refresh();
    }

    private void OnTriggerEnter(Collider other) {
      Node otherNode = other.transform.GetComponent<Node>();
      if (otherNode != null)
      {
        GetComponent<MeshRenderer>().material.color = Color.green;
      }
    }
  }
}
