using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponType Type;
    public int maxAmmo = 1;
    public int currentAmmo;
    public float attackSpeed = .5f;
    public float reloadTime = 2f;
    public int bulletAmount = 1;

    public Bullet bulletPrefab;
    public Transform shootPoint;
    public Transform objetivePoint;
    public Vector3 bulletVariation;

    bool isReloading = false;
    bool canAttack = false;

    void Start()
    {
        currentAmmo = maxAmmo;
        canAttack = true;
    }

    void Update()
    {
        Attack();
    }

    public void Attack()
    {
        if (Input.GetKey(KeyCode.Mouse0) && !isReloading)
        {
            if (currentAmmo > 0)
            {
                Shoot();
            }
            else
            {
                Reload();
            }
        }
    }

    internal void Shoot()
    {
        if (canAttack)
        {
            currentAmmo--;

            for (int i = 0; i < bulletAmount; i++)
            {
                Vector3 variacion = new Vector3(Random.Range(-bulletVariation.x, bulletVariation.x),
                                                Random.Range(-bulletVariation.y, bulletVariation.y),
                                                Random.Range(-bulletVariation.z, bulletVariation.z));
                Bullet bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
                bullet.transform.LookAt(objetivePoint.position + variacion);
            }

            StartCoroutine(AttackCooldown());
        }
    }

    internal void Reload()
    {
        if (!isReloading)
            StartCoroutine(ReloadCoroutine());
    }

    IEnumerator ReloadCoroutine()
    {
        isReloading = true;
        Debug.Log("Recargando...");

        // Mostrar algún tipo de UI de recarga si es necesario
        // Aquí podrías añadir efectos de sonido, animaciones, etc.

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;

        Debug.Log("Recarga completa!");
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;

        yield return new WaitForSeconds(attackSpeed);

        canAttack = true;
    }
}

public enum WeaponType
{
    Escopeta,
    Metralleta,
    Bazooka
}
