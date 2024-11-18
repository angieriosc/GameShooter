using UnityEngine;
using TMPro;

public class BossLaser : MonoBehaviour
{
    public TextMeshProUGUI gameText;
    void OnTriggerEnter(Collider other) // Detecta colisiones
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
            UpdateResultText();
        }
    }
        void UpdateResultText()
    {
        // Actualiza el texto de la salud del jefe
        gameText.text = "You lose";
    }
}

