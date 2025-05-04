using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Character : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private bool isGravityInverted = false;

    private bool hasKey = false;
    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.1f;
    public LayerMask groundLayer;
    private bool isGrounded = false;

    private float moveX;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Capture input
        moveX = Input.GetAxisRaw("Horizontal");

        // Ground check
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        // Invert gravity ONLY if grounded
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            isGravityInverted = !isGravityInverted;
            rb.gravityScale *= -1;

            // Flip character visually
            Vector3 scale = transform.localScale;
            scale.y *= -1;
            transform.localScale = scale;
        }
    }

    void FixedUpdate()
    {
        // Apply constant linear movement
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);

        // Clamp position to bounds
        float minX = -9f;
        float maxX = 9f;
        float minY = -5f;
        float maxY = 5f;

        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);
        transform.position = clampedPosition;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("key"))
        {
            hasKey = true;
            Destroy(other.gameObject); // Remove key from scene
            Debug.Log("Key collected!");
        }
        else if (other.CompareTag("door"))
        {
            if (hasKey)
            {
                Debug.Log("Goal reached with key! Showing level complete UI.");
                LevelCompleteManager.Instance?.ShowLevelCompleteUI();
            }
            else
            {
                Debug.Log("You need the key to complete the level.");
            }
        }
        else if (other.CompareTag("hazard"))
    {
        Debug.Log("Hit a hazard! Restarting level...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    }


    // Optional: draw ground check gizmo in editor
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }
    }
}
