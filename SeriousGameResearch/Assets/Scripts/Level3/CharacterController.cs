using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Number of grid cells the player moves upward per second")]
    public float ForwardSpeed = 2f;    // cells per second
    [Tooltip("Number of grid cells per second the player shifts left/right")]
    public float HorizontalSpeed = 2f; // cells per second
    [Tooltip("World size of 1 grid cell")]
    public float cellSize = 1f;

    [Header("Boundaries")]
    public float MinX = -5f;
    public float MaxX = 5f;

    public bool CanMove { get; set; } = false;

    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if (!CanMove) {
            return;
        }

        // Convert speed from cells/sec to world units/sec
        float forwardMove = ForwardSpeed * cellSize;
        float horizontalMove = HorizontalSpeed * cellSize;

        // Always move forward (up)
        Vector2 moveDir = Vector2.up * forwardMove;

        // Get mouse world position
        Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        // Check mouse X vs character X
        if (mouseWorldPos.x < transform.position.x - 0.1f)
        {
            moveDir += Vector2.left * horizontalMove;
        }
        else if (mouseWorldPos.x > transform.position.x + 0.1f)
        {
            moveDir += Vector2.right * horizontalMove;
        }

        // Apply movement
        transform.Translate(moveDir * Time.deltaTime);

        // Clamp X position
        float clampedX = Mathf.Clamp(transform.position.x, MinX, MaxX);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        float minY = transform.position.y - 10f;
        float maxY = transform.position.y + 50f;

        // Left boundary
        Gizmos.DrawLine(new Vector3(MinX, minY, 0f), new Vector3(MinX, maxY, 0f));
        // Right boundary
        Gizmos.DrawLine(new Vector3(MaxX, minY, 0f), new Vector3(MaxX, maxY, 0f));
    }
}
