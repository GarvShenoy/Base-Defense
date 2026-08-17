using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 10f;

    [Header("Zoom")]
    [SerializeField] private float zoomSpeed = 50f;
    [SerializeField] private float minZoom = 3f;

    [Header("Bounds")]
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    //Ensures that the methods are called every frame. This allows for smooth key interactions.
    private void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    //Since this method is called every frame, it triggers whenever the key is pressed.
    //In this case, it reads the WASD keyboard inputs to calculate and normalize a directional movement vector for the camera.
    //It then scales the vector by moveSpeed and Time.deltaTime to move the camera's position.
    //Lastly, calls ClampPosition() tO keep the camera within boundaries.  
    private void HandleMovement()
    {
        Vector2 moveInput = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) moveInput.y += 1;
            if (Keyboard.current.sKey.isPressed) moveInput.y -= 1;
            if (Keyboard.current.aKey.isPressed) moveInput.x -= 1;
            if (Keyboard.current.dKey.isPressed) moveInput.x += 1;
        }
        
        moveInput = moveInput.normalized;

        Vector3 move = new Vector3(moveInput.x, moveInput.y, 0f) * moveSpeed * Time.deltaTime;

        transform.position += move;

        ClampPosition();
    }

    //This codes reads the mouse scroll wheel input to adjust the camera's orthographic size based on zoomSpeed.
    //It also calculates dynamic maximum zoom limits using the boundary dimensions and camera aspect ratio to ensure the view does not extend past the boundaries.
    //it then clamps the orthographic size within minZoom and dynamicMaxZoom, then calls ClampPosition() to ensure camera does not go beyond boundaries.
    private void HandleZoom()
{
    if (Mouse.current == null) return;

    float scroll = Mouse.current.scroll.ReadValue().y;

    if (Mathf.Abs(scroll) > 0.01f)
    {
        cam.orthographicSize -= scroll * zoomSpeed * 0.01f;

        float maxVertical = (maxBounds.y - minBounds.y) / 2f;
        float maxHorizontal = (maxBounds.x - minBounds.x) / (2f * cam.aspect);

        float dynamicMaxZoom = Mathf.Min(maxVertical, maxHorizontal);

        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, dynamicMaxZoom);

        ClampPosition();
    }
}
    //This is the clamp method. It calculates the camera's half-height and half-width using its orthographic size and aspect ratio.
    //This then determines the valid minimum and maximum position limits by insetting the world bounds by the camera's extent.
    //Finally, it clamps the camera's current transform position within the boundaries determined.
    private void ClampPosition()
    {
        float camHeight = cam.orthographicSize;
        float camWidth = cam.aspect * camHeight;

        float minX = minBounds.x + camWidth;
        float maxX = maxBounds.x - camWidth;

        float minY = minBounds.y + camHeight;
        float maxY = maxBounds.y - camHeight;

        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;
    }
}