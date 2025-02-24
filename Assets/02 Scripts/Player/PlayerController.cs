using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player player;
    private ButtonHandler buttonHandler;
    private StatHandler statHandler;
    private DataManager dataManager;
    public bool isFlap = false;
    public bool isSlide = false;
    private bool isGrounded = false;

    void Start()
    {
        player = GetComponent<Player>();
        buttonHandler = FindObjectOfType<ButtonHandler>();
        statHandler = GetComponent<StatHandler>();
        dataManager = DataManager.Instance;
    }

    void Update()
    {
        if (player == null) return;

        HandleJump();
        if (!isFlap) HandleSlide();
    }

    void FixedUpdate()
    {
        if (player == null) return;
        Move();
    }

    private void Move()
    {
        if (player.rigid == null) return;

        player.rigid.velocity = new Vector2(player.playerSpeed, player.rigid.velocity.y);
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1.2f, LayerMask.GetMask("Ground"));

        if (isGrounded)
        {
            player.jumpCount = 0;
            isFlap = false;
        }
       
    }

    public void HandleJump()
    {
        if (!isFlap) return; 
        if (player.jumpCount >= player.maxJumpCount) return; // 점프 횟수 초과 시 실행 방지
        player.jumpCount++;
        Jump();
        isFlap = false;
    }

    private void Jump()
    {
        if (player.rigid != null)
        {
            player.rigid.velocity = new Vector2(player.rigid.velocity.x, player.jumpForce);
            player.jumpCount++;
        }
    }

    public void HandleSlide()
    {
        if (isFlap) return;
        if (isSlide) Slide();
        else ResetSlide();
    }

    private void Slide()
    {
        if (player.coll != null)
        {
            player.coll.size = new Vector2(player.originalColliderSize.x, player.originalColliderSize.y * 0.5f);
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
    }

    private void ResetSlide()
    {
        if (player.coll != null)
        {
            player.coll.size = player.originalColliderSize;
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
            if (collision.GetType() == typeof(PotionItem))
            {
                PotionItem item = collision.gameObject.GetComponent<PotionItem>();
                if (item != null)
                {
                    statHandler.Heal(item);
                }
                //item.OnCollisionEffect();
            }

            if (collision.GetType() == typeof(SpeedItem))
            {
                SpeedItem item = collision.gameObject.GetComponent<SpeedItem>();
                if (item != null)
                {
                    statHandler.ChangeSpeed(item);
                }
                //item.OnCollisionEffect();
            }

            if (collision.GetType() == typeof(CoinItem))
            {
                CoinItem item = collision.gameObject.GetComponent<CoinItem>();
                if (item != null)
                {
                    dataManager.AddScore(item.CoinScore);
                }
                //item.OnCollisionEffect();
            }
        }
    }
}