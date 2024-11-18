using UnityEngine;

public class colision : MonoBehaviour
{
    public float bounceForce = 5f; // Fuerza de rebote

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AsteroidPrefab"))
        {
            Debug.Log("Triggered with Asteroid!");
            
            // Simular un rebote
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Empujar en dirección opuesta 
                Vector3 direction = transform.position - other.transform.position;
                rb.AddForce(direction.normalized * bounceForce, ForceMode.Impulse);
            }
        }
    }
}