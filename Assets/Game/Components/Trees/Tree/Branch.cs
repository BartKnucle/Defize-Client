using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

namespace Game.Trees
{
  [RequireComponent(typeof(ProBuilderMesh))]
  [RequireComponent(typeof(BoxCollider))]
  public class Branch : MonoBehaviour
  {
    ProBuilderMesh mesh;
    Vector3 end;
    bool position = false;
    bool created = false;

    private void Awake() {
      this.mesh = GetComponent<ProBuilderMesh>();
      GetComponent<Collider>().isTrigger = true;
    }

    // Start is called before the first frame update
    void Start()
    {
      this.end = Utils.RandVector3(
        transform.position + Vector3.down / 2 + Vector3.left + Vector3.back,
        transform.position + Vector3.up * 2 + Vector3.right + Vector3.forward
      );

      CreateChilds();
    }

    private void Update() {
      if (!position && this.transform.position != this.end)
      {
        this.transform.position = Vector3.MoveTowards(
          this.transform.position,
          this.end,
          Time.deltaTime * 1
        );
      } else {
        position = true;
      }
    }

    public void CreateChilds()
    {
      int childcount = Random.Range(0, 3);

      for (int i = 0; i < childcount; i++)
      {
        CreateChild();
      }
    }
    
    public void CreateChild()
    {
      GameObject branchGo = new GameObject();
      branchGo.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
      branchGo.transform.position = this.end;
      branchGo.transform.parent = this.transform;
      Branch branch = branchGo.AddComponent<Branch>();
    }

    public void Create(int size = 0)
    {
      Branch parent = transform.parent.GetComponent<Branch>();
      if (parent != null)
      {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
          Transform child = transform.parent.GetChild(i);
          
          if (child.transform.position != transform.position)
          {
            Vector3 center = (transform.position + child.position + transform.parent.position) / 3;
            Debug.DrawLine(transform.position, center, Color.green, 600);
          }
        }
      }

      // Center
      Debug.DrawLine(transform.parent.position, transform.position, Color.red, 600);
    }

    private void OnTriggerEnter(Collider other) {
      Destroy(this.gameObject);
    }
  }
  
}
