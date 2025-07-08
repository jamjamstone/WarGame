using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticField
{
    public static int MaxCardCost = 5;
    public static int MaxUnitCost = 5;
    public static int startMoney = 500;
    public static float maximumBattleTime = 50f;
    public static float maximumBuyTime = 200f;
    public static float maxPlayerHp = 1000;

    public static int speedModifieValue = 2;

    public readonly static int hashIdle = Animator.StringToHash("isIdle");
    public readonly static int hashDead = Animator.StringToHash("isDead");
    public readonly static int hashMove = Animator.StringToHash("isMove");
    public readonly static int hashAttack = Animator.StringToHash("isAttack");
}



