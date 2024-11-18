using System.Collections;
using UnityEngine;

public class Enemies2 : MonoBehaviour
{
    public Rigidbody asteroidPrefab; // Prefab del asteroide a copiar
    public float projectileSpeed = 2f; // Velocidad del asteroide
    public float movementSpeed = 1f; // Velocidad de movimiento del disparador
    public int repetitions = 4; // Número de repeticiones del disparo circular
    public float intervalBetweenCircles = 1f; // Tiempo entre cada disparo circular
    public float intervalBetweenLevels = 2f; // Tiempo inicial antes de empezar a disparar

    // Ángulos específicos para los disparos
    private readonly float[] angles = { 45f, 90f, 135f, 180f, 225f, 270f, 315f, 360f };

    void Start()
    {
        StartCoroutine(StartAfterDelay());
    }

    void Update()
    {
        MoveShooter(); 
    }

    void MoveShooter()
    {
        transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
    }


    IEnumerator StartAfterDelay()
    {
        yield return new WaitForSeconds(intervalBetweenLevels);

        StartCoroutine(RepeatCircularShots());
    }

 // Funcion for para disparar muchos círculos de asteroides
    IEnumerator RepeatCircularShots()
    {
        for (int i = 0; i < repetitions; i++)
        {
            ShootAsteroids();
            yield return new WaitForSeconds(intervalBetweenCircles); 
        }
    }

 // Funcion que dispara un círculo de asteroides
    void ShootAsteroids()
    {
        foreach (float angle in angles)
        {
            // Crear una rotación basada en el ángulo actual
            Quaternion rotation = Quaternion.Euler(0, angle, 0);

            // Repiclar el asteroide con la rotación calculada
            Rigidbody instantiatedAsteroid = Instantiate(
                asteroidPrefab,
                transform.position,
                transform.rotation * rotation);

            // Velocidad del asteroide
            instantiatedAsteroid.linearVelocity = instantiatedAsteroid.transform.forward * projectileSpeed;
        }
    }
}
