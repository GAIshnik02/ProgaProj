using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public float speed;

    private void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        //TODO: сюда добавить движение
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
