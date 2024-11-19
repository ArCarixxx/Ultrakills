using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootGun : weapon
{
    public int bulletAmount = 5;
    public Vector2 bulletVariationX;
    public Vector2 bulletVariationY;
    public Vector2 bulletVariationZ;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isReloading)
        {
            if (currentAmmo > 0)
            {
                Vector3 variacion = new Vector3(Random.Range(bulletVariationX.x, bulletVariationX.y), Random.Range(bulletVariationY.x,bulletVariationY.y), Random.Range(bulletVariationZ.x,bulletVariationZ.y));
                Shoot(bulletAmount);
            }
            else
            {
                Reload();
            }
        }
    }
}
