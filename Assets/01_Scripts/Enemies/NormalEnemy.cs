using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : MonoBehaviour
{
    public int rutina;
    public float cronometro;
    public Animator animator;
    public Quaternion angulo;
    public float grado;

    public Player target;
    public bool playerLocated = false;
    public float playerDetection = 15f;
    public float maxDistance = 10f;
    public float locateTimer = 0f;

    public float speed = 5f;
    public bool walk = false;

    public int maxLife = 5;
    public int life;

    public bool canMove = true;
    public float waitMoveInterval = 1f;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        target = FindAnyObjectByType<Player>();
        life = maxLife;
    }

    // Update is called once per frame
    void Update()
    {
        Comportamiento();
    }

    public void Comportamiento()
    {
        if (canMove)
        {
            if (playerLocated)
            {

                if (Vector3.Distance(transform.position, target.transform.position) > maxDistance)
                {
                    WalkToPlayer();
                }
                else
                {
                    walk = false;
                    //attack
                    //mira al jugador

                    Vector3 directionToPlayer = target.transform.position - transform.position;
                    directionToPlayer.y = 0;
                    Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 0.5f);
                }
            }
            else
            {
                WalkFree();

                if (target != null && Vector3.Distance(transform.position, target.transform.position) < playerDetection)
                {
                    playerLocated = true;
                }
                else
                {
                    target = FindAnyObjectByType<Player>();
                }
            }

            Walk();
        }
    }

    #region caminatas
    private void WalkToPlayer()
    {
        var lookPos = target.transform.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 3);
        walk = true;
    }

    private void WalkFree()
    {
        cronometro += Time.deltaTime;
        if (cronometro >= 4)
        {
            rutina = Random.Range(0, 2);
            cronometro = 0;
        }
        switch (rutina)
        {
            case 0:
                walk = false;
                break;

            case 1:
                grado = Random.Range(0, 360);
                angulo = Quaternion.Euler(0, grado, 0);
                rutina++;
                break;

            case 2:
                transform.rotation = Quaternion.RotateTowards(transform.rotation, angulo, 0.5f);
                walk = true;
                break;
        }
    }

    void Walk ()
    {
        if (walk) transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    #endregion


    public void TakeDamage()
    {
        life -= 1;
        if (life <= 0)
        {
            target.AddPoint();
            Destroy(gameObject);
        }
    }

    IEnumerator AttackCooldown()
    {
        canMove = false;

        yield return new WaitForSeconds(waitMoveInterval);

        canMove = true;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Player player = collision.gameObject.GetComponent<Player>();
                player.TakeDamage();
                player.TakeDamage();
                StartCoroutine(AttackCooldown());
                if (animator != null) animator.SetTrigger("Attack");
            }
        }
    }
}
