using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

[RequireComponent(typeof(Collider))]
public class Selection : MonoBehaviour
{
  public List<GameObject> selectedObjects;
  private bool _hovered = false;
  private bool _selected = false;

  private Camera mainCamera;
  private float CameraZDistance;

  void Start() {
    mainCamera = Camera.main;
    CameraZDistance =
      mainCamera.WorldToScreenPoint(transform.position).z; //z axis of the game object for screen view
    _unover();
  }

  void Update() {
    if (_hovered) {
      if (Input.GetMouseButtonDown(0))
      {  
        _select();
      }

      if (Input.GetMouseButtonUp(0))
      {
        _unselect();
      }
    }
  }

  void OnMouseOver()
  {
    _over();
  }

  void OnMouseExit()
  {
    _unover();
  }

  void OnMouseDrag() {
    _move();
  }

  private void _over() {
    foreach (GameObject selected in selectedObjects)
    {
        selected.GetComponent<Outline>().enabled = true;
    }
    _hovered = true;
  }

  private void _unover() {
    foreach (GameObject selected in selectedObjects)
    {
        selected.GetComponent<Outline>().enabled = false;
    }
    _hovered = false;
  }

  private void _select() {
    foreach (GameObject selected in selectedObjects)
    {
        selected.GetComponent<Outline>().color = 1;
    }
    _selected = true;
  }

  private void _unselect() {
    foreach (GameObject selected in selectedObjects)
    {
        selected.GetComponent<Outline>().color = 0;
    }
    _selected = false;
  }

  private void _move() {
    Vector3 ScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, CameraZDistance); //z axis added to screen point 
    Vector3 NewWorldPosition = mainCamera.ScreenToWorldPoint(ScreenPosition); //Screen point converted to world point
    transform.position = NewWorldPosition;
}
}

