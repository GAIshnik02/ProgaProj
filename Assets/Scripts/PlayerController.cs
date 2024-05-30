using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float health;
    public Rigidbody2D rb; 
    public Vector2 moveInput; // направление движения
    public Vector2 moveVelocity; // итоговая скорость
    public GameObject potionEffect;
    public GameObject shield;
    private Animator animator; // ссылка на компонент Animator
    public bool facingRight = true; // отслеживание направления игрока
    public TMP_Text hp_text;
    public ShieldTimer shieldTimer;
    public GameObject shieldEffect;
    public GameObject panel;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // инициализация компонента Animator
        
    }
    
    // Update вызывается один раз за кадр
    void Update()
    {
        if (health <= 0)
        {
            panel.SetActive(true);
            // Destroy(gameObject);
        }
        
        
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); // считываем верт и гор движения
        moveVelocity = moveInput.normalized * speed;
        
        // Проверка направления движения по горизонтали
        if (moveInput.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInput.x < 0 && facingRight)
        {
            Flip();
        }

        animator.SetFloat("MoveX", moveInput.x); // обновляем направление по X
        animator.SetFloat("MoveY", moveInput.y); // обновляем направление по Y
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
        if (!shield.activeInHierarchy || shield.activeInHierarchy && healthValue > 0)
        {
            health += healthValue;
            hp_text.text = "HP: " + health;
        }
        else if (shield.activeInHierarchy && healthValue < -0)
        {
            shieldTimer.ReduceTime(healthValue);
        }
    }
}