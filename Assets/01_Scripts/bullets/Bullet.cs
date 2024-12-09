using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public BulletType type;
    public float deadTime = 2f;
    public float speed = 5f;
    public int damage = 1;
    public Rigidbody rb;
    public GameObject explosion;
    public bool fromEnemy = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject,deadTime);
        rb.velocity = transform.forward * speed;
    }


    void OnDestroy()
    {
        if (type == BulletType.Explosive)
        {
            Instantiate(explosion,transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (!other.gameObject.CompareTag("Bullet"))
            {
                if (type == BulletType.Explosive) Instantiate(explosion, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}

public enum BulletType
{
    Normal,
    Explosive
} 
