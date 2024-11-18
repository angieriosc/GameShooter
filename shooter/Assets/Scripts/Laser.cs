using UnityEngine;

public class Laser : MonoBehaviour
{
    void OnTriggerEnter(Collider other) // Detecta colisiones con un objeto 3D
    {
        if (other.gameObject.CompareTag("AsteroidPrefab")) 
        {
            // Busca el componente Fracture en el objeto
            Fracture fracture = other.gameObject.GetComponent<Fracture>();

            if (fracture != null) 
            {
                fracture.FractureObject(); 
            }
            else
            {
                Destroy(other.gameObject);
            }
            Destroy(gameObject);
        }
    }
}
