using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletExplosion : MonoBehaviour
{
    public int damage = 1;
    public float duration = 1f;

    void Start()
    {
        Destroy(gameObject, duration);
    }
}
