using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform gunTurret;
    public float rotationAngle = 45f; // Maximum angle to rotate side to side
    public float rotationSpeed = 2f; // Speed of the rotation

    public Transform player; // Reference to the player
    public float detectionRange = 2f; // Range at which the sentry detects the player
    public float detectionAngle = 90f;  // Field of view angle for detection
    public float trackingSpeed = 5f; // Speed at which the gun rotates toward the player
    private bool playerDetected = false;

    public Transform firePoint;
    public Bullet bulletPrefab;

    private float startingRotationY; // Initial Y rotation
    private float currentRotationY; // Current Y rotation
    private bool rotatingRight = true; // Direction of rotation

    public float attackSpeed = 0.5f;
    public float timer = 0f;

    public int maxLife = 5;
    public int life;

    public Animator animator;
    public AudioSource audioShoot;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        // Automatically find the player in the scene by name
        GameObject playerObject = GameObject.Find("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Player not found in the scene");
        }

        if (gunTurret == null)
        {
            Debug.LogError("Gun cylinder is not assigned!");
            return;
        }

        if (player == null)
        {
            Debug.LogError("Player is not assigned!");
        }

        // Store the initial rotation of the sentry gun
        startingRotationY = transform.localEulerAngles.y;
        currentRotationY = startingRotationY;
        life = maxLife;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            // Check if the player is within detection range
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= detectionRange)
            {
                TrackPlayer(); // Rotate toward the player
                Attack();
            }
            else
            {
                IdleAnimation(); // Perform idle animation
            }
        }
    }

    void IdleAnimation()
    {
        // Calculate the target rotation angle based on the direction
        float targetRotationY = rotatingRight
            ? startingRotationY + rotationAngle
            : startingRotationY - rotationAngle;

        // Smoothly rotate the gun cylinder towards the target angle
        currentRotationY = Mathf.MoveTowards(currentRotationY, targetRotationY, rotationSpeed * Time.deltaTime);
        gunTurret.localEulerAngles = new Vector3(gunTurret.localEulerAngles.x, currentRotationY, gunTurret.localEulerAngles.z);

        // Check if the rotation reached the target and switch direction
        if (Mathf.Approximately(currentRotationY, targetRotationY))
        {
            rotatingRight = !rotatingRight;
        }
    }

    void DetectPlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;

        // Check if the player is within detection range
        if (directionToPlayer.magnitude <= detectionRange)
        {
            // Check if the player is within the field of view
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            if (angleToPlayer <= detectionAngle / 2)
            {
                // Perform a raycast to ensure there's no obstacle blocking the view
                if (Physics.Raycast(transform.position, directionToPlayer.normalized, out RaycastHit hit, detectionRange))
                {
                    if (hit.transform == player)
                    {
                        playerDetected = true;
                        return;
                    }
                }
            }
        }

        // If any condition fails, the player is not detected
        playerDetected = false;
    }

    void TrackPlayer()
    {
        // Calculate the direction to the player
        Vector3 directionToPlayer = player.position - gunTurret.position;

        // Calculate the target rotation to look at the player
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        // Add an offset to adjust the correct face
        Quaternion offsetRotation = Quaternion.Euler(0, 180, 0); // Adjust 180 degrees or any required angle
        targetRotation *= offsetRotation;

        // Smoothly rotate the gun cylinder toward the player
        gunTurret.rotation = Quaternion.Slerp(
            gunTurret.rotation,
            Quaternion.Euler(gunTurret.rotation.eulerAngles.x, targetRotation.eulerAngles.y, gunTurret.rotation.eulerAngles.z),
            trackingSpeed * Time.deltaTime
        );
    }

    void Attack()
    {
        if (timer < attackSpeed)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            Bullet bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            bullet.transform.LookAt(player.position);
            animator.SetTrigger("Shoot");
            audioShoot.Play();
        }
    }

    public void TakeDamage()
    {
        life -= 1;
        if (life <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.gameObject.CompareTag("Bullet"))
            {
                var obj = other.gameObject.GetComponent<Bullet>();
                if (!obj.fromEnemy)
                {
                    TakeDamage();
                    if (obj.type == BulletType.Explosive) Instantiate(obj.explosion, obj.transform.position, Quaternion.identity);
                }

                Destroy(other.gameObject);

            }
            else if (other.gameObject.CompareTag("Explosion"))
            {
                TakeDamage();
            }
        }
    }
}
