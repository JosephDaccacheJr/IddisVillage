using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AttackZone : MonoBehaviour
{
    public int damageStrength;
    bool _hasHit;


    void Start()
    {
        Destroy(gameObject, 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if(!_hasHit)
            {
                _hasHit = true;
                GameManager.instance.changeHealth(-damageStrength);
            }  
        }
    }
}
