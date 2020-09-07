using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingDemonHealth : EnemyHealth
{

    protected override void Death()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
