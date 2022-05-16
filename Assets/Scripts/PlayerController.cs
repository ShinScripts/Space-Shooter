using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable] // * Gör klassen Boundary synlig i inspektören under playercontroller
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{
    public float speed, tilt, fireRate;
    public Boundary boundary;

    public GameObject shot;
    public Transform shotSpawn;

    public AudioClip boltShot;

    private float nextFire;

    void Update()
    {
        //* Om du håller ner eller trycker på mellanslag och tiden är större eller lika med nextfire, instantiate a shot
        if (Input.GetKey(KeyCode.Space) && Time.time >= nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation); // * Instantierar skotten (spawnar in dem)

            //* Sätter upp ljudet, hittar AudioSource komponenten inom objektet
            AudioSource sound = GetComponent<AudioSource>();
            //* Ljudets kill är boltShot
            sound.clip = boltShot;
            //* Spela upp ljudet
            sound.Play();
        }
    }

    //* Powerup funktionen, gör så att du sjuter snabbare under den tid du sagt (argument 1), sedan tar den tillbaka värdet till det den var innan
    IEnumerator SwitchFireRate(float time)
    {
        fireRate = 0.15f;
        yield return new WaitForSeconds(time);
        fireRate = 0.3f;
        yield return null;
    }

    //* Kollar om du har träffat något med ditt skepp
    void OnTriggerEnter(Collider other)
    {
        //* Om den har taggen "PowerUp", starts powerup funktionen i med tiden 3 sekunder och förstör objektet (Så man inte kan ta det flera gånger)
        if (other.CompareTag("PowerUp"))
        {
            StartCoroutine(SwitchFireRate(10));
            Destroy(other.gameObject);
        }
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        //* Skapar en vektor3 och sätt dens x, y och z argument
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        GetComponent<Rigidbody>().velocity = movement * speed; // * Ändra hastigheten på Rigidbody (player ship) till movement * den speed du anger

        //* Mathf.Clamp gör så att det första argumentet inte kan lämna den område man säger imellan den andra och tredje argumentet. 
        //* (Andra argument = -2. Tredje argument = 2. Man får ebdast vara mellan -2 och 2) Skapar en osynlig vägg på sidorna
        GetComponent<Rigidbody>().position = new Vector3(Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax), 0.0f, Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax));

        //* Rotetar Rigidbody (Player Ship) i Z-axeln. Tar hastigheten i X * -tilt 
        GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
    }
}
