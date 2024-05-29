using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    public float health;
    public float speed;
    public GameObject hitEffect;
    private float timeBtwAttack;
    private PlayerController player;
    private Animator animator; // ссылка на компонент Animator
    private bool facingRight = true; // отслеживание направления игрока

    private Vector2 lastPosition;
    private bool wasMovingRight = true; // Флаг для отслеживания направления движения

    private void Start()
    {
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();

        if (player == null)
        {
            Debug.LogError("PlayerController not found in the scene.");
        }

        lastPosition = transform.position;
    }

    private void Update()
    {
        if (player == null)
        {
            return;
        }

        if (health <= 0)
        {
            Die();
        }
        else
        {
            MoveTowardsPlayer();
        }

        UpdateAnimatorParameters();
    }

    private void MoveTowardsPlayer()
    {
        Vector2 targetPosition = player.transform.position;

        // Проверяем, есть ли препятствие между врагом и игроком
        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetPosition - (Vector2)transform.position, Vector2.Distance(transform.position, targetPosition));

        if (hit.collider != null && hit.collider.CompareTag("block")) // Проверяем, является ли препятствие стеной
        {
            // Если есть препятствие, изменяем направление движения
            targetPosition = (Vector2)transform.position - (targetPosition - (Vector2)transform.position).normalized;
        }

        // Продолжаем двигаться к цели
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    private void UpdateAnimatorParameters()
    {
        Vector2 currentPosition = transform.position;
        Vector2 movement = currentPosition - lastPosition;

        // Raw movement values (скорость движения по осям)
        float moveX = movement.x / Time.deltaTime;
        float moveY = movement.y / Time.deltaTime;
        bool isMoving = movement.sqrMagnitude > 0.01f;

        // if (moveX > 0.1f)
        // {
        //     moveX = 1;
        // } else if (moveX < 0.1f)
        // {
        //     moveX = -1;
        // }
        //

        // Update animator parameters
        animator.SetFloat("MoveX", moveX);
        animator.SetFloat("MoveY", moveY);
        animator.SetBool("IsMoving", isMoving);

        //TODO: БАВДЛЫБВЛЭЫЖДв СДЕЛАТЬ ЧТО_ТО С ЭТОЙ ХУЙНЕЙ
        // Проверка изменения направления движения
        if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y)) // Проверяем, движется ли враг больше по оси X, чем по Y
        {
            if (movement.x > 0)
            {
                Flip();
                wasMovingRight = true;
            }
            else if (movement.x < 0)
            {
                Flip();
                wasMovingRight = false;
            }
        }

        lastPosition = currentPosition;
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Добавьте любую анимацию или эффекты смерти здесь
        Destroy(gameObject);
    }
}
