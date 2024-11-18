using System.Collections;
using UnityEngine;

public class Enemies1 : MonoBehaviour
{
    public Rigidbody enemiesl1; // Prefab del proyectil a instanciar
    public float projectileSpeed = 2f; // Velocidad del proyectil
    public float movementSpeed = 1f; // Velocidad de movimiento del disparador
    public float interval = 0.5f; // Intervalo entre cada disparo
    public int shots = 18; // Número total de disparos
    public float spreadAngle = 180f; // Ángulo total de dispersión

    void Start()
    {
        StartCoroutine(ShootInPattern());
    }

    void Update()
    {
        // Mover al disparador mientras dispara
        MoveShooter();
    }

    void MoveShooter()
    {
        transform.Translate( Vector3.forward * movementSpeed * Time.deltaTime);

    }

    IEnumerator ShootInPattern()
    {
        for (int i = 0; i < shots; i++)
        {
            // Calcular ángulos de disparo 
            float angle = i * (spreadAngle / (shots - 1)) - spreadAngle / 2;

            // Crear una rotación basada en los angulos
            Quaternion rotation = Quaternion.Euler(0, angle, 0);

            // Replicar el proyectil con la rotación calculada
            Rigidbody instantiatedProjectile = Instantiate(
                enemiesl1, transform.position, transform.rotation * rotation);

            // Establecer la velocidad
            instantiatedProjectile.linearVelocity = instantiatedProjectile.transform.forward * projectileSpeed;

            // Esperar antes del siguiente
            yield return new WaitForSeconds(interval);
        }
    }
}
