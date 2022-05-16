using UnityEngine;

public class BGScroller : MonoBehaviour
{
    public float scrollSpeed, tileSizeZ;

    private Vector3 startPosition;

    void Start()
    {
        //* Sätter vektor3 startpositionen till den position den börjar på
        startPosition = transform.position;
    }

    void Update()
    {
        //* För varje bild så upprepar den bilden. Tar tiden * den hastighet du väljer. Andra argumemntet är hur stor bilden är, så den vet när den ska upprepas. (Den går from 0 till tileSizeZ, när den nått tileSizeZ går den tillbaka till 0 osv)
        float newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSizeZ);
        //* Sätt positionen till startpositionen + Z-axeln * newPosition
        gameObject.transform.position = startPosition + Vector3.forward * newPosition;
    }
}
