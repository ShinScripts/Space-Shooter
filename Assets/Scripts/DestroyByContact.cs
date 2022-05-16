using UnityEngine;

public class DestroyByContact : MonoBehaviour
{
    public GameObject explosion, playerExplosion, asteroidScore;
    private GameController gameController;
    // private int death = 0;

    void Start()
    {
        //* letar efter ett objekt med taggen "GameController"
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");

        //* Ifall det finns ett objekt med taggen, ta GameController komponenten
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        //* Om det inte finns en gamekontroller, returnera en error i konsolen
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // * Om det andra föremålet som träffas av det huvudsakliga har taggen "Boundary", returnera ingenting
        if (other.CompareTag("Boundary"))
        {
            return;
        }

        //* Om det andra objektet har en tag "Player", skapa en explosion och kör gameover funktionen
        if (other.CompareTag("Player"))
        {
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            gameController.GameOver();
        }

        // * Ser till att du inte får en extra poäng genom att dö (Bug = när spelaren dör räknas den som en asteriod och ger dig en poäng)
        if (!other.CompareTag("Player"))
        {
            if (!CompareTag("Dynamite"))
            {
                gameController.AddScore(1);
            }
        }

        // * Om det du sjukter har en tag "Dynamite", skapa ett objekt som letar efter ett object med taggen "Player", förstör sedan objektet, skapa en explosion, och sätt gameover till true
        if (CompareTag("Dynamite"))
        {
            // death++;
            // if (death == 3)
            // {
            GameObject plr = GameObject.FindWithTag("Player");
            Destroy(plr);
            Instantiate(explosion, plr.transform.position, Quaternion.identity);
            gameController.GameOver();
            // }
        }

        Vector3 asteroidPos = new Vector3(transform.position.x, -4, transform.position.z);

        // * Ger explosionen en slumpmässig storlek från 1 - 3
        float size = Random.Range(1f, 4f);
        Vector3 scaler = new Vector3(size, size, size);
        explosion.transform.localScale = scaler;

        // * Kör explosionen vid det första objektets position och rotation
        Instantiate(explosion, transform.position, transform.rotation);
        // * Skapar ett objekt vart asteroiden blev förstörd
        Instantiate(asteroidScore, asteroidPos, Quaternion.Euler(0, 180, 0));

        // * Förstör båda objekten (som kolliderade)
        Destroy(other.gameObject);
        Destroy(gameObject);
    }
}
