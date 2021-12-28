using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraControls : MonoBehaviour
{
    private Vector3 direction;

    public int cameraDragSpeed;
    public float cameraZoomSpeed;

    public TileMapManager mapManager;

    private CameraMouse cameraMouse;
    private bool moving = false;
    private Camera mainCamera;

    public void Awake()
    {
        cameraMouse = new CameraMouse();
        mainCamera = Camera.main;
    }

    public void OnEnable()
    {
        cameraMouse.Enable();
    }

    public void OnDisable()
    {
        cameraMouse.Disable();
    }

    public void Start()
    {
        cameraMouse.CameraMovement.RightClick.performed += _ => StartMovement();
        cameraMouse.CameraMovement.RightClickReleased.performed += _ => StopMovement();
        cameraMouse.CameraMovement.MiddleClick.performed += _ => ResetPosition();
        cameraMouse.CameraMovement.MouseScroll.performed += _ => Zoom();
    }

    private void LateUpdate()
    {
        Vector3 cameraPos = Camera.main.transform.position;
        Vector2 mousePos = cameraMouse.CameraMovement.Position.ReadValue<Vector2>();

        if (moving && !EventSystem.current.IsPointerOverGameObject())
        {
            float speed = cameraDragSpeed * Time.deltaTime;
            direction = (mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cameraPos.z - mapManager.battleMap.transform.position.z))) - cameraPos;
            mainCamera.transform.position -= new Vector3(direction.x * speed, direction.y * speed, 0);
        }
    }

    private void StartMovement()
    {
        moving = true;
    }

    private void StopMovement()
    {
        moving = false;
    }

    private void ResetPosition()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        MoveCamToCenter();
    }

    public void MoveCamToCenter()
    {
        mainCamera.transform.position = new Vector3(mapManager.battleMap.cellBounds.center.x, mapManager.battleMap.cellBounds.center.y, mainCamera.transform.position.z);
    }

    private void Zoom()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        float scroll = cameraMouse.CameraMovement.MouseScroll.ReadValue<float>();
        if((mainCamera.transform.position.z + scroll * cameraZoomSpeed) < -1)
        {
            mainCamera.transform.position += new Vector3(0, 0, scroll * cameraZoomSpeed);
        }
    }
}
