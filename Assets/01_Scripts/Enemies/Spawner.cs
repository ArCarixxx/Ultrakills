using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform TopRight;
    public Transform BottomLeft;

    public float intervalTime = 3f;
    public int enemiesToSpawn = 1;
    public float actualInterval;
    public int actualenemiesToSpawn;
    public float timer = 0;
    public List<GameObject> EnemiesPrefabs;
    // Start is called before the first frame update
    void Start()
    {
        actualInterval = Random.Range(intervalTime / 2, intervalTime * 2);
        actualenemiesToSpawn = Random.Range(1,enemiesToSpawn);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > actualInterval)
        {
            timer = 0;

            for (int i = 0; i < actualenemiesToSpawn; i++)
            {
                Vector3 pos = new Vector3(Random.Range(BottomLeft.position.x, TopRight.position.x), transform.position.y, Random.Range(BottomLeft.position.z, TopRight.position.z));
                Instantiate(EnemiesPrefabs[Random.Range(0, EnemiesPrefabs.Count)], pos, Quaternion.identity);
                Debug.Log("spawneado");
            }
            actualInterval = Random.Range(intervalTime / 2, intervalTime * 2);
            actualenemiesToSpawn = Random.Range(1,enemiesToSpawn);
        }
        else
        {
            timer += Time.deltaTime;

        }
    }
}
