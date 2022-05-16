using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public float speed;
    void Start()
    {
       // * Tar objektet och flyttar det framåt på Z-axeln
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

}
