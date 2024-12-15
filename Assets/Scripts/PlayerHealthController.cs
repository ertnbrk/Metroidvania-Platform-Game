using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{

    [HideInInspector]   //currenthelth public olsun ��nk� daha sonra bir yerden ula�mam gerekebilir ama inspectorda g�z�kmesin
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
        //instance bo� mu diye bak�caz ��nk� sahne yenilendi�inde yenisiini olu�turmuyoruz var olan� hayatta tutuyoruz bu y�zden e�er instance yoksa olu�turmas� i�in.
        //Aksi durumda birden fazla karakterimiz olmu� olurdu.
        if (instance == null) { 
        instance = this;    //playerHealthController'� single a �eviriyoruz bu sayade getcomponent demeden instance'e eri�ebiliyoruz ve instance �zerinden t�m fonksiyonlara.

        DontDestroyOnLoad(gameObject);  //yeni bir sahne olu�turdu�umuzda veya mevcut sahneyi yeniledi�imizde bu objeyi yok etme diyoruz.��nk� �l�nce otamatik tekrar olu�turuyodu.
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
        //damage al�nca bir s�re damage almam�z� engelliyicek karakterimizi bir g�r�n�r bir g�r�nmez yap�cak
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
            else //Damage ald���m�zda karakterimiz yok olup geri g�z�ks�n ki hasar ald���m�z anla��ls�n
            {
                invincCounter = invincibilityLength;
            }
            UIController.instance.UpdateHealth(currentHealth, maxHealth);
        }
    }
    /// <summary>
    /// Can bar�m�z geri dolsun
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

