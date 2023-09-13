using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float zoomSpeed = 4f;
    public float minZoom = 4f;
    public float maxZoom = 10f;
    public float dragSpeed = 0.5f;

    private Vector3 dragOrigin;

    public float focusSpeed = 10f;
    public Vector3 focusPoint = Vector3.zero;

    void Update()
    {
        // Zoom in/out using the mouse scroll wheel
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        float zoom = Mathf.Clamp(Camera.main.orthographicSize - scrollWheel * zoomSpeed, minZoom, maxZoom);
        Camera.main.orthographicSize = zoom;

        // Pan the camera by dragging the right mouse button
        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(dragOrigin - Input.mousePosition);
            Vector3 move = new Vector3(pos.x * dragSpeed, pos.y * dragSpeed, 0);
            transform.Translate(move, Space.World);
            dragOrigin = Input.mousePosition;
        }

        // Focus the camera on a specific point (in this case, the origin point Vector3(0,0,0)) by pressing the space key
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject g = GameObject.Find("SelectionSystem").GetComponent<SelectingAgent>().lastUnitSelected;
            transform.position = new Vector3(g.transform.position.x, g.transform.position.y, -10);
        }
    }
}
