using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audiosMenu : MonoBehaviour
{
    public AudioClip musica;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.SetMusic(musica);
    }
}
