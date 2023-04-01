using UnityEngine;
using System.Collections;

public class EnemyZig : Enemy
{
    public override void Move()
    {
        Vector3 tempPos = Pos;
        tempPos.x = Mathf.Sin(Time.time * Mathf.PI * 2) * 4;
        Pos = tempPos;
        base.Move(); //Calls the superclass Move()
    }
}
