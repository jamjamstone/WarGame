using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticField
{
    public static int MaxCardCost = 5;
    public static int MaxUnitCost = 5;
    public readonly static int hashIdle = Animator.StringToHash("isIdle");
    public readonly static int hashDead = Animator.StringToHash("isDead");
    public readonly static int hashMove = Animator.StringToHash("isMove");
    public readonly static int hashAttack = Animator.StringToHash("isAttack");
}



