using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyingController : MonoBehaviour
{

    public float rangeToStartChase; //karakterimiz bir alana girince onu pe�liyicek alan�n �ap�
    private bool isChasing;    //pe�lemi� mi

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
            //player ile benim aramdaki mesafe pe�leme mesafesinden k�saysa pe�le
            if (Vector3.Distance(transform.position,player.position) < rangeToStartChase)
            {
                isChasing = true; //pe�le
                anim.SetBool("IsHanging", isChasing);
            }
        }
        else
        {
            if (player.gameObject.activeSelf)
            {
                //y�n
                Vector3 direction = transform.position - player.position;
                //buray� anlamam gerekmiyo angle laz�m olunca bu laz�m
                float angle = Mathf.Atan2(direction.y,direction.x)*Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.AngleAxis(angle,Vector3.forward);

                //Rotasyonumuzu de�i�tiriyoruz oldu�umuz rotasyodan targetRotasyona smooth ge�i� sa�l�yor d�nme h�z�m�z� kareh�z�mz�la �arp�yoruz
                transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,turnSpeed*Time.deltaTime);

                //bir noktadan di�erine git demek.Bulundu�umuz konumdan player'�n konumuna h�z�m�z * frame
                transform.position = Vector3.MoveTowards(transform.position,player.position,moveSpeed * Time.deltaTime);
                anim.SetBool("IsHanging", isChasing);
            }
        }
    }
}
