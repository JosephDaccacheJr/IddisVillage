using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AnimationEvents : MonoBehaviour
{
    public Player_Controller playerCon;
    public void AttackMade()
    {
        playerCon.MakeAttack();
    }
}
