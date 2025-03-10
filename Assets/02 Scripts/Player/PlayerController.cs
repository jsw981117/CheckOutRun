using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player player;
    private StatHandler statHandler;
    private DataManager dataManager;
    private PlayerAnimationHandler playerAnimationHandler;
    public bool isFlap = false; // 점프 
    public bool isSlide = false; // 슬라이드
    private bool isGrounded = false; // 플랫폼 오브젝트에

    private void Awake()
    {
        player = GetComponent<Player>();
        statHandler = GetComponent<StatHandler>();
        playerAnimationHandler = GetComponent<PlayerAnimationHandler>();  
    }

    void Start()
    {
        dataManager = DataManager.Instance;
        dataManager.Init();
    }



    void Update()
    {
        if (player == null) return;

        if (!isSlide&&Input.GetKeyDown(KeyCode.Space)) // 스페이스바로 점프
        {
            isFlap = true;
            isSlide = false;
        }
        else if (playerAnimationHandler.IsJump1 == false && (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift)))
        {
            isSlide = true;
        }

        if(Input.GetKeyUp(KeyCode.RightShift) || Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSlide = false;
        }

    }

    void FixedUpdate()
    {
        if (player == null && GameManager.Instance.isWin) return;

        Move();
        HandleJump();
        HandleSlide();
    }

    private void Move() // 플레이어가 X축으로 쭉 이동하도록 구현
    {
        if (player.rigid == null) return;

        player.rigid.velocity = new Vector2(player.playerSpeed, player.rigid.velocity.y);
        Vector3 temp = transform.position - new Vector3(0.3f, 0, 0);
        isGrounded = Physics2D.Raycast(temp, Vector2.down * 0.9f, 1f, LayerMask.GetMask("Ground"));
        Debug.DrawRay(transform.position, Vector2.down * 0.8f, Color.green);

        if(isGrounded == false)
        {
            isSlide = false;
        }

        playerAnimationHandler.PlayerIsGround(isGrounded);
    }

    public void HandleJump() 
    {
        // 버튼 입력 또는 키보드 입력 감지 되면 해당 함수 호출
        if (!isFlap || playerAnimationHandler.IsJump2 == true) 
        { 
            isFlap = false; 
            return; 
        } 

        Jump();
        isFlap = false;
    }

    private void Jump()
    {
        // 점프 동작 수행
        if (player.rigid != null)
        {
            if (playerAnimationHandler.IsJump1) playerAnimationHandler.IsJump2 = true;
            player.rigid.velocity = new Vector2(player.rigid.velocity.x, player.jumpForce);

            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.sfxManager.PlaySFX(SoundLibrary.Instance.sfxJump, 0.5f);
            }
        }
    }

    public void HandleSlide()
    {
        // 버튼 입력 또는 키보드 입력 감지 되면 해당 함수 호출
        if (isFlap || !isGrounded) return;
        if (isSlide) Slide();
        else ResetSlide();
    }

    private void Slide()
    {
        // 슬라이드 동작 수행
        if (player.coll != null)
        {
            playerAnimationHandler.IsSlide = true;
            player.coll.size = new Vector2(player.originalColliderSize.x, player.originalColliderSize.y - 1f);
            player.coll.offset = new Vector2(player.originalColliderOffset.x, player.originalColliderOffset.y - 0.5f);
        }
    }

    private void ResetSlide()
    {
        // 슬라이드 해제 및 원래 상태로 복구
        if (player.coll != null)
        {
            playerAnimationHandler.IsSlide = false;
            player.coll.size = player.originalColliderSize;
            player.coll.offset = player.originalColliderOffset;
            transform.rotation = Quaternion.identity;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 아이템 충독 감지 및 처리
        if (collision == null)
        {
            return;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            if (collision.GetComponent<IItem>()?.GetType() == typeof(PotionItem))
            {
                PotionItem item = collision.gameObject.GetComponent<PotionItem>();
                if (item != null)
                {
                    statHandler.Heal(item);
                    item.OnCollisionEffect();
                }
            }
            else if (collision.GetComponent<IItem>()?.GetType() == typeof(SpeedItem))
            {
                SpeedItem item = collision.gameObject.GetComponent<SpeedItem>();
                if (item != null)
                {
                    statHandler.ChangeSpeed(item);
                    item.OnCollisionEffect();
                }
            }
            else if (collision.GetComponentInParent<IItem>().GetType() == typeof(MagnetItem))
            {
                MagnetItem item = collision.gameObject.GetComponentInParent<MagnetItem>();
                if (item != null)
                {
                    item.OnCollisionEffect();
                }
            }
            else if (collision.GetComponent<IItem>()?.GetType() == typeof(StarItem))
            {
                StarItem item = collision.gameObject.GetComponent<StarItem>();
                if (item != null)
                {
                    item.OnCollisionEffect();
                }
            }

        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.sfxManager.PlaySFX(SoundLibrary.Instance.sfxHit, 0.5f);
            }
            statHandler.Damage(10, collision);
        }

    }

}