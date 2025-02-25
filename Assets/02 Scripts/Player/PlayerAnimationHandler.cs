using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    public static readonly int IsInvincibleKey = Animator.StringToHash("IsInvincible");
    public static readonly int IsJump1KeY = Animator.StringToHash("IsJump1");
    public static readonly int IsJump2Key = Animator.StringToHash("IsJump2");
    public static readonly int IsGroundKey = Animator.StringToHash("IsGround");
    public static readonly int IsSlideKey = Animator.StringToHash("IsSlide");

    public bool IsGround
    { 
        get => playerAnimator.GetBool(IsGroundKey);
        set => playerAnimator.SetBool(IsGroundKey, value);
    }

    public bool IsJump1
    {
        get => playerAnimator.GetBool(IsJump1KeY);
        set => playerAnimator.SetBool(IsJump1KeY, value);
    }

    public bool IsJump2
    {
        get => playerAnimator.GetBool(IsJump2Key);
        set => playerAnimator.SetBool(IsJump2Key, value);
    }

    public bool IsInvincible
    {
        get => playerAnimator.GetBool(IsInvincibleKey);
        set => playerAnimator.SetBool(IsInvincibleKey, value);
    }
    public bool IsSlide
    {
        get => playerAnimator.GetBool(IsSlideKey);
        set => playerAnimator.SetBool(IsSlideKey, value);
    }

    public Animator playerAnimator;
    private float alpha = 0;
    private bool isCoroutine = false;
    private SpriteRenderer _renderer;

    private void Awake()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
    }

    //private void Update()
    //{
    //    if (IsInvincible && isCoroutine == false)
    //    {
    //        isCoroutine = true;
    //        alpha = 1;
    //        StartCoroutine(nameof(InvinciblePlayer));
    //    }
    //    else if (IsInvincible == false)
    //    {
    //        isCoroutine = false;
    //    }
    //    alpha = Mathf.PingPong(alpha, 0.8f) + 0.2f;
    //}

    //IEnumerator InvinciblePlayer()
    //{
    //    while (isCoroutine)
    //    {
    //        _renderer.color = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, alpha);
    //    }
    //    yield break;
    //}


    //플레이어가 땅에 닿고 있을 때랑 닿지 않을 때를 비교해서 파라미터를 변경
    public void PlayerIsGround(bool isGround)
    {
        if (isGround)
        {
            IsGround = true;
            IsJump1 = false;
            IsJump2 = false;
        }
        else
        {
            IsGround = false;
            IsJump1 = true;
        }
    }
}
