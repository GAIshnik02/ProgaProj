using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float speed;
    public float distance;
    public int damage;
    public LayerMask whatIsSolid;
    public float lifetime = 5f;
    public GameObject deathEffect;

    private float aliveTime = 0f;

    private void Update()
    {
        aliveTime += Time.deltaTime;

        if (aliveTime >= lifetime)
        {
            Destroy(gameObject);
            return;
        }

        transform.Translate(Vector2.up * speed * Time.deltaTime);

        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, transform.up, distance, whatIsSolid);
        if (hitinfo.collider != null)
        {
            if (hitinfo.collider.CompareTag("Enemy"))
            {
                hitinfo.collider.GetComponent<Enemy>().TakeDamage(damage);
            }
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}