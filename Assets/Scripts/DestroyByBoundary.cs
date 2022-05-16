using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByBoundary : MonoBehaviour
{

    private GameController gameController;
    private int removeScore = -3;

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

    // * När ett objekt lämnar det andra objektet förstörs det (används för prestanda)
    void OnTriggerExit(Collider other)
    {
        //* Förstör objektet som lämnade spelytan
        Destroy(other.gameObject);
        //* Ifall detta objekt inte hade taggen "Shootable"
        if (!other.gameObject.CompareTag("Shootable"))
        {
            //* Ifall detta objekt inte hade taggen "Dynamite"
            if (!other.gameObject.CompareTag("Dynamite"))
            {
                //* Ta bort 3 poäng
                gameController.AddScore(removeScore);
            }
        }
    }
}