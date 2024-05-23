using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb; 
    private Vector2 moveInput; // направление движения
    private Vector2 moveVelocity; // итоговая скорость

    private Animator animator; // ссылка на компонент Animator
    private bool facingRight = true; // отслеживание направления игрока

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // инициализация компонента Animator
    }
    
    // Update вызывается один раз за кадр
    void Update()
    {
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
}