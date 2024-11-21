using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public float life;
    public float maxLife = 100f;

    public Image healthBar;
    public Image healthBar1;
    // Start is called before the first frame update
    void Start()
    {
        life = maxLife;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null && healthBar1 != null)
        {
            healthBar.fillAmount = (float)life / maxLife;
            healthBar1.fillAmount = (float)life / maxLife;
        }

    }
}
