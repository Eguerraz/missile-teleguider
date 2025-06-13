using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float acceleration = 5f;     // accélération avant/arrière
    public float maxSpeed = 10f;        // vitesse max
    public float rotationSpeed = 150f;  // vitesse de rotation (degrés/s)
    public float driftFactor = 0.95f;   // glisse latérale

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // --- Entrées clavier ---
        float rotationInput = 0f;
        if (Input.GetKey(KeyCode.LeftArrow)) rotationInput = 1f;
        else if (Input.GetKey(KeyCode.RightArrow)) rotationInput = -1f;

        float accelerationInput = 0f;
        if (Input.GetKey(KeyCode.UpArrow)) accelerationInput = 1f;
        else if (Input.GetKey(KeyCode.DownArrow)) accelerationInput = -1f;

        // --- Rotation (seulement si on appuie) ---
        if (rotationInput != 0f)
            rb.MoveRotation(rb.rotation + rotationInput * rotationSpeed * Time.fixedDeltaTime);

        // --- Accélération dans la direction actuelle ---
        Vector2 forward = transform.up;
        rb.AddForce(forward * accelerationInput * acceleration);

        // --- Limitation de la vitesse max ---
        if (rb.velocity.magnitude > maxSpeed)
            rb.velocity = rb.velocity.normalized * maxSpeed;

        // --- Drift ---
        Vector2 forwardVelocity = forward * Vector2.Dot(rb.velocity, forward);
        Vector2 sideVelocity = transform.right * Vector2.Dot(rb.velocity, transform.right);
        rb.velocity = forwardVelocity + sideVelocity * driftFactor;

        ClampToCamera();
    }

    void ClampToCamera()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        viewPos.x = Mathf.Clamp01(viewPos.x);
        viewPos.y = Mathf.Clamp01(viewPos.y);
        transform.position = Camera.main.ViewportToWorldPoint(viewPos);
    }

    void Die()
    {
        Destroy(gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DeadZone") || other.CompareTag("Missile"))
        {
            Die();
        }
    }
}
