using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ImageTracker : MonoBehaviour
{
    public ARTrackedImageManager imageManager;
    public Vector3 postition;
    // Start is called before the first frame update
    void Start()
    {
        imageManager.trackedImagesChanged += _onTrackedChanged;
    }

    private void _onTrackedChanged(ARTrackedImagesChangedEventArgs imgEvent) {
        foreach (ARTrackedImage img in imgEvent.added)
        {
            postition = img.transform.position;
        }
    }
}
