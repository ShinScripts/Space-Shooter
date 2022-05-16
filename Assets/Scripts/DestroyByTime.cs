using UnityEngine;

public class DestroyByTime : MonoBehaviour
{
    public float lifeTime;

    void Start()
    {
        // * Förstör spelobjektet efter den angivna tiden (andra argumentet)
        Destroy(gameObject, lifeTime);
    }
    
}
