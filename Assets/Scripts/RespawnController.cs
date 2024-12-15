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
            instance = this;    //playerHealthController'� single a �eviriyoruz bu sayade getcomponent demeden instance'e eri�ebiliyoruz ve instance �zerinden t�m fonksiyonlara.

            DontDestroyOnLoad(gameObject);  //yeni bir sahne olu�turdu�umuzda veya mevcut sahneyi yeniledi�imizde bu objeyi yok etme diyoruz.��nk� �l�nce otamatik tekrar olu�turuyodu.
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //respawn oluca�� yer
    private Vector3 respawnPoint;
    //respawndann �nce ne kadar beklyiicez
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
        StartCoroutine(RespawnCo());    //coroutine(��lem s�reci)
    }


    //Normalde i�lem s�reci bellidir ve program s�rayla okur biz respawn�m�z i�in yeni bir i�lem s�reci a��yoruz.Bunun i�in Ienumator kullan�yoruz

    IEnumerator RespawnCo()
    {
        thePlayer.SetActive(false);

        //belirledi�im s�re kadar burda i�lemleri durdurucak yani yeni olu�turdu�um corutu durdurucak.
        yield return new WaitForSeconds(waitToRespawn); 
        //Sahneyi yeniliyoruz ��nk� respawn oldu�muzda y�k�lan �eyler geri gelmeli ve canavarlar tekrar do�mal� en k�sa yolu bu.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        thePlayer.transform.position = respawnPoint;
        thePlayer.SetActive (true);


        PlayerHealthController.instance.fillHealth();
    }

}
