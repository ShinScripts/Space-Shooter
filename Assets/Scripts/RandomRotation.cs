using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    void Start()
    {
        // * Så alla inte att roterar i samma hastighet
        float rotate = Random.Range(2, 4);

        //* Rotationshastigheten = en random position inom ett klot med en radius av 1, ta det multiplicerat med rotate
        GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * rotate;
    }
}
