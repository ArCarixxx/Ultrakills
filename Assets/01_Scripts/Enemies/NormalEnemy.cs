using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class NormalEnemy : MonoBehaviour
{
    public int rutina;
    public float cronometro;
    public Animator animator;
    public Quaternion angulo;
    public float grado;

    public GameObject target;
    public bool playerLocated = false;
    public float playerDetection = 15f;
    public float maxDistance = 10f;
    public bool maxDistanceReached = false;
    public float locateTimer = 0f;

    public float speed = 5f;
    public bool walk = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Comportamiento();
    }

    public void Comportamiento()
    {
        if (playerLocated)
        {

            if (!maxDistanceReached && Vector3.Distance(transform.position, target.transform.position) < maxDistance)
            {
                WalkToPlayer();
            }
            else
            {
                maxDistanceReached = true;
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

            locateTimer += Time.deltaTime;

            if (locateTimer > 1)
            {
                if (Vector3.Distance(transform.position, target.transform.position) < playerDetection)
                {
                    playerLocated = true;
                }
                locateTimer = 0;
            }
        }

        Walk();
    }

    #region caminatas
    private void WalkToPlayer()
    {
        var lookPos = target.transform.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 3);
        if (animator != null) animator.SetBool("Walk", true);
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
                if (animator != null) animator.SetBool("Walk", false);
                walk = false;
                break;

            case 1:
                grado = Random.Range(0, 360);
                angulo = Quaternion.Euler(0, grado, 0);
                rutina++;
                break;

            case 2:
                transform.rotation = Quaternion.RotateTowards(transform.rotation, angulo, 0.5f);
                if (animator != null) animator.SetBool("Walk", true);
                walk = true;
                break;
        }
    }

    void Walk ()
    {
        if (walk) transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    #endregion
}
