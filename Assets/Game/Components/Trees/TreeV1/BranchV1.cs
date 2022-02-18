using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

namespace Game.Trees
{
  public class BranchV1 : MonoBehaviour
  {
    public TreeV1 tree;
    public int generation = 0;

    public List<Vector3> vertices = new List<Vector3>();

    private void Start() {
      this.Create();
    }

    private void FixedUpdate() {
      if (generation != tree.generations)
      {
        Grow();
      }

      // Debug
      for (int g = 0; g <= generation; g++)
      {
        for (int v = 0; v < tree.resolution; v++)
        {
          int verticeIndex = (g * tree.resolution) + v;
          int nextVerticeIndex = (g * tree.resolution) + (v + 1) % tree.resolution;
          Debug.DrawLine(vertices[verticeIndex], vertices[nextVerticeIndex], Color.green);
          if (g != 0)
          {
            int downVerticeIndex = ((g - 1) * tree.resolution) + v;
            Debug.DrawLine(vertices[verticeIndex], vertices[downVerticeIndex], Color.green);
          }
        } 
      }
    }

    public void Create()
    {
      AddBranch();
    }

    public void Grow()
    {
      for (int i = 0; i < tree.resolution; i++)
      {
        float radius = tree.radius * (1 - ((float)generation / (float)tree.generations));
        float angle = i * Mathf.PI * 2f / tree.resolution;
        Vector3 endPos = new Vector3(Mathf.Cos(angle) * radius, generation, Mathf.Sin(angle) * radius);
        
        int verticeIndex = (generation * tree.resolution) + i;
        int nextVerticeIndex = (generation * tree.resolution) + (i + 1) % tree.resolution;
        
        Vector3 nextPos = Vector3.MoveTowards(
          vertices[verticeIndex],
          endPos,
          Time.deltaTime * 0.5f
        );

        // Detect horizontal collisions
        if (DectectCollision(vertices[verticeIndex], vertices[nextVerticeIndex]))
        {
          generation += 1;
          AddBranch();
          break;
        }

        // Vertical collisions detection
        if (generation != 0)
        {
          int downVerticeIndex = ((generation - 1) * tree.resolution) + i;
          if (DectectCollision(vertices[verticeIndex], vertices[downVerticeIndex]))
          {
            generation += 1;
            AddBranch();
            break;
          }
        }
        
        if(vertices[verticeIndex] == endPos)
        {
          generation += 1;
          AddBranch();
          break;
        }

        vertices[verticeIndex] = nextPos;
      }
    }

    public bool DectectCollision(Vector3 start, Vector3 end)
    {
      if (Physics.Linecast(start, end))
      {
        Debug.DrawLine(start, end, Color.red);
        return true;
      }
      else
      {
        return false;
      }
    }

    public void AddBranch()
    {
      Vector3? nextNode = transform.position;
      if (generation != 0)
      {
        nextNode = GetNextHeight().Value;
      }

      if (nextNode != null)
      {
        for (int i = 0; i < tree.resolution; i++)
        {
          vertices.Add(nextNode.Value);
        }
      }
    }

    public Vector3? GetNextHeight()
    {
      for (int x = 0; x < 10; x++)
      {
        Vector3 nextHeight = RandomNext((generation - 1) * Vector3.up);
        Color color = Random.ColorHSV();
        bool valid = true;
        for (int i = 0; i < tree.resolution; i++)
        {
          float radius = tree.radius * (1 - ((float)generation / (float)tree.generations));
          float angle = i * Mathf.PI * 2f / tree.resolution;
          Vector3 nextPos = new Vector3(Mathf.Cos(angle) * radius, generation, Mathf.Sin(angle) * radius);
          nextPos += nextHeight;
          
          int lastVerticeIndex = ((generation - 1) * tree.resolution) + i;
          // Detect horizontal collisions
          Debug.DrawLine(vertices[lastVerticeIndex], nextPos, color, 600);
          valid &= !DectectCollision(vertices[lastVerticeIndex], nextPos);
        }

        if (valid)
        {
          return nextHeight;
        }
      }

      return null;
    }

    public Vector3 RandomNext(Vector3 start)
    {
      return Utils.RandVector3(
        start + Vector3.left + Vector3.forward,
        start + Vector3.right + Vector3.back
      );
    }

  }
}
