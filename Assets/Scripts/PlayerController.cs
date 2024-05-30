using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float health;
    public Rigidbody2D rb; 
    public Vector2 moveInput; 
    public Vector2 moveVelocity; 
    public GameObject potionEffect;
    public GameObject shield;
    private Animator animator;
    public bool facingRight = true;
    public TMP_Text hp_text;
    public ShieldTimer shieldTimer;
    public GameObject shieldEffect;
    public GameObject panel;
    public GameObject panel2;
    public static int RoomsCleared = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (health <= 0)
        {
            panel.SetActive(true);
            Time.timeScale = 0f;
        }
        else if (RoomsCleared == 15)
        {
            panel2.SetActive(true);
            Time.timeScale = 0f;
            RoomsCleared = 0;
        }

        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized * speed;

        if (moveInput.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInput.x < 0 && facingRight)
        {
            Flip();
        }

        animator.SetFloat("MoveX", moveInput.x);
        animator.SetFloat("MoveY", moveInput.y);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        
        
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);        
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Potion"))
        {
            Instantiate(potionEffect, other.transform.position, Quaternion.identity);
            ChangeHealth(50);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Shield"))
        {
            if (!shield.activeInHierarchy)
            {
                shield.SetActive(true);
                Instantiate(shieldEffect, other.transform.position, Quaternion.identity);
                Destroy(other.gameObject);
                shieldTimer.gameObject.SetActive(true);
                shieldTimer.isCooldown = true;
            }
            else
            {
                shieldTimer.ResetTimer();
                Instantiate(shieldEffect, other.transform.position, Quaternion.identity);
                Destroy(other.gameObject);
            }
        }
    }

    public void ChangeHealth(float healthValue)
    {
        if (!shield.activeInHierarchy || (shield.activeInHierarchy && healthValue > 0))
        {
            health += healthValue;
            hp_text.text = "HP: " + health;
        }
        else if (shield.activeInHierarchy && healthValue < 0)
        {
            shieldTimer.ReduceTime(healthValue);
        }
    }

    public void UpdateClearRooms()
    {
        RoomsCleared++;
        
    }
}