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
    //bool alive = true;
    bool positionned = false;
    bool created = false;
    int childCount = 0;

    List<Vector3> nodes = new List<Vector3>();

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
        if (!positionned)
        {
          positionned = true;
          Create();
        }
      }
    }

    public void CreateChilds()
    {
      childCount = Random.Range(2, 4);

      for (int i = 0; i < childCount; i++)
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
      Vector3 hitEndPosition = Vector3.SlerpUnclamped(start, end, 1.1f);
      // Does the ray intersect any objects excluding the player layer
      if (Physics.Linecast(start, hitEndPosition))
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
      if (generation == tree.generations) //Leaf
      {
        GetComponent<MeshRenderer>().material.color = Color.green;
        Debug.DrawLine(transform.parent.position, transform.position, Color.green, 600);
      } else {
        GetComponent<MeshRenderer>().material.color = Color.red;
        Debug.DrawLine(transform.parent.position, transform.position, Color.red, 600);
        CreateChilds();
      }

      CreateMesh();
    }

    public bool IsReady()
    {
      bool ready = true;    
      for (int i = 0; i < transform.childCount; i++)
      {
        ready &= transform.GetChild(i).GetComponent<Branch>().positionned;
      }
      return ready;
    }

    public void CreateMesh()
    {
      float radius = tree.radius  * (tree.generations - generation) / tree.generations;
      int nodeCount = tree.resolution * childCount;

      for (int i = 0; i < nodeCount; i++)
      {
        float angle = i * Mathf.PI * 2f / nodeCount;
        Vector3 newPos = new Vector3(Mathf.Cos(angle)*radius, 0, Mathf.Sin(angle)*radius);
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.position = start + newPos;
        go.name = i.ToString();
        go.transform.localScale = Vector3.one * 0.05f;
        go.transform.parent = this.transform;
      }
    }
  }
  
}
