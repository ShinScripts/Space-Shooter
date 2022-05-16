using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 2f;
    private AudioSource music;
    private GameObject levelLoader;

    void Start()
    {
        //* Hittar ett objekt med taggen "LevelLoader"
        levelLoader = GameObject.FindWithTag("LevelLoader");
        //* Försök att hitta en audiosource, sätt sedan dens startpunkt till 45 sekunder och starta fade funktionen (ta upp musikens volym till 1)
        try
        {
            music = levelLoader.GetComponent<AudioSource>();
            music.time = 45;
            StartCoroutine(Fade(music, 3, .5f));
        }
        //* Om det inte gick att köra koden ovan, returnera inget (Så man inte får errors)
        catch
        {
            return;
        }
    }

    //* Kör fade funktionen igen (ta ner musikens volym till 0) och kör sedan LoadNextLevel funktionen
    void PlayGame()
    {
        StartCoroutine(Fade(music, 2, 0));
        LoadNextLevel();
    }

    //* Tar den scen du är i, hittar buildIndexen och ökar den med 1
    void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    //* Kör transitionen genoma att sätta start triggern till true, vänta sedan med att köra koden (WaitForSeconds). Ladda sedan scenen som du har sagt och returnera null (inget)
    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);

        yield return null;
    }

    //* Stänger ned applikationen
    void QuitGame()
    {
        Application.Quit();
    }

    //* Fade funktionen (Finns en bättre beskrivning i gamecontroller)
    IEnumerator Fade(AudioSource audioSource, float duration, float targetVolume)
    {
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
}