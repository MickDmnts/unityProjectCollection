using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dray : MonoBehaviour
{
    public enum eMode { idle,move,attack,transition }

    [Header("Set in inspector")]
    public float speed = 5;
    public float attackDuration = .25f;
    public float attackDelay = .5f;

    [Header("Set dynamically")]
    public int directionHeld = -1;
    public int facing = 1;
    public eMode mode = eMode.idle;

    private float timeAtkDone = 0;
    private float timeAtkNext = 0;
    private Rigidbody rigid;
    private Animator drayAnimator;

    private readonly Vector3[] directions = new Vector3[]
    {
        Vector3.right,Vector3.up,Vector3.left,Vector3.down
    };

    private readonly KeyCode[] keyInputs = new KeyCode[]
    {
            KeyCode.RightArrow,KeyCode.UpArrow,KeyCode.LeftArrow,KeyCode.DownArrow
    };

    //Functions
    private void Awake()
    {
        CacheGameObjectReferences();
    }

    void CacheGameObjectReferences()
    {
        rigid = this.GetComponent<Rigidbody>();
        drayAnimator = this.GetComponent<Animator>();
    }

    private void Update()
    {
        directionHeld = -1;

        CheckMoveKeyInput();

        CheckAttackKeyInput();
        FinishAttackAnimation();

        SetProperEMode();
        ApplyAnimationAndVelocity();
    }

    void CheckMoveKeyInput()
    {
        for (int i = 0; i < 4; i++)
        {
            if (Input.GetKey(keyInputs[i]))
            {
                directionHeld = i;
            }
        }
    }

    void CheckAttackKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Z) && Time.time >= timeAtkNext)
        {
            mode = eMode.attack;
            timeAtkDone = Time.time + attackDuration;
            timeAtkNext = Time.time + attackDelay;
        }
    }

    void FinishAttackAnimation()
    {
        if (Time.time >= timeAtkDone)
        {
            mode = eMode.idle;
        }
    }

    void SetProperEMode()
    {
        if (mode != eMode.attack)
        {
            if (directionHeld == -1)
            {
                mode = eMode.idle;
            }
            else
            {
                facing = directionHeld;
                mode = eMode.move;
            }
        }
    }

    void ApplyAnimationAndVelocity()
    {
        Vector3 vel = Vector3.zero;
        switch (mode)
        {
            case eMode.idle:
                drayAnimator.CrossFade("Dray_Walk_" + facing, 0);
                drayAnimator.speed = 0;
                break;
            case eMode.move:
                vel = directions[directionHeld];
                drayAnimator.CrossFade("Dray_Walk_" + facing, 0);
                drayAnimator.speed = 1;
                break;
            case eMode.attack:
                drayAnimator.CrossFade("Dray_Attack_" + facing, 0);
                drayAnimator.speed = 0;
                break;
        }

        rigid.velocity = vel * speed;
    }
}