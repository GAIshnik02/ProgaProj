using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBullet : MonoBehaviour
{
    public float speed;
    public float distance;
    public int damage;
    public LayerMask whatIsSolid;
    public float lifetime;
    public GameObject deathEffect;

    private float aliveTime = 0f;
    
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
            if (hitinfo.collider.CompareTag("Player"))
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