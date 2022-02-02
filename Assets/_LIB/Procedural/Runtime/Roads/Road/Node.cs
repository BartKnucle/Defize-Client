using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.Procedural.Roads
{
  [RequireComponent(typeof(BoxCollider))]
  [RequireComponent(typeof(Rigidbody))]
  public class Node : MonoBehaviour
  {
    public List<Node> nodes = new List<Node>();

    private void Awake() {
      GetComponent<Rigidbody>().useGravity = false;
      GetComponent<Rigidbody>().isKinematic = false;
      GetComponent<Rigidbody>().freezeRotation = true;
      GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other) {
      Node otherNode = other.transform.GetComponent<Node>();
      if (otherNode != null)
      {
        GetComponent<MeshRenderer>().material.color = Color.green;
      }
    }

    private void OnCollisionEnter(Collision other) {
      Node otherNode = other.transform.GetComponent<Node>();
      if (otherNode != null)
      {
        GetComponent<MeshRenderer>().material.color = Color.green;
      }
    }
  }
}
