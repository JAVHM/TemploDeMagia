using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float zoomSpeed = 4f;
    public float minZoom = 4f;
    public float maxZoom = 10f;
    public float dragSpeed = 0.5f;

    private Vector3 dragOrigin;
    private Camera mainCamera;

    public float focusSpeed = 10f;
    public Vector3 focusPoint = Vector3.zero;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        HandleZoom();
        HandlePan();
        HandleFocus();
    }

    private void HandleZoom()
    {
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        float zoom = Mathf.Clamp(mainCamera.orthographicSize - scrollWheel * zoomSpeed, minZoom, maxZoom);
        mainCamera.orthographicSize = zoom;
    }

    private void HandlePan()
    {
        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 pos = mainCamera.ScreenToViewportPoint(dragOrigin - Input.mousePosition);
            Vector3 move = new Vector3(pos.x * dragSpeed, pos.y * dragSpeed, 0);
            transform.Translate(move, Space.World);
            dragOrigin = Input.mousePosition;
        }
    }

    private void HandleFocus()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject selectionSystem = GameObject.Find("SelectionSystem");
            if (selectionSystem != null)
            {
                SelectingAgent selectingAgent = selectionSystem.GetComponent<SelectingAgent>();
                GameObject lastUnitSelected = selectingAgent?.lastUnitSelected;
                if (lastUnitSelected != null)
                {
                    transform.position = new Vector3(lastUnitSelected.transform.position.x, lastUnitSelected.transform.position.y, -10);
                }
            }
        }
    }
}
