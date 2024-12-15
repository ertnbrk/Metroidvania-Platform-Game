using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   //Bunu dahil etmemiz lazým

public class UIController : MonoBehaviour
{
    //Bunu bir Instance yapýyoruz healthcontrollea yaptýðýmýz gibi
    public static UIController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;    //playerHealthController'ý single a çeviriyoruz bu sayade getcomponent demeden instance'e eriþebiliyoruz ve instance üzerinden tüm fonksiyonlara.

            DontDestroyOnLoad(gameObject);  //yeni bir sahne oluþturduðumuzda veya mevcut sahneyi yenilediðimizde bu objeyi yok etme diyoruz.Çünkü ölünce otamatik tekrar oluþturuyodu.
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
        //sliderýmýzýn max ve mevcut canlarýný set ediyoruz
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
