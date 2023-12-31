using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed;
    public float time;
    public ParticleSystem deadEffect;
    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(null);
        // speed = 10;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        transform.Translate(0, -speed * Time.deltaTime, 0);
        if (time > 3)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Instantiate(deadEffect, other.transform.position, Quaternion.Euler(new Vector3(-90,0,0)) );
            other.GetComponent<Enemy>().TakeDamage();
            Destroy(gameObject);
        }
    }
}