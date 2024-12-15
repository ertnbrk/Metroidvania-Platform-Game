using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   //Bunu dahil etmemiz laz�m

public class UIController : MonoBehaviour
{
    //Bunu bir Instance yap�yoruz healthcontrollea yapt���m�z gibi
    public static UIController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;    //playerHealthController'� single a �eviriyoruz bu sayade getcomponent demeden instance'e eri�ebiliyoruz ve instance �zerinden t�m fonksiyonlara.

            DontDestroyOnLoad(gameObject);  //yeni bir sahne olu�turdu�umuzda veya mevcut sahneyi yeniledi�imizde bu objeyi yok etme diyoruz.��nk� �l�nce otamatik tekrar olu�turuyodu.
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public Slider healthSlider;

    public Image fadeScreen;
    public float fadeSpeed = 2f;
    private bool fadingToBlack, fadingFromBlack;
    public void UpdateHealth(int currentHealth,int maxHealth)
    {
        //slider�m�z�n max ve mevcut canlar�n� set ediyoruz
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }


    public void StartFadeToBlack()
    {
        fadingToBlack = true;
        fadingFromBlack = false;
    }
    public void StartFadeFromBlack()
    {
        fadingFromBlack=true;
        fadingToBlack=false;
    }


    void Start()
    {
        
    }

    
    void Update()
    {
        //FadingCheck
        if (fadingToBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r,fadeScreen.color.g,fadeScreen.color.b,Mathf.MoveTowards(fadeScreen.color.a,1f,fadeSpeed * Time.deltaTime));


            if (fadeScreen.color.a == 1f)
            {
                fadingToBlack = false;
            }

        }
        else if (fadingFromBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));

            if (fadeScreen.color.a == 0f)
            {
                fadingFromBlack = false;
            }
        }

    }





}
