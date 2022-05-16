using System.Collections;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject asteroids, Dynamite, Powerup;

    public Vector3 spawnValues;
    public int hazardCount;
    public float minSize, maxSize;
    public float spawnWait, startWait, waveWait;
    public GameObject scoreText, RestartText, GameOverText, explosion;
    private int score, highScore;
    private bool gameOver;

    private AudioSource[] sounds;
    private AudioSource StartMusic, SecondMusic, ThirdMusic, GameSound;
    private static bool a1 = false, a2 = false, a3 = false;

    //* Ställer in alla standardvärden
    void Start()
    {
        gameOver = false;
        RestartText.GetComponent<TextMeshProUGUI>().text = "";
        GameOverText.GetComponent<TextMeshProUGUI>().text = "";

        //* Ställer in asteroidhastigheten till -7 i början av varje spel och ser till att hazardCount börjar med ett värde på 5
        getTheMoverScript(asteroids, -7);
        getTheMoverScript(Dynamite, -7);
        hazardCount = 5;

        //* Ställer in poängen till 0 i början av varje spel
        score = 0;
        //* Ladda in din senaste HighScore
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateScore();

        StartCoroutine(spawnWaves());

        //* Ställer in musiken
        sounds = GetComponents<AudioSource>();
        StartMusic = sounds[0];
        SecondMusic = sounds[1];
        ThirdMusic = sounds[2];
        GameSound = sounds[3];

        StartMusic.time = 15f;

        //* Sätter de 3 boolean värderna till false
        a1 = false;
        a2 = false;
        a3 = false;
    }

    //* För att stödja äldre versioner
    [System.Obsolete]
    void Update()
    {
        AudioLowPassFilter audioLowPass = gameObject.GetComponent<AudioLowPassFilter>();
        if (PauseMenu.GameIsPaused)
        {
            audioLowPass.enabled = true;
        }
        else
        {
            audioLowPass.enabled = false;
        }
            
        // * Om gameover är sant, visa texten och ställ restart till true
        if (gameOver)
        {
            RestartText.GetComponent<TextMeshProUGUI>().text = "Press 'R' to restart!";

            if (Input.GetKeyDown(KeyCode.R))
            {
                //* Om R trycks in, ladda nivån som laddades vid den första initialiseringen
                Application.LoadLevel(Application.loadedLevel);
            }
        }

        //* Om din score är lika med eller mindre än 0 efter 10 sekunder efter att scenen har laddat, reference player och förstör objektet, kör sedan gameover funktionen
        if (Time.timeSinceLevelLoad > 10 && score <= 0)
        {
            try
            {
                GameObject plr = GameObject.FindWithTag("Player");
                Destroy(plr);
                Instantiate(explosion, plr.transform.position, Quaternion.identity);
                GameOver();
            }
            catch
            {
                return;
            }
        }

        //* Sångbyte beroende på din poäng
        if (score == 0 && a1 == false)
        {
            StartCoroutine(Fade(StartMusic, 5, 1));
            a1 = true;
        }
        else if (score == 49 && a2 == false)
        {
            SecondMusic.time = 0;
        }
        else if (score == 50 && a2 == false)
        {
            StartCoroutine(Fade(StartMusic, 7, 0));
            StartCoroutine(Fade(SecondMusic, 10, .7f));
            a2 = true;
        }
        else if (score == 99 && a3 == false)
        {
            ThirdMusic.time = 0;
        }
        else if (score == 100 && a3 == false)
        {
            StartCoroutine(Fade(SecondMusic, 7, 0));
            StartCoroutine(Fade(ThirdMusic, 10, 1));
            a3 = true;
        }
    }

    //* För att kunna vänta emellan koden
    IEnumerator Fade(AudioSource audioSource, float duration, float targetVolume)
    {
        //* Sätter upp default values
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            //* Tiden mellan aktuell och föregående frame
            currentTime += Time.deltaTime;
            // * Gör en linjär ökning / minskning av volymen under varaktigheten
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }

    // IEnumerator Wait(float time)
    // {
    //     if (score >= 0 && score < 35)
    //     {
    //         StartMusic.volume = .3f;
    //         GameSound.Play();
    //         yield return new WaitForSeconds(time);
    //         StartCoroutine(Fade(StartMusic, 2, 1f));
    //     }

    // }
    //     yield return null;

    //* För att kunna vänta emellan koden
    IEnumerator spawnWaves()
    {
        //* Väntar i x antal sekunder innan den börjar köra koden nedan
        yield return new WaitForSeconds(startWait);

        //* Infinite loop
        while (true)
        {
            // * Medan i är mindre än hazardCount, kör koden nedan
            for (int i = 0; i < hazardCount; i++)
            {
                // * Om gameOver är lika med true, sluta spawna in asteroider
                if (gameOver)
                {
                    //* För att sluta instantiate asteroider
                    hazardCount = 0;
                    //* Slute köra all kod, ett stopp
                    break;
                }

                // * Random.Range tar siffran, gör 2 nummer av det, 1 negativt och 1 positivt. (Om talet är 10, Random.Range = Random.Range (-10, 10))
                Vector3 random1 = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Vector3 random2 = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Vector3 random3 = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Vector3 random4 = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);

                // * Ger asteroiderna en slumpmässig storlek för varje spawn
                float size = Random.Range(minSize, maxSize);
                //* Skapar en ny vektor 3 med samma värde på x, y och z
                Vector3 scaler = new Vector3(size, size, size);
                //* Sätter asteriodens storlek till scaler
                asteroids.transform.localScale = scaler;

                // * Ställer in spawn-rotation till värdet 0
                Quaternion spawnRotation = Quaternion.identity;

                Instantiate(asteroids, random1, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
                Instantiate(asteroids, random2, spawnRotation);
                yield return new WaitForSeconds(spawnWait);

                // * Gör det 25% chans för en dynamit att spawna in 
                if (Random.value < .25)
                {
                    Instantiate(Dynamite, random3, spawnRotation);
                    yield return new WaitForSeconds(spawnWait);
                }

                // * 3% chans för en powerup att spawna in
                if (Random.value < .03)
                {
                    Instantiate(Powerup, random4, Quaternion.Euler(0, 180, 0));
                }
            }
            //* Väntar med att köra resten av koden i x antal sekunder
            yield return new WaitForSeconds(waveWait);

            // * Gör asteroiderna snabbare efter varje våg
            ChangeAsteroidSpeed();

            // * hazardCount ökar med 1 efter varje våg
            hazardCount++;
        }
    }

    // * En funktion som ändrar asteroidhastigheten med 1 enhet
    private void ChangeAsteroidSpeed()
    {
        //* Referaar till Mover skripten och dens "speed" property, sätter den sedan till samma sak minus 1 (för att bli snabbare)
        asteroids.GetComponent<Mover>().speed = asteroids.GetComponent<Mover>().speed - 1;
        Dynamite.GetComponent<Mover>().speed = Dynamite.GetComponent<Mover>().speed - 1;
    }

    public void AddScore(int scoreValue)
    {
        // * Lägg till scoreValue i score
        score += scoreValue;

        // * Uppdaterar bara poängen medan gameover = false
        if (!gameOver)
        {
            //* Så länge gameOver är falskt, uppdatera poängen
            UpdateScore();
        }
    }

    // * Hämta textkomponenten och hänvisa till "text"-objektet inuti den och ändra den sedan till Poäng: (poäng)
    void UpdateScore()
    {
        //* Hittar komponenten text inom scoretext och referar till texten, sedan ändrar den texten till "Score" med (value)
        scoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + score;
    }

    //* Hitta mover skriptet inom objektet och ändra dens hastighet till det du säger
    private void getTheMoverScript(GameObject script, float theSpeed)
    {
        script.GetComponent<Mover>().speed = theSpeed;
    }

    public void GameOver()
    {
        //* Om din score är högre än din förra score, set HighScore till score och spara det i PlayerPrefs
        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", score);
        }
        //* Hittar text komponenten inom objektet GameOverText och ändrar texten. @ gör så att man kan skriva på flera rader och inte bara en
        GameOverText.GetComponent<TextMeshProUGUI>().text = @"Game Over! 

                                                            Score: " + score + @"
                                                            
                                                            Highscore: " + highScore;
        //* Ändrar gameOver boolean till truw
        gameOver = true;
        //* Spelar upp ljuded för när man dör
        GameSound.Play();
        PauseMenu.over = true;
    }
}