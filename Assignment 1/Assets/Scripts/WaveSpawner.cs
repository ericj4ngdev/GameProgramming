using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public GameObject Enemy;
    public float startTime;
    public float endTime;
    public float spawnRate;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn", startTime, spawnRate);
        Invoke("CancelInvoke", endTime);
    }

    void Spawn()
    {
        Vector3 spawnArea = new Vector3(Random.Range(120, 150), 1.366f, Random.Range(120, 150));
        Vector3 spawnRotate = new Vector3(0, Random.Range(0, 180), 0);
        Instantiate(Enemy, spawnArea, Quaternion.Euler(spawnRotate));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
