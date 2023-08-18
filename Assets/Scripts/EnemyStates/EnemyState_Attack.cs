using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Attack : IEnemyState
{
    public IEnemyState DoState(Enemy_Base enemyBase)
    {
        enemyBase.Movement();
        enemyBase.AttackController();


        return enemyBase.attackState;
    }
}
