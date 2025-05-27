using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Contrôle des mouvements du joueur
        float moveX = 0f;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveX = -1.5f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            moveX = 1.5f;
        }

        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y); // Correction ici

        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // Si le joueur tombe dans le vide
        if (transform.position.y < -10f)
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Détection du sol pour permettre le saut
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Lorsque le joueur quitte le sol
        isGrounded = false;
    }

    void Die()
    {
        Destroy(gameObject); // Détruit le joueur
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name); // Recharge la scène
    }

    // Détection des collisions avec la dead zone
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si le joueur touche un objet avec le tag "DeadZone" ou un missile
        if (other.CompareTag("DeadZone") || other.CompareTag("Missile"))
        {
            Die();  // Appeler la fonction de mort du joueur
        }
    }
}
