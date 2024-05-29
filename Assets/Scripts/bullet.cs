using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float distance;
    public int damage;
    public LayerMask whatIsSolid;
    public float lifetime;
    public GameObject deathEffect;

    private float aliveTime = 0f;

    [SerializeField] private bool enemyBullet;

    private void Update()
    {
        aliveTime += Time.deltaTime;

        if (aliveTime >= lifetime)
        {
            DestroyBullet();
            return;
        }

        transform.Translate(Vector2.up * speed * Time.deltaTime);

        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, transform.up, distance, whatIsSolid);
        if (hitinfo.collider != null)
        {
            if (hitinfo.collider.CompareTag("Enemy") && !enemyBullet)
            {
                hitinfo.collider.GetComponent<Enemy>().TakeDamage(damage);
            }
            else if (hitinfo.collider.CompareTag("Player") && enemyBullet)
            {
                hitinfo.collider.GetComponent<PlayerController>().ChangeHealth(-damage);
            }
            DestroyBullet();
        }
    }

    private void DestroyBullet()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}