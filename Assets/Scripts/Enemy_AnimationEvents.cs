using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AnimationEvents : MonoBehaviour
{
    public Enemy_Base enemyBase;
    public void Attack()
    {
        enemyBase.MakeAttack();
    }
}
