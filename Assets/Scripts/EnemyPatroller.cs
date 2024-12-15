using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatroller : MonoBehaviour
{

    //D��manlar�n devriye alanlar�n� ve hareket etmelerini ayarlamak

    //bir dizi istiyorum bu dizi patrolpointlerim olan objeler onlar� vericem
    [SerializeField]
    Transform[] patrolPoints;
    //mevcut point
    private int currentPoint;

    public float moveSpeed, waitAtPoints;
    private float waitCounter;

    public float jumpForce;

    [SerializeField]
    Rigidbody2D theRB;
    [SerializeField]
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        waitCounter = waitAtPoints;
        foreach (Transform pPoint in patrolPoints)
        {
            //ebeveyni yok art�k diyoruz ki yarat�kla beraber pointler hareket etmesin
            pPoint.SetParent(null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //pointten 2f uzaktaysak
        if (Mathf.Abs( transform.position.x - patrolPoints[currentPoint].position.x) > .2f)
        {
            //e�er pointin solundaysak sa�a git sa��ndaysak sola git
            if (transform.position.x < patrolPoints[currentPoint].position.x)
            {
                theRB.velocity = new Vector2(moveSpeed, theRB.velocity.y);
                //sa�a giderken y�z�n� sa�a d�ns�n
                transform.localScale = new Vector3(-1f,1f,1f);
            }
            else
            {
                theRB.velocity = new Vector2(-moveSpeed, theRB.velocity.y);
                //sola giderken sola d�ns�n
                transform.localScale = Vector3.one;
            }
            //pointim 5fden yukardaysa z�pla ve ayn� zamanda zaten z�plam�� olmam�� olmam�z� kontrol et
            if (transform.position.y < patrolPoints[currentPoint].position.y - .5f && theRB.velocity.y < .1f)
            {
                theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
            }
        }
        //pointe yeterince yak�nsak yani ula�t�ysak
        else
        {   //hareketi durdur
            theRB.velocity = new Vector2(0f, theRB.velocity.y);
            //geriye saymaya ba�la(orda bir s�re dursun devriye at�yo)
            waitCounter -= Time.deltaTime;
            //s�reyi public olarak biz belirledik
            if (waitCounter <=0)
            {
                //s�reyi resetle
                waitCounter = waitAtPoints;
                ++currentPoint;
                if (currentPoint >= patrolPoints.Length)
                {
                    currentPoint = 0;
                }
            }
        }
        anim.SetFloat("Speed", Mathf.Abs(theRB.velocity.x));
    }
}
