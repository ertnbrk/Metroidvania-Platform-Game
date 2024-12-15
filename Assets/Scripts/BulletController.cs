using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    public float bulletSpeed;
    public Rigidbody2D theRB;
    public Vector2 moveDirection;
    
    //Efektimizi çaðýrýyoruz
    [SerializeField]
    GameObject impactEffect;


    //Vericeði hasar
    public int damageAmount = 1;


    // Update is called once per frame
    void Update()
    {
        theRB.velocity = moveDirection * bulletSpeed;   //mermi saða doðru hareket edicek

        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //eðer bir düþman tagýna sahip birine deðerse mermim ona hasar vericek olan fonksiyonu çaðýrýyorum
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyHealthControlller>().DamageEnemy(damageAmount);
            
        }





        //Bu method benim scriptime sahip obje baþka bir colidera deðerse çalýþýcak
        //Efectimizi koyuyoruz
        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);    //gameObject dediðim scripte sahip bütün objeler
    }
    //Obje sahneden çýktýðýnda çalýþýr
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
