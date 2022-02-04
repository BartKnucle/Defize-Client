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

      return true;
    }

    public List<Vector3> GetVertices()
    {
      List<Vector3> vertices = new List<Vector3>();

      Vector3 nodePlusNormal = transform.position + Normal();
      Vector3 nodeMinusNormal = transform.position - Normal();
      float maxNodeHeight = GetMaxHeight(nodePlusNormal, nodeMinusNormal);
      nodePlusNormal.y = maxNodeHeight;
      nodeMinusNormal.y = maxNodeHeight;
   
      int index = segment.nodes.IndexOf(this);
      if (index == 0)
      {
        vertices.Add(nodePlusNormal);
        vertices.Add(transform.position - Normal());
      } else {
        Vector3 lastMiddle = (segment.nodes[index - 1].transform.position + transform.position) / 2;
        Vector3 lastMiddleNormal = Normal(segment.nodes[index - 1].transform.position, transform.position);
        Vector3 lastMiddlePlusNormal = lastMiddle + lastMiddleNormal;
        Vector3 lastMiddleMinusNormal = lastMiddle - lastMiddleNormal;
        float maxlastMiddleHeight = GetMaxHeight(lastMiddlePlusNormal, lastMiddleMinusNormal);
        lastMiddlePlusNormal.y = maxlastMiddleHeight;
        lastMiddleMinusNormal.y = maxlastMiddleHeight;

        vertices.Add(lastMiddlePlusNormal);
        vertices.Add(nodePlusNormal);
        vertices.Add(nodeMinusNormal);
        vertices.Add(lastMiddleMinusNormal);
      }
      return vertices;
    }

    public float GetMaxHeight(Vector3 first, Vector3 second)
    {
      float firstHeight = (float)FunkySheep.Procedural.Earth.SO.GetHeight(first);
      float secondHeight = (float)FunkySheep.Procedural.Earth.SO.GetHeight(second);

      if (firstHeight > secondHeight)
      {
        return firstHeight;
      } else {
        return secondHeight;
      }
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
        }
      }
    }

    public Vector3 Normal()
    {
      Vector3 normal = new Vector3();
      int indexToSegment = segment.nodes.IndexOf(this);
      if (indexToSegment == 0)
      {
        normal = Vector3.Cross(this.transform.position, segment.nodes[indexToSegment + 1].transform.position);

      } else if (indexToSegment == segment.nodes.Count - 1)
      {
        normal = Vector3.Cross(segment.nodes[indexToSegment - 1].transform.position, this.transform.position);
      } else {
        normal = Vector3.Cross(segment.nodes[indexToSegment - 1].transform.position, segment.nodes[indexToSegment + 1].transform.position);
      }
      normal.y = 0;
      normal = normal.normalized * segment.road.size / 2;

      return normal;
    }

    public Vector3 Normal(Vector3 from, Vector3 to)
    {
      Vector3 normal = new Vector3();
      normal = Vector3.Cross(from, to);
      normal.y = 0;
      normal = normal.normalized * segment.road.size / 2;

      return normal;
    }
  }
}
