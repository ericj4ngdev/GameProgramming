using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortEnemy : Enemy
{
    public override void TakeDamage()
    {
        bool isDie = false;//= status.DecreaseHP(damage);
        if (isDie == true)
        {
            Debug.Log("GameOver");
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}