using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public float speed;
    public GameObject hitEffect;

    private void Update()
    {
        if (health <= 0)
        {
            // Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        //TODO: сюда добавить движение
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
