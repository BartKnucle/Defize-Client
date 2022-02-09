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

        public bool AddDoor(GameObject doorPrefab)
        {
            float wallWidth = Vector3.Distance(start, end);
            this._door = GameObject.Instantiate(doorPrefab);

            if (wallWidth < 2f)
            {
                return false;
            }

            this._door.transform.position = (start + end) * 0.5f;
            this._door.transform.rotation = Quaternion.FromToRotation(Vector3.right, end - start);
            this._door.transform.parent = transform;
            return this._door.GetComponent<Game.Building.Door.Manager>().Create(Random.Range(0, 1000), wallWidth);
            
        }

        public void RemoveDoor()
        {
            Destroy(this._door);
        }
    }    
}
