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
    public Image healthBar1;

    public Image weaponImage;
    public Image weaponImage1;
    public TextMeshProUGUI totalBulletsText;
    public TextMeshProUGUI totalBulletsText1;
    public TextMeshProUGUI availableBulletsText; 
    public TextMeshProUGUI availableBulletsText1;

    public List<Weaponnnn> weaponsUI = new List<Weaponnnn>();
    public List<Weapon> weapons = new List<Weapon>();
    private int currentWeaponIndex = 0; 

    void Start()
    {
        life = maxLife;
        UpdateWeaponCanvas();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeToNextWeapon();
        }
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

    private void UpdateHealthBar()
    {
        if (healthBar != null && healthBar1 != null)
        {
            healthBar.fillAmount = (float)life / maxLife;
            healthBar1.fillAmount = (float)life / maxLife;
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
        if (weaponsUI.Count > 0 && weaponImage != null && totalBulletsText != null && availableBulletsText != null)
        {
            Weaponnnn currentWeapon = weaponsUI[currentWeaponIndex];
            weaponImage.sprite = currentWeapon.weaponSprite;
            availableBulletsText.text = currentWeapon.availableBullets.ToString();
            totalBulletsText.text = $"/ {currentWeapon.totalBullets}";
            weaponImage1.sprite = currentWeapon.weaponSprite;
            availableBulletsText1.text = currentWeapon.availableBullets.ToString();
            totalBulletsText1.text = $"/ {currentWeapon.totalBullets}";
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

[System.Serializable]
public class Weaponnnn
{
    public string weaponName; 
    public Sprite weaponSprite; 
    public int totalBullets; 
    public int availableBullets; 
}
