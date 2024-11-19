using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{
    public int maxAmmo = 1;
    public int currentAmmo;
    public float attackSpeed = .5f;
    public float reloadTime = 2f;

    public Bullet bulletPrefab;
    public Transform shootPoint;

    internal bool isReloading = false;
    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isReloading)
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
        currentAmmo--;

        Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

        StartCoroutine(AttackCooldown());
    }
    internal void Shoot(int amount, Vector3 variacion)
    {
        currentAmmo--;

        for (int i = 0; i < amount; i++)
        {
            Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        }

        StartCoroutine(AttackCooldown());
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
        yield return new WaitForSeconds(attackSpeed);
    }
}
