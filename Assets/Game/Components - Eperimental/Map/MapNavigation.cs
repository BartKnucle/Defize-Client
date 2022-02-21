using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MapNavigation : MonoBehaviour
{
    Camera cam;
    int zoom = 14;
    float tileSize;
    //public FunkySheep.OSM.Map2D _map;
    
    private void Start() {
        this.cam = GetComponent<Camera>();
        UpdateCamera();
    }

    void Update()
    {
        // Handle screen touches.
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (Input.touchCount == 1)
            {
                if (touch.phase == TouchPhase.Moved)
                {
                    cam.transform.Rotate(Vector3.up * touch.deltaPosition.y * Time.deltaTime * 10, Space.World);
                }
            }

            if (Input.touchCount == 2)
            {
                // get current touch positions
                Touch tZero = Input.GetTouch(0);
                Touch tOne = Input.GetTouch(1);
                // get touch position from the previous frame
                Vector2 tZeroPrevious = tZero.position - tZero.deltaPosition;
                Vector2 tOnePrevious = tOne.position - tOne.deltaPosition;

                float oldTouchDistance = Vector2.Distance (tZeroPrevious, tOnePrevious);
                float currentTouchDistance = Vector2.Distance (tZero.position, tOne.position);

                // get offset value
                float deltaDistance = oldTouchDistance - currentTouchDistance;

                if (tZero.phase == TouchPhase.Moved || tOne.phase == TouchPhase.Moved)
                {
                    if (zoom == 19 && cam.orthographicSize < tileSize) {
                        cam.orthographicSize = tileSize;
                    } else {
                        cam.orthographicSize += deltaDistance;
                    
                        if (cam.orthographicSize > tileSize * 1.5f ) {
                            //zoom.PreviousZoom();
                            UpdateCamera();
                        } else if (cam.orthographicSize < tileSize / 1.5f)
                        {
                            //_map.NextZoom();
                            UpdateCamera();
                        }
                    }
                }
 
                if (touch.phase == TouchPhase.Began)
                {
                }

                if (touch.phase == TouchPhase.Ended)
                {
                }
            }
        }
    }

    /// <summary>
        /// Update the othographic camera parameters depending on zoom values
        /// </summary>
        public void UpdateCamera() {
            this.cam.orthographicSize = tileSize;
            this.cam.nearClipPlane = -tileSize * 2f;
            this.cam.farClipPlane = tileSize * 2f;
        }
}
