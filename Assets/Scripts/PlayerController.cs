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

    public TextMeshProUGUI coinCountText;
    private int coinCount = 0;

    public TextMeshProUGUI timerText;
    private float timer = 300f; 

    private bool isGameActive = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    void Update()
    {
        if (isGameActive)
        {
            coinCountText.text = "Coins: x" + coinCount;
            UpdateTimer();
            Move();
            Jump();
            IsGrounded();
            CheckInteractions();
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
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput * moveSpeed, rb.velocity.y, verticalInput * moveSpeed);
        rb.velocity = movement;
    }

    void Jump()
    {
        if (IsGrounded() && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, col.bounds.extents.y + 0.1f);
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
                    // If the hit object is a brick, destroy it
                    Destroy(hit.collider.gameObject);
                }
                else if (hit.collider.CompareTag("Question"))
                {
                    // If the hit object is a question block, add coins to the UI
                    AddCoins(1); // You can adjust the number of coins as needed
                }
            }
        }
    }

    void AddCoins(int amount)
    {
        coinCount += amount;
        coinCountText.text = "Coins: " + coinCount;
    }
}

