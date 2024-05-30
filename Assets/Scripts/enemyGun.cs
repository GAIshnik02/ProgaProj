using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyGun : MonoBehaviour
{
    public float offset;
    public GameObject bullet;
    public Transform shotPoint;
    private float timeBtwShots;
    public float startTimeBtwShots;
    public int maxAmmo; // Максимальное количество патронов в магазине
    private int currentAmmo; // Текущее количество патронов
    public float reloadTime; // Время перезарядки в секундах
    private bool isReloading = false; // Флаг, указывающий на то, что идет перезарядка
    private PlayerController player;

    void Start()
    {
        currentAmmo = maxAmmo; // Инициализация текущего количества патронов
        player = FindObjectOfType<PlayerController>(); // Найти объект игрока в сцене
    }

    void Update()
    {
        if (!player)
            return; // Если игрок не найден, выходим из метода

        // Получаем направление к игроку
        Vector3 direction = (player.transform.position - transform.position).normalized;
        
        // Получаем угол поворота по Z для оружия
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        // Поворачиваем оружие
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

        if (isReloading)
        {
            return; // Если идет перезарядка, то прерываем выполнение стрельбы
        }
        
        if (timeBtwShots <= 0)
        {
            if (currentAmmo > 0)
            {
                Shoot();
                timeBtwShots = startTimeBtwShots;
            }
            else if (currentAmmo <= 0)
            {
                StartCoroutine(Reload());
            }
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }

    void Shoot()
    {
        Instantiate(bullet, shotPoint.position, shotPoint.rotation); 
        currentAmmo--; // Уменьшаем количество патронов после выстрела
    }

    IEnumerator Reload()
    {
        isReloading = true;
        float elapsedTime = 0f;

        while (elapsedTime < reloadTime)
        {
            elapsedTime += 0.5f; 
            yield return new WaitForSeconds(0.5f); 
        }

        currentAmmo = maxAmmo; // Восстанавливаем патроны
        isReloading = false;
        
    }
}
