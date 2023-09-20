using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float delay;

    // Start is called before the first frame update
    void Start()
    {
        delay = 5; 
        Destroy(gameObject, delay);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(2 * Time.deltaTime, 0, 0);
    }
}
