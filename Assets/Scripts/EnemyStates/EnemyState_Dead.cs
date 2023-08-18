using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Dead : IEnemyState
{
    public IEnemyState DoState(Enemy_Base enemyBase)
    {
        return enemyBase.deadState;
    }
}

