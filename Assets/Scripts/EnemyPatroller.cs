using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatroller : MonoBehaviour
{

    //Düþmanlarýn devriye alanlarýný ve hareket etmelerini ayarlamak

    //bir dizi istiyorum bu dizi patrolpointlerim olan objeler onlarý vericem
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
            //ebeveyni yok artýk diyoruz ki yaratýkla beraber pointler hareket etmesin
            pPoint.SetParent(null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //pointten 2f uzaktaysak
        if (Mathf.Abs( transform.position.x - patrolPoints[currentPoint].position.x) > .2f)
        {
            //eðer pointin solundaysak saða git saðýndaysak sola git
            if (transform.position.x < patrolPoints[currentPoint].position.x)
            {
                theRB.velocity = new Vector2(moveSpeed, theRB.velocity.y);
                //saða giderken yüzünü saða dönsün
                transform.localScale = new Vector3(-1f,1f,1f);
            }
            else
            {
                theRB.velocity = new Vector2(-moveSpeed, theRB.velocity.y);
                //sola giderken sola dönsün
                transform.localScale = Vector3.one;
            }
            //pointim 5fden yukardaysa zýpla ve ayný zamanda zaten zýplamýþ olmamýþ olmamýzý kontrol et
            if (transform.position.y < patrolPoints[currentPoint].position.y - .5f && theRB.velocity.y < .1f)
            {
                theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
            }
        }
        //pointe yeterince yakýnsak yani ulaþtýysak
        else
        {   //hareketi durdur
            theRB.velocity = new Vector2(0f, theRB.velocity.y);
            //geriye saymaya baþla(orda bir süre dursun devriye atýyo)
            waitCounter -= Time.deltaTime;
            //süreyi public olarak biz belirledik
            if (waitCounter <=0)
            {
                //süreyi resetle
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
