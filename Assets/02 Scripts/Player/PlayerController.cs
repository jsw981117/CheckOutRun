using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player player;
    private StatHandler statHandler;
    private DataManager dataManager;
    public AudioClip hitSFX, pickupCoinSFX;
    private PlayerAnimationHandler playerAnimationHandler;
    private GameObject absorber;
    public bool isFlap = false;
    public bool isSlide = false;
    private bool isGrounded = false;
    private bool wasGrounded = false;

    private void Awake()
    {
        player = GetComponent<Player>();
        statHandler = GetComponent<StatHandler>();
        playerAnimationHandler = GetComponent<PlayerAnimationHandler>();
        absorber = transform.Find("Absorber")?.gameObject;
        if (absorber != null)
        {
            absorber.SetActive(false); // 시작 시 비활성화
        }
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
        else
        {
            isSlide = false; // 쉬프트가 눌리지 않으면 슬라이드 해제
        }

    }

    void FixedUpdate()
    {
        if (player == null) return;

        Move();
        HandleJump();
        HandleSlide();
    }

    private void Move()
    {
        if (player.rigid == null) return;

        player.rigid.velocity = new Vector2(player.playerSpeed, player.rigid.velocity.y);
        Vector3 temp = transform.position - new Vector3(0.3f, 0, 0);
        isGrounded = Physics2D.Raycast(temp, Vector2.down * 0.9f, 1f, LayerMask.GetMask("Ground"));
        Debug.DrawRay(transform.position, Vector2.down * 0.8f, Color.green);
        if (isGrounded && !wasGrounded)
        {
            isFlap = false;
        }

        playerAnimationHandler.PlayerIsGround(isGrounded);

        wasGrounded = isGrounded;
    }

    public void HandleJump()
    {
        if (!isFlap || playerAnimationHandler.IsJump2 == true) return; // 점프 횟수 초과 시 실행 방지

        Jump();
        isFlap = false;
    }

    private void Jump()
    {
        if (player.rigid != null)
        {
            if (playerAnimationHandler.IsJump1) playerAnimationHandler.IsJump2 = true;
            player.rigid.velocity = new Vector2(player.rigid.velocity.x, player.jumpForce);
        }
    }

    public void HandleSlide()
    {
        if (isFlap || !isGrounded) return;
        if (isSlide) Slide();
        else ResetSlide();
    }

    private void Slide()
    {
        if (player.coll != null)
        {
            playerAnimationHandler.IsSlide = true;
            player.coll.size = new Vector2(player.originalColliderSize.x, player.originalColliderSize.y - 1f);
            player.coll.offset = new Vector2(player.originalColliderOffset.x, player.originalColliderOffset.y - 0.5f);
        }
    }

    private void ResetSlide()
    {
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
        if (collision == null)
        {
            return;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            if (collision.GetComponent<IItem>().GetType() == typeof(PotionItem))
            {
                PotionItem item = collision.gameObject.GetComponent<PotionItem>();
                if (item != null)
                {
                    statHandler.Heal(item);
                    item.OnCollisionEffect();
                }
            }

            if (collision.GetComponent<IItem>().GetType() == typeof(SpeedItem))
            {
                SpeedItem item = collision.gameObject.GetComponent<SpeedItem>();
                if (item != null)
                {
                    statHandler.ChangeSpeed(item);
                    item.OnCollisionEffect();
                }
            }

            if (collision.GetComponent<IItem>().GetType() == typeof(CoinItem))
            {
                CoinItem item = collision.gameObject.GetComponent<CoinItem>();
                if (item != null)
                {
                    dataManager.AddScore(item.CoinScore);
                    item.OnCollisionEffect();
                }
            }

            if(collision.GetComponent<IItem>().GetType() == typeof(MagnetItem))
            {
                MagnetItem item = collision.gameObject.GetComponent<MagnetItem>();
                if(item != null)
                {
                    MagnetEffect(5f);
                    item.OnCollisionEffect();
                }

            }
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.sfxManager.PlaySFX(hitSFX, 0.5f);
            }
            statHandler.Damage(20, collision);
        }

    }
    private void MagnetEffect(float duration)
    {
        if (absorber == null) return;

        absorber.SetActive(true); 
        StartCoroutine(DisableMagnetEffect(duration));
    }
    private IEnumerator DisableMagnetEffect(float delay)
    {
        yield return new WaitForSeconds(delay);
        absorber.SetActive(false);
    }
}