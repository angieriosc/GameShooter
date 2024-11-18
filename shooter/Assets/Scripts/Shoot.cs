using UnityEngine;
using TMPro; 

public class Shoot : MonoBehaviour
{
    public Rigidbody projectile; // El prefab del proyectil a replicar
    public float speed = 20f;  
    public TextMeshProUGUI bulletCounterText; 

    private int bulletCount = 0; 

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            // Replicar el laser en la posición y rotación actuales del objeto
            Rigidbody instantiatedProjectile = Instantiate(
                projectile, transform.position, transform.rotation);
            // velocidad del laser
            instantiatedProjectile.linearVelocity = transform.TransformDirection(new Vector3(0, 0, speed));
            bulletCount++;
            UpdateBulletCounter();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            Rigidbody instantiatedProjectile = Instantiate(
                projectile, transform.position, transform.rotation);
            instantiatedProjectile.linearVelocity = transform.TransformDirection(new Vector3(0, 0, speed* 2f));
            bulletCount++;
            UpdateBulletCounter();
        }

        if (Input.GetButtonDown("Fire3"))
        {
            Rigidbody instantiatedProjectile = Instantiate(
                projectile, transform.position, transform.rotation);
            instantiatedProjectile.linearVelocity = transform.TransformDirection(new Vector3(1, 1, speed *0.5f));
            bulletCount++;
            UpdateBulletCounter();
        }
    }

    void UpdateBulletCounter()
    {
        bulletCounterText.text = "Bullets: " + bulletCount;
    }
}
