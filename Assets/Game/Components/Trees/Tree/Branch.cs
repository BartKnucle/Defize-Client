using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

namespace Game.Trees
{
  [RequireComponent(typeof(BoxCollider))]
  public class Branch : MonoBehaviour
  {
    public Tree tree;
    public int generation = 0;
    Vector3 start;
    Vector3 end;
    bool created = false;

    private void Awake() {
      GetComponent<Collider>().isTrigger = true;
    }

    // Start is called before the first frame update
    void Start()
    {
      start = this.transform.position;
      if (!FindEnd())
      {
        Destroy(this.gameObject);
      }
    }

    private void Update() {
      if (this.transform.position != this.end)
      {
        this.transform.position = Vector3.MoveTowards(
          this.transform.position,
          this.end,
          Time.deltaTime * 1
        );
      } else {
        if (!created)
        {
          Create();
        }
      }
    }

    public void CreateChilds()
    {
      int childcount = Random.Range(2, 6);

      for (int i = 0; i < childcount; i++)
      {
        CreateChild();
      }
    }
    
    public void CreateChild()
    {
      GameObject branchGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
      branchGo.transform.localScale = Vector3.one * 0.1f;
      branchGo.transform.position = this.end;
      branchGo.transform.parent = this.transform;
      Branch branch = branchGo.AddComponent<Branch>();
      branch.tree = this.tree;
      branch.generation = generation + 1;
    }

    public bool FindEnd()
    {
      for (int i = 0; i < 10; i++)
      {
        this.end = GetEnd();
        if (CheckEnd(this.end))
        {
          return true;
        }
      }

      return false;
    }

    public Vector3 GetEnd()
    {
      return Utils.RandVector3(
        start + Vector3.left + Vector3.back,
        start + Vector3.up * 2 + Vector3.right + Vector3.forward
      );
    }

    public bool CheckEnd(Vector3 end)
    {
      RaycastHit hit;
      // Does the ray intersect any objects excluding the player layer
      if (Physics.Raycast(start, end, out hit, Vector3.Distance(start, end) + 0.5f))
      {
        return false;
      }
      else
      {
        return true;
      }
    }

    public void Create()
    {
      // Center
      Debug.DrawLine(transform.parent.position, transform.position, Color.red, 600);

      if (generation != tree.generations)
      {
        CreateChilds();
      }
      created = true;
    }

    private void OnTriggerEnter(Collider other) {
      Destroy(this.gameObject);
    }
  }
  
}
