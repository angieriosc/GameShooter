using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    public float speed = 2.5f;           // Velocidad normal del jugador
    public float slowSpeed = 1.25f;      // Velocidad cuando el jugador está moviéndose lentamente
    private float horizontalInput;       // Entrada para movimiento horizontal
    private float forwardInput;          // Entrada para movimiento vertical

    // Tecla para activar el movimiento lento
    public KeyCode slowKey = KeyCode.LeftShift;

    /// <summary>
    /// This method is called before the first frame update
    /// </summary>
    void Start()
    {
    }

    /// <summary>
    /// This method is called once per frame
    /// </summary>
    void Update()
    {
        // Obtener las entradas del jugador para el movimiento horizontal y vertical
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");

        // Comprobar si la tecla para el movimiento lento está presionada
        if (Input.GetKey(slowKey))
        {
            // Movimiento lento (reduce la velocidad)
            Vector3 movement = new Vector3(horizontalInput, 0, forwardInput) * slowSpeed * Time.deltaTime;
            transform.Translate(movement, Space.World);
        }
        else
        {
            // Movimiento normal
            Vector3 movement = new Vector3(horizontalInput, 0, forwardInput) * speed * Time.deltaTime;
            transform.Translate(movement, Space.World);
        }
    }
}
