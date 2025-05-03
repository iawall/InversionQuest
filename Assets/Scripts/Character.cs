using UnityEngine;
using UnityEngine.SceneManagement;
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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Horizontal Movement
        float moveX = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);

        // Invert gravity when spacebar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isGravityInverted = !isGravityInverted;
            rb.gravityScale *= -1;

            // Flip the character's Y scale for visual effect (optional)
            Vector3 scale = transform.localScale;
            scale.y *= -1;
            transform.localScale = scale;
        }
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
                //Debug.Log("Goal reached with key! Level complete.");
                SceneManager.LoadScene("level2");
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
