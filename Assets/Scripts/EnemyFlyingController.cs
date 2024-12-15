using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyingController : MonoBehaviour
{

    public float rangeToStartChase; //karakterimiz bir alana girince onu peþliyicek alanýn çapý
    private bool isChasing;    //peþlemiþ mi

    public float moveSpeed, turnSpeed;
    private Transform player;

    public Animator anim;
    





    // Start is called before the first frame update
    void Start()
    {
        player = PlayerHealthController.instance.transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isChasing)
        {
            //player ile benim aramdaki mesafe peþleme mesafesinden kýsaysa peþle
            if (Vector3.Distance(transform.position,player.position) < rangeToStartChase)
            {
                isChasing = true; //peþle
                anim.SetBool("IsHanging", isChasing);
            }
        }
        else
        {
            if (player.gameObject.activeSelf)
            {
                //yön
                Vector3 direction = transform.position - player.position;
                //burayý anlamam gerekmiyo angle lazým olunca bu lazým
                float angle = Mathf.Atan2(direction.y,direction.x)*Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.AngleAxis(angle,Vector3.forward);

                //Rotasyonumuzu deðiþtiriyoruz olduðumuz rotasyodan targetRotasyona smooth geçiþ saðlýyor dönme hýzýmýzý karehýzýmzýla çarpýyoruz
                transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,turnSpeed*Time.deltaTime);

                //bir noktadan diðerine git demek.Bulunduðumuz konumdan player'ýn konumuna hýzýmýz * frame
                transform.position = Vector3.MoveTowards(transform.position,player.position,moveSpeed * Time.deltaTime);
                anim.SetBool("IsHanging", isChasing);
            }
        }
    }
}
