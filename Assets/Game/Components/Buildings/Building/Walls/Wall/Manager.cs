using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

namespace Game.Building.Walls.Wall
{
    public class Manager : MonoBehaviour
    {
      public Walls.Manager walls;
      public int id;
      public Vector3 start;
      public Vector3 startInside;
      public Vector3 end;
      public Vector3 endInside;
      public float height;
      GameObject _door;

    public bool AddDoor(GameObject doorPrefab)
    {
      this._door = GameObject.Instantiate(doorPrefab, this.transform, false);
      this._door.transform.localPosition = (end - start) / 2;
      //this._door.transform.position = (start + end) * 0.5f;
      this._door.transform.rotation = Quaternion.FromToRotation(Vector3.right, end - start);
      //this._door.transform.parent = transform;
      return this._door.GetComponent<Game.Building.Door.Manager>().Create(this, Random.Range(0, 1000));
    }

    public void RemoveDoor()
    {
      Destroy(this._door);
    }
  }    
}
