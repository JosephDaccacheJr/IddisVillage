using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AttackZone : MonoBehaviour
{
    List<GameObject> _hitTargets = new List<GameObject>();
    void Start()
    {
        Destroy(gameObject, 0.5f);
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (!_hitTargets.Contains(other.gameObject))
            {
                other.GetComponent<Enemy_Base>().DoDamage(GameManager.instance.playerDamage);
                _hitTargets.Add(other.gameObject);
            }
        }
    }
}
