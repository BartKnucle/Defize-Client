using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Building.Walls.Wall
{
    public class Manager : MonoBehaviour
    {
        public Vector3 start;
        public Vector3 end;
        GameObject _door;
        public void AddDoor(GameObject doorPrefab)
        {
            this._door = GameObject.Instantiate(doorPrefab);
            this._door.transform.position = (start + end) * 0.5f;
            this._door.transform.rotation = Quaternion.FromToRotation(Vector3.right, end - start);
            this._door.transform.parent = transform;
            this._door.GetComponent<Game.Building.Door.Manager>().Create(Random.Range(0, 1000));
            
        }

        public void RemoveDoor()
        {
            //Destroy(this._door);
        }
    }    
}
