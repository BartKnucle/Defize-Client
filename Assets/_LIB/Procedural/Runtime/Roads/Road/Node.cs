using System.Linq;
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
    ProBuilderMesh mesh;
    OSM.Node node;
    public bool positionned = false;
    public Material material;
    private void Awake() {
      GetComponent<Rigidbody>().useGravity = false;
      GetComponent<Rigidbody>().isKinematic = false;
      GetComponent<Rigidbody>().freezeRotation = true;
      GetComponent<BoxCollider>().isTrigger = true;
      mesh = GetComponent<ProBuilderMesh>();
    }

    public Segment segment;
    public Intersection intersection = null;
    public void Create(OSM.Node node)
    {
      this.node = node;
      this.name = node.id.ToString();
      SetPosition();
    }

    private void Update() {
      if (!positionned)
      {
        positionned = SetPosition();
      }
    }

    public bool SetPosition()
    {
      Vector2 position = new Vector2(
        (float)FunkySheep.GPS.Utils.lonToX(node.longitude) - FunkySheep.GPS.Manager.Instance.initialMercatorPosition.Value.x,
        (float)FunkySheep.GPS.Utils.latToY(node.latitude) - FunkySheep.GPS.Manager.Instance.initialMercatorPosition.Value.z
      );

      float? height = FunkySheep.Procedural.Earth.SO.GetHeight(position);
      if (height == null)
      {
        height = 0;
        return false;
      }

      this.transform.position = new Vector3(
        position.x,
        height.Value,
        position.y
      );

      CreateSegments();

      return true;
    }

    public void CreateSegments()
    {
      int indexInSegment = segment.nodes.IndexOf(this);
      if (indexInSegment != 0 && segment.nodes[indexInSegment - 1].positionned)
      {
        CreatePreviousSegment(segment.nodes[indexInSegment - 1]);
      }
    }

    public void CreatePreviousSegment(Node previsousNode)
    {
      CreateSegment(previsousNode, this);
    }

    public void CreateSegment(Node from, Node to)
    {
      int indexToNode = segment.nodes.IndexOf(to);

      Vector3 middlePosition = (from.transform.position + to.transform.position) / 2;
      middlePosition.y = (float)FunkySheep.Procedural.Earth.SO.GetHeight(middlePosition);
      Vector3 middleNormal = Normal(from.transform.position, to.transform.position);

      
      Debug.DrawLine(middlePosition, middlePosition + middleNormal, Color.yellow, 600); 
      Debug.DrawLine(transform.position, transform.position + Normal(), Color.blue, 600); 
      Debug.DrawLine(this.transform.position, middlePosition, Color.red, 600); 

      Vector3[] vertices = new Vector3[6];
      vertices[0] = from.transform.position + from.Normal();
      vertices[1] = from.transform.position - from.Normal();
      vertices[2] = middlePosition - middleNormal;
      vertices[3] = to.transform.position - to.Normal();
      vertices[4] = to.transform.position + to.Normal();
      vertices[5] = middlePosition + middleNormal;

      GameObject segmentGo = new GameObject();
      segmentGo.transform.parent = this.transform;
      ProBuilderMesh segmentMesh = segmentGo.AddComponent<ProBuilderMesh>();
      segmentGo.GetComponent<MeshRenderer>().material = material;
      segmentMesh.CreateShapeFromPolygon(vertices.ToList(), 0.5f, false);

      /*List<ProBuilderMesh> final = new List<ProBuilderMesh>();
      final.Add(segmentMesh);
      CombineMeshes.Combine(final, mesh);
      MeshUtility.CollapseSharedVertices(GetComponent<MeshFilter>().sharedMesh);
      mesh.Refresh();
      Destroy(segmentMesh);*/
    }

    public void RemoveSegment()
    {

    }

    public void CreateIntersection()
    {
      GameObject interGo = new GameObject();
      interGo.transform.position = this.transform.position;
      intersection = interGo.AddComponent<Intersection>();
      intersection.AddNode(this);
      interGo.transform.parent = this.transform;
      interGo.name = "inter";
    }

    private void OnTriggerStay(Collider other) {
      if (positionned && intersection == null)
      {
        Node otherNode = other.transform.GetComponent<Node>();
        if (otherNode != null)
        {
          if (otherNode.intersection != null)
          {
            intersection = otherNode.intersection;
            otherNode.intersection.AddNode(this);
          } else {
            CreateIntersection();
          }
          RemoveSegment();
        }
      }
    }

    public Vector3 Normal()
    {
      Vector3 normal = new Vector3();
      int indexToSegment = segment.nodes.IndexOf(this);
      if (indexToSegment == 0)
      {
        normal = Vector3.Cross(this.transform.position, segment.nodes[indexToSegment + 1].transform.position).normalized;

      } else if (indexToSegment == segment.nodes.Count - 1)
      {
        normal = Vector3.Cross(segment.nodes[indexToSegment - 1].transform.position, this.transform.position).normalized;
      } else {
        normal = Vector3.Cross(segment.nodes[indexToSegment - 1].transform.position, segment.nodes[indexToSegment + 1].transform.position).normalized;
      }
      normal *= segment.road.size / 2;
      normal.y = 0;

      return normal;
    }

    public Vector3 Normal(Vector3 from, Vector3 to)
    {
      Vector3 normal = new Vector3();
      normal = Vector3.Cross(from, to).normalized;
      normal *= segment.road.size / 2;
      normal.y = 0;

      return normal;
    }
  }
    /*public List<Node> nodes = new List<Node>();
    public Material material;
    public Vector2 insideCellPosition;
    public TerrainData terrainData;

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
  }*/
}
