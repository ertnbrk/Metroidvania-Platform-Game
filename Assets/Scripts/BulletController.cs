using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    public float bulletSpeed;
    public Rigidbody2D theRB;
    public Vector2 moveDirection;
    
    //Efektimizi �a��r�yoruz
    [SerializeField]
    GameObject impactEffect;


    //Verice�i hasar
    public int damageAmount = 1;


    // Update is called once per frame
    void Update()
    {
        theRB.velocity = moveDirection * bulletSpeed;   //mermi sa�a do�ru hareket edicek

        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //e�er bir d��man tag�na sahip birine de�erse mermim ona hasar vericek olan fonksiyonu �a��r�yorum
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyHealthControlller>().DamageEnemy(damageAmount);
            
        }





        //Bu method benim scriptime sahip obje ba�ka bir colidera de�erse �al���cak
        //Efectimizi koyuyoruz
        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);    //gameObject dedi�im scripte sahip b�t�n objeler
    }
    //Obje sahneden ��kt���nda �al���r
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
