using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class gun : MonoBehaviour
{
    public float offset;
    public Camera mainCamera; // Переменная для хранения ссылки на камеру
    public GameObject bullet;
    public Transform shotPoint;
    private float timeBtwShots;
    public float startTimeBtwShots;
    public int maxAmmo; // Максимальное количество патронов в магазине
    private int currentAmmo; // Текущее количество патронов
    public float reloadTime; // Время перезарядки в секундах
    private bool isReloading = false; // Флаг, указывающий на то, что идет перезарядка

    public TMP_Text ammoText; // Ссылка на UI текстовый элемент для отображения количества патронов

    void Start()
    {
        currentAmmo = maxAmmo; // Инициализация текущего количества патронов
        UpdateAmmoUI(); // Обновление UI элемента при старте
    }

    void Update()
    {
        RotateGun();

        if (isReloading)
        {
            return; // Если идет перезарядка, то прерываем выполнение стрельбы
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }

        if (timeBtwShots <= 0)
        {
            if (Input.GetMouseButton(0) && currentAmmo > 0)
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

    void RotateGun()
    {
        if(mainCamera != null) // Проверка, что переменная не пуста
        {
            Vector3 difference = mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);
        }
    }

    void Shoot()
    {
        Instantiate(bullet, shotPoint.position, transform.rotation);
        currentAmmo--; // Уменьшаем количество патронов после выстрела
        UpdateAmmoUI(); // Обновление UI элемента после выстрела
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        float elapsedTime = 0f;
        int dotCount = 0;

        while (elapsedTime < reloadTime)
        {
            ammoText.text = "Reloading" + new string('.', dotCount + 1); 
            dotCount = (dotCount + 1) % 3; 
            elapsedTime += 0.5f; 
            yield return new WaitForSeconds(0.5f); 
        }

        currentAmmo = maxAmmo; // Восстанавливаем патроны
        isReloading = false;
        UpdateAmmoUI(); // Обновление UI элемента после перезарядки
    }

    void UpdateAmmoUI()
    {
        ammoText.text = "Ammo: " + currentAmmo + "/" + maxAmmo;
    }
}