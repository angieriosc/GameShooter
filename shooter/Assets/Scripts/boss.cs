using UnityEngine;
using TMPro;
using System.Collections;

public class Boss : MonoBehaviour
{
    public Rigidbody boss;                   // Prefab del jefe (opcional)
    public float health = 100f;              // Salud del jefe
    public TextMeshProUGUI healthText;   
    public TextMeshProUGUI gameText;      
    public float transitionTime = 40f;       // Tiempo para cambiar de fase
    public Rigidbody attackPrefab;           // Prefab del laser

    // Variables específicas para el ataques 
    public float intervalBetweenLevels = 3f; // Tiempo entre niveles
    public float intervalBetweenCircles = 4f; // Tiempo entre cada círculo de disparos
    public int repetitions = 3;              // Número de círculos de disparos
    public Rigidbody asteroidPrefab;         // Prefab del asteroide
    public float projectileSpeed = 20f;      // Velocidad de los disparos
    public float[] angles = { 0f, 45f, 90f, 135f, 180f, 225f, 270f, 315f }; // Ángulos para disparar en círculo
    public float[] anglesDestruction = { 0f, 10f};
    private enum BossState { WarmUp, Attack, Destruction, Laser, Wait, Wait2 };
    private BossState currentState;
    private float stateTimer;

    void Start()
    {
        currentState = BossState.WarmUp;    // Comienza en la primer fase
        stateTimer = transitionTime;        // Establece el tiempo para la transición de fases
        UpdateHealthText();
    }

    void Update()
    {
        // Actualiza el temporizador para cambiar de fase
        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0)
        {
            // Cambiar de fase cada vez que el temporizador llegue a cero
            ChangeState();
            stateTimer = transitionTime;    // Reiniciar el temporizador
        }

        // Ejecutar diferentes funciones dependiendo al estado actual
        switch (currentState)
        {
            case BossState.WarmUp:
                WarmUpBehavior();
                break;
            case BossState.Attack:
                AttackBehavior();
                break;
            case BossState.Destruction:
                DestructionBehavior();
                break;
            case BossState.Wait:
                WaitBehavior();
                break;
            case BossState.Wait2:
                WaitBehavior2();
                break; 
            case BossState.Laser:
                LaserBehavior();
                break; 
        }
    }

    void ChangeState()
    {
        // Cambiar entre los estados del jefe
        if (currentState == BossState.WarmUp)
        {
            currentState = BossState.Attack;
        }
        else if (currentState == BossState.Attack)
        {
            currentState = BossState.Wait2;
        }
        else if (currentState == BossState.Wait2)
        {
            currentState = BossState.Destruction;
        }
        else if (currentState == BossState.Destruction)
        {
            currentState = BossState.Wait;
        }
        else if (currentState == BossState.Wait)
        {
            currentState = BossState.Laser;
        }
    }

    void WarmUpBehavior()
    {
        // Iniciar el movimiento del JEFE
        StartCoroutine(MoveBoss());
    }

    private IEnumerator MoveBoss()
    {
        Vector3 firstPos = new Vector3(45f, 0, 20f);
        Vector3 secondPos = new Vector3(114.49f, 0, 20f);
        Vector3 thirdPos = new Vector3(114.49f, 0, 0.24f);

        float timeToMove = 18f;

        yield return StartCoroutine(MoveToPoint(firstPos, secondPos, timeToMove));
        yield return StartCoroutine(MoveToPoint(secondPos, thirdPos, timeToMove));
    }

    private IEnumerator MoveToPoint(Vector3 currentPos, Vector3 targetPos, float timeToMove)
    {
        float timeElapsed = 0;

        while (timeElapsed < timeToMove)
        {
            transform.position = Vector3.Lerp(currentPos, targetPos, timeElapsed / timeToMove);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Asegurarse de llegar exactamente al punto final
        transform.position = targetPos;
    }

    void AttackBehavior()
    {
        // Lógica de la fase de ataque
        Debug.Log("Jefe en fase de ataque...");

        // Inicia la corutina para manejar el ataque con retraso y repeticiones
        StartCoroutine(StartAfterDelay());
    }

    IEnumerator StartAfterDelay()
    {
        // Esperar antes de empezar las repeticiones
        yield return new WaitForSeconds(intervalBetweenLevels);

        // Iniciar la corutina para los disparos circulares
        StartCoroutine(RepeatCircularShots());
    }

    IEnumerator RepeatCircularShots()
    {
        for (int i = 0; i < repetitions; i++)
        {
            ShootAsteroids(); // Dispara un círculo completo de asteroides
            yield return new WaitForSeconds(intervalBetweenCircles); // Espera antes de la siguiente repetición
        }
    }

    void ShootAsteroids()
    {
        foreach (float angle in angles)
        {
            // Crear una rotación basada en el ángulo actual
            Quaternion rotation = Quaternion.Euler(0, angle, 0);

            // Replicar el asteroide con la rotación calculada
            Rigidbody instantiatedAsteroid = Instantiate(
                asteroidPrefab,
                transform.position,
                transform.rotation * rotation);

            // Establecer la velocidad del asteroide
            instantiatedAsteroid.linearVelocity = instantiatedAsteroid.transform.forward * projectileSpeed;
        }
    }
    void DestructionBehavior()
    {
        Debug.Log("Jefe en fase de DESTRUCCIÓN...");
        StartCoroutine(SpinAndShoot());
    }

    private IEnumerator SpinAndShoot()
    {
        float spinDuration = 2f; // Duración total del giro
        float elapsedTime = 0f;

        while (elapsedTime < spinDuration)
        {
            // Gira el jefe
            transform.Rotate(0, 5 * Time.deltaTime, 0); // Rotación suave alrededor del eje Y

            // Dispara los proyectiles en círculos
            foreach (float angle in anglesDestruction)
            {
                Quaternion rotation = Quaternion.Euler(0, angle, 0);
                Rigidbody instantiatedProjectile = Instantiate(
                    attackPrefab, 
                    transform.position, 
                    transform.rotation * rotation
                );

                // Asignar velocidad al proyectil
                instantiatedProjectile.linearVelocity = instantiatedProjectile.transform.forward * projectileSpeed;
            }

            // Espera un breve momento antes de disparar el siguiente círculo
            yield return new WaitForSeconds(3f);

            // Incrementar el tiempo transcurrido
            elapsedTime += 1f;
        }

        // Transición a la siguiente fase después de completar el giro
        currentState = BossState.Wait; 
    }

    void LaserBehavior()
    {
        // Lógica de la fase de destrucción
        Debug.Log("Jefe en fase de Laser...");
        StartCoroutine(ShootLaserProjectiles());
    }
    private IEnumerator ShootLaserProjectiles()
    {
        // Número de proyectiles que quieres disparar
        int numberOfProjectiles = 5; 
        float angleIncrement = 15f; // Ángulo de separación entre proyectiles
        float startAngle = -((numberOfProjectiles - 1) * angleIncrement) / 2f; // Para que los proyectiles estén centrados

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            // Calcular el ángulo para cada proyectil
            transform.Rotate(0, 5 * Time.deltaTime, 0); 
            float angle = startAngle + (i * angleIncrement);
            
            // Crear la rotación del proyectil basado en el ángulo
            Quaternion rotation = Quaternion.Euler(0, angle, 0);

            // Instanciar el proyectil (puede ser el mismo que usas para los asteroides)
            Rigidbody instantiatedProjectile = Instantiate(
                attackPrefab, 
                transform.position, 
                transform.rotation * rotation
            );

            // Asignar velocidad al proyectil
            instantiatedProjectile.linearVelocity = instantiatedProjectile.transform.forward * projectileSpeed;

            yield return null;  
        }
    }


    void WaitBehavior()
    {
        // Lógica de la fase de destrucción
        Debug.Log("Jefe en fase de ESPERA...");
    }
    void WaitBehavior2()
    {
        // Lógica de la fase de destrucción
        Debug.Log("Jefe en fase de ESPERA...");
    }

    void UpdateHealthText()
    {
        // Actualiza el texto de la salud del jefe
        healthText.text = "Health: " + health.ToString();
    }

    public void TakeDamage(float damage)
    {
        // Recibir daño
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
        UpdateHealthText();
    }

    void OnTriggerEnter(Collider other)
    {
        // Detectar ataque de laser
        if (other.CompareTag("PlayerLaser"))
        {
            // Llamar función para restar vida
            TakeDamage(1f);
        }
    }

    void Die()
    {
        // Lógica cuando el jefe muere
        Destroy(boss);
        Destroy(gameObject);
        UpdateResultText();
        
    }
    void UpdateResultText()
    {
        // Actualiza el texto de la salud del jefe
        gameText.text = "Winner";
    }

}
