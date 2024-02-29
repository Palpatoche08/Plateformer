using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Windows.Input;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    private Rigidbody rb;
    private Collider col;

    private Animator animator;

    public TextMeshProUGUI coinCountText;
    private int coinCount = 0;

    public TextMeshProUGUI scoreCountText;
    private int scoreCount = 0;


    public TextMeshProUGUI timerText;
    private float timer = 100f; 

    private bool isGameActive = true;

    private bool isJumping = false;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float BufferDuration = 0.1f;

    public AudioSource dingSound;
    public AudioSource blockSound;

    private float BufferTimer = 0f; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        animator = GetComponent<Animator>();

    }

    void Update()
    {
        if (isGameActive)
        {
            coinCountText.text = "Coins: x" + coinCount;
            scoreCountText.text = "Score: " + scoreCount;
            UpdateTimer();
            Move();
            Jump();
            IsGrounded();
            CheckInteractions();
            Buffering();
            animator.SetBool("IsJumping", isJumping);
            ClampFallSpeed();
        }
    }

    void UpdateTimer()
    {
        timer -= Time.deltaTime;
        timerText.text = "Time: " + Mathf.Ceil(timer);

        if (timer <= 0)
        {
            isGameActive = false;
            Debug.Log("Game Over!");
        }
    }

    void Move()
    {
        float horizontalInput = 0f;

        if (Input.GetKey(KeyCode.Q))
        {
            horizontalInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1f;
        }

        Vector3 movement = new Vector3(horizontalInput * moveSpeed, rb.velocity.y, 0f);
        rb.velocity = movement;
    }

    void Jump()
    {
        if (IsGrounded() && Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void Buffering()
    {
        if (BufferTimer > 0)
        {
            BufferTimer -= Time.deltaTime;
            if (BufferTimer > 0 && IsGrounded())
            {
                PerformJump();
            }
        }

        if (IsGrounded() && Input.GetButtonDown("Jump"))
        {
            BufferTimer = BufferDuration;
        }
    }

    void PerformJump()
    {
        isJumping = true;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        BufferTimer = 0f; 
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, col.bounds.extents.y + 0.1f);
    }

    void ClampFallSpeed()
    {
        if (rb.velocity.y < 0) 
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) 
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    void CheckInteractions()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Brick"))
                {
                    AddScore(100);
                    blockSound.Play(); 
                    Destroy(hit.collider.gameObject);
                }
                else if (hit.collider.CompareTag("Question"))
                {
                    AddCoins(1);
                    dingSound.Play(); 
                }
                
            }
        }
    }

    void AddCoins(int amount)
    {
        coinCount += amount;
        coinCountText.text = "Coins: " + coinCount;
    }

    void AddScore(int amount)
    {
        scoreCount += amount;
        scoreCountText.text = "Score: " + scoreCount;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (IsGrounded())
        {
            isJumping = false;
        }
        
        if (collision.gameObject.CompareTag("Goal") )
        {
            isGameActive = false;
            AddScore(100 * coinCount);
            coinCount = 0;            
            coinCountText.text = "Coins: " + 0;
            Debug.Log("You win !");
        }
        else if (collision.gameObject.CompareTag("Water") || collision.gameObject.CompareTag("Danger"))
        {
            isGameActive = false;
            Debug.Log("Game over!");
        }
    }
}

