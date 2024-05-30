using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShieldTimer : MonoBehaviour
{

    public float cooldown;

    [HideInInspector] public bool isCooldown;

    private Image shielImage;
    private PlayerController player;
    
    void Start()
    {
        shielImage = GetComponent<Image>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        isCooldown = true;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (isCooldown)
        {
            shielImage.fillAmount -= 1 / cooldown * Time.deltaTime;
            if (shielImage.fillAmount <= 0)
            {
                shielImage.fillAmount = 1;
                isCooldown = false;
                player.shield.SetActive(false);
                gameObject.SetActive(false);
            }
        }
        
    }

    public void ResetTimer()
    {
        shielImage.fillAmount = 1;
    }

    public void ReduceTime(float damage)
    {
        shielImage.fillAmount += damage / 30f;
    }
}
