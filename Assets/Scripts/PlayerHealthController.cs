using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{

    [HideInInspector]   //currenthelth public olsun çünkü daha sonra bir yerden ulaþmam gerekebilir ama inspectorda gözükmesin
    public int currentHealth;
    public int maxHealth;

    public GameObject deathEffect;

    public SpriteRenderer[] playerSprite;
    public float invincibilityLength;
    private float invincCounter;
    public float flashLength;
    private float flashCounter;


    public static PlayerHealthController instance;
    
    private void Awake()
    {
        //instance boþ mu diye bakýcaz çünkü sahne yenilendiðinde yenisiini oluþturmuyoruz var olaný hayatta tutuyoruz bu yüzden eðer instance yoksa oluþturmasý için.
        //Aksi durumda birden fazla karakterimiz olmuþ olurdu.
        if (instance == null) { 
        instance = this;    //playerHealthController'ý single a çeviriyoruz bu sayade getcomponent demeden instance'e eriþebiliyoruz ve instance üzerinden tüm fonksiyonlara.

        DontDestroyOnLoad(gameObject);  //yeni bir sahne oluþturduðumuzda veya mevcut sahneyi yenilediðimizde bu objeyi yok etme diyoruz.Çünkü ölünce otamatik tekrar oluþturuyodu.
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        UIController.instance.UpdateHealth(currentHealth, maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        //damage alýnca bir süre damage almamýzý engelliyicek karakterimizi bir görünür bir görünmez yapýcak
        if (invincCounter > 0)
        {
            invincCounter -= Time.deltaTime;

            flashCounter -= Time.deltaTime;
            if (flashCounter <= 0)
            {
                foreach (SpriteRenderer item in playerSprite)
                {
                    item.enabled = !item.enabled;
                }
                flashCounter = flashLength;
            }
            if (invincCounter <= 0)
            {
                foreach (SpriteRenderer item in playerSprite)
                {
                    item.enabled = true;
                }
                flashCounter = 0f;
            }
        }   
    }


    public void DamagePlayer(int damageAmount)
    {

        if (invincCounter <= 0)
        {



            currentHealth -= damageAmount;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                RespawnController.instance.Respawn();
                if (deathEffect != null)
                {
                    Instantiate(deathEffect, transform.position, transform.rotation);
                }

                gameObject.SetActive(false);
            }
            else //Damage aldýðýmýzda karakterimiz yok olup geri gözüksün ki hasar aldýðýmýz anlaþýlsýn
            {
                invincCounter = invincibilityLength;
            }
            UIController.instance.UpdateHealth(currentHealth, maxHealth);
        }
    }
    /// <summary>
    /// Can barýmýz geri dolsun
    /// </summary>
    public void fillHealth()
    {
        currentHealth = maxHealth;
        UIController.instance.UpdateHealth(currentHealth, maxHealth);
    }


    public void HealthUp(int healthAmount)
    {

        if ((currentHealth + healthAmount) <= maxHealth)
        {
            currentHealth += healthAmount;
            
        }
        else
        {
            currentHealth = maxHealth;
        }
        UIController.instance.UpdateHealth(currentHealth, maxHealth);
    }
}

