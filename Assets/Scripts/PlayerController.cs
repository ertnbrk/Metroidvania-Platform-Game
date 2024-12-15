using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField]
    Rigidbody2D theRB;
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    float jumpForce;


    public Transform groundPoint;   //playera eklediðimiz groundchecki alýcaz
    private bool isOnGround;    //karakter yere düþmeden zýplayamasýn diye bool deðiþkenimiz
    public LayerMask whatIsGround;  //layermask hangi katmaný checklediðimizi göstericek

    //Animator menüsünden eklediðmiiz animasyonlarýn baðlantýsýný yaptýk 
    //eðer karakter yerdeyse ve hýzý 0.1 den büyükse koþma animasyonuna. Eðer karakter yerde ve hýzý 0 ise durma animasyonuna gibi eklemeler yaptýk

    //þimdi isOnground ve speed deðiþkenlerini scriptimizden düzenliyicez
    
    public Animator anim;

    //Ateþ emri vermek için kodlarýmýzý yazalým
    public BulletController shotToFire;
    public Transform shotPoint;

    //skills
    public float dashSpeed, dashTime;
    private float dashCounter;
    private bool canDoubleJump;
    //topa dönüþme 
    public GameObject standing,ball;
    public float waitToBall;
    private float ballCounter;
    public Animator ballAnim;

    [SerializeField]
    SpriteRenderer theSR, afterImage;   //karakterimiz ve afterimage
    [SerializeField]
    float afterImageLifeTime, timeBetweenAfterImages;   //Arkadna çýkýcak olan izin ne kadar kalýcaðý ve ne kadar sýk bir iz olucaðýný
    private float afterImageCounter;    //sayaç
    [SerializeField]
    Color afterImageColor;  //renk
    [SerializeField]
    float waitAfterDashing;
    private float dashRechargeCounter;

    //bomba
    public Transform bombPoint;
    public GameObject bomb;
    //Kapýlardan geçme
    public bool canMove;

    private PlayerAbilityTracker abilities;

    void Start()
    {
        abilities = GetComponent<PlayerAbilityTracker>();


        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        //CanMove
        if (canMove)
        {




            isOnGround = Physics2D.OverlapCircle(groundPoint.position, 0.5f, whatIsGround);

            //Jump
            if (Input.GetButtonDown("Jump") && (isOnGround || (canDoubleJump && abilities.canDoubleJump)))
            {
                if (isOnGround)
                {
                    canDoubleJump = true;
                }
                else
                {
                    canDoubleJump = false;
                    anim.SetTrigger("DoubleJump");

                }

                theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
            }

            if (dashRechargeCounter > 0)
            {
                dashRechargeCounter -= Time.deltaTime;
            }

            else
            {

                //dash atma(ayaktaysak atabiliriz eðer topsak atamayýz)
                if (Input.GetButtonDown("Fire2") && standing.activeSelf && abilities.canDash)
                {
                    dashCounter = dashTime;
                    ShowAfterImage();

                }

            }
            if (dashCounter > 0)
            {

                dashCounter = dashCounter - Time.deltaTime;

                theRB.velocity = new Vector2(dashSpeed * transform.localScale.x, theRB.velocity.y);

                afterImageCounter -= Time.deltaTime;
                if (afterImageCounter <= 0)
                {
                    ShowAfterImage();
                }

                dashRechargeCounter = waitAfterDashing;

            }
            else
            {

                //velocity(hýz) yer deðiþtirmeyi ifade eder bir þeyin yerini deðiþtiriceksek hareket ettiriceksek kullanýrýz.

                theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, theRB.velocity.y);
                //Yatay harekete göre yer deðiþtir demiþ olduk Unityde tanýnlý axislere göre.
                //GetAxisRow sýradan axis belirtiyoruz
                //GetAxis ise momentum da verebilemiz gibi özellikler barýndýrýyor.


                //Karekterin animasyonu tek yönlü olduðu için yön deðiþtirme
                if (theRB.velocity.x < 0) //sola hareket emri verdiysem
                {
                    transform.localScale = new Vector2(-1, transform.localScale.y);
                }
                else if (theRB.velocity.x > 0)  //saða hareket emri verdiysem
                {
                    transform.localScale = new Vector2(1, transform.localScale.y);
                }
                //velocity kullanmamýzýn nedeni hýzýmýz sola doðru negatiftir.


            }
        }
        else
        {
            theRB.velocity = Vector2.zero;
        }


        //ayakta olma halimiz
        if (standing.activeSelf)
        {
            //Animatör menüsünden oluþturduðumuz deðiþkene iþlem yaparak elde ettiðimiz verileri atýyoruz ki animatör ne zaman çalýþýcaðýný anlasýn ve dinamik olsun
            anim.SetBool("isOnGround", isOnGround);

            anim.SetFloat("speed", Mathf.Abs(theRB.velocity.x));    //x yönündeki hýzýmýzý atýyoruz yani 0.1 den büyük mü küçük mü bakabilsin diye
                                                                                //bunu yaparken mevcut velocitymizin karesini alýyoruz abs ile çünkü sola hareket edince velocity deðeri negatif oluyor ve biz animatöre 0.1 den büyük dedik bunu çözmek için karesini alýyoruz ve her halikurda pozitif oluyor
        }
        if (ball.activeSelf)    //top pozisyonundaysak ballanime hýzýmýzý set ediyoruz
        {
            ballAnim.SetFloat("BallSpeed", Mathf.Abs(theRB.velocity.x));
        }








        //Ateþ etme
        if (Input.GetButtonDown("Fire1"))
        {
            if (standing.activeSelf)
            {
                //Vuruþ animasyonuna geçmesi için eklediðimiz triggerý set edicez
                anim.SetTrigger("ShotFire"); 
                Instantiate(shotToFire, shotPoint.position, shotPoint.rotation).moveDirection = new Vector2(transform.localScale.x, 0);

                
            }
            else if (ball.activeSelf && abilities.canDropBomb)
            {
                //bomba fýrlat
                Instantiate(bomb, transform.position, transform.rotation);
            }
            
        }
        //ball mode
        if (!ball.activeSelf)   //top active deðilse
        {
            if (Input.GetAxisRaw("Vertical") < -.9f && abilities.canBecomeBall)    //dikey konumumuz -.9f altýna düþerse topa dönüþüecz(yani s tuþuna basýlý tutulunca)
            {
                ballCounter -= Time.deltaTime;  //topa dönüþme süresi her framede azalsýn
                if (ballCounter <= 0)           //topa dönüþme sürem bittiyse dönüþ
                {
                    ball.SetActive(true);
                    standing.SetActive(false);
                    moveSpeed= moveSpeed *1.5f;
                }
            }
            else //süreyi sýfýrla
            {
                ballCounter = waitToBall;
            }
        }
        else
        {
            if (Input.GetAxisRaw("Vertical") > .9f)    //dikey konumumuz .9f üstüne çýkarsa normale dönücez(yani w tuþuna basýlý tutulunca)
            {
                ballCounter -= Time.deltaTime;  //topa dönüþme süresi her framede azalsýn
                if (ballCounter <= 0)           //topa dönüþme sürem bittiyse dönüþ
                {
                    ball.SetActive(false);
                    standing.SetActive(true);
                    moveSpeed= moveSpeed /1.5f;
                }
            }
            else //süreyi sýfýrla
            {
                ballCounter = waitToBall;
            }
        }

        
        


    }
    //dash izini býrakmak için
    public void ShowAfterImage()
    {
        SpriteRenderer image = Instantiate(afterImage,transform.position,transform.rotation);
        image.sprite = theSR.sprite;    //playerýmýn mevcut spriteýný atýyorum yani dash attýðýnda hangi yöne bakýyosa bana onu ver diyorum
        image.transform.localScale = transform.localScale;
        image.color = afterImageColor;
        //belirlediðim süreden sonra yok olsun
        Destroy(image.gameObject, afterImageLifeTime);
    }

   

}
