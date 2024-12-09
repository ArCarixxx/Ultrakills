using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public float life;
    public float maxLife = 100f;

    public Image healthBar;

    public Image weaponImage;
    public TextMeshProUGUI totalBulletsText;
    public TextMeshProUGUI availableBulletsText;

    public TextMeshProUGUI pointsText;
    public int totalPoints;

    public List<Weapon> weapons = new List<Weapon>();
    private int currentWeaponIndex = 0; 

    void Start()
    {
        life = maxLife;
        UpdateWeaponCanvas();
        totalPoints = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeToNextWeapon();
        }
        UpdateWeaponCanvas();
    }

    public void TakeDamage()
    {
        life -= 1;
        if (life <= 0)
        {
            //algo
        }
        else
        {
            UpdateHealthBar();
        }
    }
    public void AddPoint()
    {
        totalPoints++;
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = (float)life / maxLife;
        }
    }

    public void ChangeToNextWeapon()
    {
        weapons[currentWeaponIndex].gameObject.SetActive(false);

        currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count; 
        UpdateWeaponCanvas();

        weapons[currentWeaponIndex].gameObject.SetActive(true);
    }

    private void UpdateWeaponCanvas()
    {
        if (weapons.Count > 0 && weaponImage != null && totalBulletsText != null && availableBulletsText != null)
        {
            weaponImage.sprite = weapons[currentWeaponIndex].data.weaponSprite;
            availableBulletsText.text = weapons[currentWeaponIndex].currentAmmo.ToString();
            totalBulletsText.text = $"/ {weapons[currentWeaponIndex].maxAmmo.ToString()}";
            pointsText.text = $"{totalPoints.ToString()}";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.gameObject.CompareTag("Bullet"))
            {
                var obj = other.gameObject.GetComponent<Bullet>();
                if (obj.fromEnemy)
                {
                    TakeDamage();
                }

                Destroy(other.gameObject);
            }
        }
    }
}