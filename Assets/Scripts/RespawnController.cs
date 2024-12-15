using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RespawnController : MonoBehaviour
{

    public static RespawnController instance;

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

    //respawn olucaðý yer
    private Vector3 respawnPoint;
    //respawndann önce ne kadar beklyiicez
    public float waitToRespawn;

    private GameObject thePlayer;


    void Start()
    {
        thePlayer = PlayerHealthController.instance.gameObject;

        respawnPoint = thePlayer.transform.position;
    }

    
    void Update()
    {
        
    }

    public void SetSpawn(Vector3 newPosition)
    {
        respawnPoint = newPosition;
    }

    public void Respawn()
    {
        StartCoroutine(RespawnCo());    //coroutine(Ýþlem süreci)
    }


    //Normalde iþlem süreci bellidir ve program sýrayla okur biz respawnýmýz için yeni bir iþlem süreci açýyoruz.Bunun için Ienumator kullanýyoruz

    IEnumerator RespawnCo()
    {
        thePlayer.SetActive(false);

        //belirlediðim süre kadar burda iþlemleri durdurucak yani yeni oluþturduðum corutu durdurucak.
        yield return new WaitForSeconds(waitToRespawn); 
        //Sahneyi yeniliyoruz çünkü respawn olduðmuzda yýkýlan þeyler geri gelmeli ve canavarlar tekrar doðmalý en kýsa yolu bu.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        thePlayer.transform.position = respawnPoint;
        thePlayer.SetActive (true);


        PlayerHealthController.instance.fillHealth();
    }

}
