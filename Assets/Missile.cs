using UnityEngine;
using UnityEngine.SceneManagement;

public class Missile : MonoBehaviour
{
    public float speed = 5f;
    public float spawnDistance = 10f; // Distance par rapport au bord de l'écran
    private Transform player;
    private Rigidbody2D rb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();

        if (player != null && rb != null)
        {
            // Calcul des limites de la caméra
            Camera mainCamera = Camera.main;
            float screenWidth = mainCamera.orthographicSize * mainCamera.aspect;
            float screenHeight = mainCamera.orthographicSize;

            // Choisir un côté aléatoire : 0 = gauche, 1 = droite, 2 = haut
            int side = Random.Range(0, 3);
            Vector3 spawnPosition = Vector3.zero;

            switch (side)
            {
                case 0: // Gauche
                    spawnPosition = new Vector3(-screenWidth - spawnDistance, Random.Range(-screenHeight, screenHeight), 0);
                    break;
                case 1: // Droite
                    spawnPosition = new Vector3(screenWidth + spawnDistance, Random.Range(-screenHeight, screenHeight), 0);
                    break;
                case 2: // Haut
                    spawnPosition = new Vector3(Random.Range(-screenWidth, screenWidth), screenHeight + spawnDistance, 0);
                    break;
            }

            // Appliquer la position de spawn
            transform.position = spawnPosition;

            // Désactiver la gravité
            rb.gravityScale = 0;
        }
        else
        {
            Debug.LogError("Player ou Rigidbody2D manquant !");
        }

        // Détruire automatiquement le missile après 5 secondes
        Destroy(gameObject, 5f);
    }

    void FixedUpdate()
    {
        if (player == null || rb == null) return;

        // Calcule la direction actuelle du missile
        Vector2 currentDirection = rb.velocity.normalized;

        // Calcule la direction souhaitée vers le joueur
        Vector2 targetDirection = (player.position - transform.position).normalized;

        // Interpolation entre l'ancienne direction et la nouvelle
        float turnSpeed = 2f; // Plus cette valeur est basse, plus le missile tourne lentement
        Vector2 newDirection = Vector2.Lerp(currentDirection, targetDirection, Time.fixedDeltaTime * turnSpeed).normalized;

        // Applique la nouvelle vitesse interpolée
        rb.velocity = newDirection * speed;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
            ReloadScene();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
            ReloadScene();
        }
    }

    private void ReloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
        if (rb != null)
        {
            rb.velocity = rb.velocity.normalized * speed;
        }
    }
}
