using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletExplosion : MonoBehaviour
{
    public Vector3 targetScale = new Vector3(5f, 5f, 5f); // Escala objetivo
    public float growDuration = 1f; // Duración del crecimiento en segundos

    private float elapsedTime = 0f; // Tiempo transcurrido desde el inicio del crecimiento
    private bool isGrowing = false; // Indica si el objeto está creciendo

    public AudioClip sonido;

    void Start()
    {
        // Inicializar la escala del objeto en 0
        transform.localScale = Vector3.zero;

        // Iniciar el crecimiento
        StartGrowing();
        AudioManager.instance.PlaySFX(sonido);
    }

    public void StartGrowing()
    {
        isGrowing = true;
    }

    void Update()
    {
        if (isGrowing)
        {
            GrowOverTime();
        }
    }

    private void GrowOverTime()
    {
        // Incrementar el tiempo transcurrido
        elapsedTime += Time.deltaTime;

        // Calcular la escala proporcional al tiempo transcurrido
        float t = Mathf.Clamp01(elapsedTime / growDuration); // Valor normalizado (0 a 1)
        transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, t);

        // Destruir el objeto cuando termine el tiempo
        if (elapsedTime >= growDuration)
        {
            Destroy(gameObject); // Destruye el objeto al terminar
        }
    }
}
