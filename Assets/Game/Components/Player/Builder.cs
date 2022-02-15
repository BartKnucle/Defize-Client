using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

namespace Game.Player
{
    public class Builder : MonoBehaviour
    {
      public GameObject buildingGo;
      public bool DoorAdded = false;
      GameObject _lastWall;
      GameObject _currentWall;
      public GameObject doorPrefab;

      // Update is called once per frame
      void Update()
      {
          int layerMask = 1 << 21;

          // This would cast rays only against colliders in layer 8.
          // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
          //layerMask = ~layerMask;

          RaycastHit hit;
          // Does the ray intersect any objects excluding the player layer
          if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, Mathf.Infinity, layerMask))
          {
              _currentWall = hit.collider.gameObject;
              if (_currentWall != _lastWall)
              {
                  if (_currentWall.GetComponent<Game.Building.Walls.Wall.Manager>().AddDoor(doorPrefab))
                  {
                    _currentWall.GetComponent<MeshRenderer>().enabled = false;
                    DoorAdded = true;
                  } else {
                    DoorAdded = false;
                  }

                  if (_lastWall != null)
                  {
                      _lastWall.GetComponent<Game.Building.Walls.Wall.Manager>().RemoveDoor();
                      _lastWall.GetComponent<MeshRenderer>().enabled = true;
                  }

                  _lastWall = _currentWall;
              }
          }

          if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.touchCount == 2)  && DoorAdded)
          {
            Destroy(_currentWall.GetComponent<ProBuilderMesh>());
            Destroy(_currentWall.GetComponent<MeshRenderer>());
            Destroy(_currentWall.GetComponent<MeshCollider>());
            buildingGo.GetComponent<Game.Building.Manager>().created = true;
            _currentWall = null;
            _lastWall = null;
            DoorAdded = false;
            enabled = false;
          }
      }
  }    
}
