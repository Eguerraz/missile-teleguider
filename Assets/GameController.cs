using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public GameObject missilePrefab;  // Le prefab du missile
    public float missileSpeedIncrease = 0.01f; // Augmentation de la vitesse des missiles
    public float spawnRateIncrease = 3f; // Augmentation de la fréquence des missiles
    public float spawnInterval = 2f; // Intervalle initial entre les spawns
    public float missileSpeed = 5f; // Vitesse initiale des missiles

    private float timeSinceLastSpawn = 0f;

    void Awake()
    {
        // Assurer qu'il y a une seule instance de GameController
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Gérer le spawn des missiles
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnMissile();
            timeSinceLastSpawn = 0f;

            // Augmenter la vitesse des missiles et la fréquence des spawns
            missileSpeed *= 1 + missileSpeedIncrease;
            spawnInterval -= spawnRateIncrease * Time.deltaTime * 2;

            // Limiter la fréquence des spawns pour ne pas aller en dessous de 0.5 seconde
            if (spawnInterval < 0.5f) spawnInterval = 0.5f;
        }
    }

    void SpawnMissile()
    {
        // Choisir un côté aléatoire de la map (gauche ou droite) pour faire apparaître le missile
        float spawnX = Random.Range(-8f, 8f); // Adapte ces valeurs en fonction de la taille de ton écran
        float spawnY = Random.Range(5f, 10f); // Distance de spawn au-dessus du joueur (ajuste selon l'écran)
        Vector2 spawnPosition = new Vector2(spawnX, spawnY);

        // Créer un missile
        GameObject missile = Instantiate(missilePrefab, spawnPosition, Quaternion.identity);

        // Ajouter la vitesse au missile
        Missile missileScript = missile.GetComponent<Missile>();
        if (missileScript != null)
        {
            missileScript.SetSpeed(missileSpeed);  // Définir la vitesse actuelle des missiles
        }
    }

}