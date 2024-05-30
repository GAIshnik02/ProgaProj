using System;
using UnityEngine;
using UnityEngine.AI;

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
    private NavMeshAgent agent;
    private Vector2 lastPosition;
    private bool wasMovingRight = true; // Флаг для отслеживания направления движения
    public RoomTrigger room;
    public static int enemyKilled = 0; // статическая переменная для подсчета убитых врагов

    private void Start()
    {
        room = GetComponentInParent<RoomTrigger>();
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

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
        
        MoveTowardsPlayer();
        UpdateAnimatorParameters();
    }

    private void MoveTowardsPlayer()
    {
        agent.SetDestination(player.transform.position);
    }

    private void UpdateAnimatorParameters()
    {
        Vector2 currentPosition = transform.position;
        Vector2 movement = currentPosition - lastPosition;

        // Raw movement values (скорость движения по осям)
        float moveX = movement.x / Time.deltaTime;
        float moveY = movement.y / Time.deltaTime;
        bool isMoving = movement.sqrMagnitude > 0.01f;
        
        animator.SetFloat("MoveX", moveX);
        animator.SetFloat("MoveY", moveY);
        animator.SetBool("IsMoving", isMoving);

        // Проверка изменения направления движения
        if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y)) // Проверяем, движется ли враг больше по оси X, чем по Y
        {
            if (movement.x > 0 && !wasMovingRight)
            {
                Flip();
                wasMovingRight = true;
            }
            else if (movement.x < 0 && wasMovingRight)
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
            enemyKilled++;
            Destroy(gameObject);
            room.enemies.Remove(gameObject);
        }
    }
}
