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


    public Transform groundPoint;   //playera ekledi�imiz groundchecki al�caz
    private bool isOnGround;    //karakter yere d��meden z�playamas�n diye bool de�i�kenimiz
    public LayerMask whatIsGround;  //layermask hangi katman� checkledi�imizi g�stericek

    //Animator men�s�nden ekledi�miiz animasyonlar�n ba�lant�s�n� yapt�k 
    //e�er karakter yerdeyse ve h�z� 0.1 den b�y�kse ko�ma animasyonuna. E�er karakter yerde ve h�z� 0 ise durma animasyonuna gibi eklemeler yapt�k

    //�imdi isOnground ve speed de�i�kenlerini scriptimizden d�zenliyicez
    
    public Animator anim;

    //Ate� emri vermek i�in kodlar�m�z� yazal�m
    public BulletController shotToFire;
    public Transform shotPoint;

    //skills
    public float dashSpeed, dashTime;
    private float dashCounter;
    private bool canDoubleJump;
    //topa d�n��me 
    public GameObject standing,ball;
    public float waitToBall;
    private float ballCounter;
    public Animator ballAnim;

    [SerializeField]
    SpriteRenderer theSR, afterImage;   //karakterimiz ve afterimage
    [SerializeField]
    float afterImageLifeTime, timeBetweenAfterImages;   //Arkadna ��k�cak olan izin ne kadar kal�ca�� ve ne kadar s�k bir iz oluca��n�
    private float afterImageCounter;    //saya�
    [SerializeField]
    Color afterImageColor;  //renk
    [SerializeField]
    float waitAfterDashing;
    private float dashRechargeCounter;

    //bomba
    public Transform bombPoint;
    public GameObject bomb;
    //Kap�lardan ge�me
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

                //dash atma(ayaktaysak atabiliriz e�er topsak atamay�z)
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

                //velocity(h�z) yer de�i�tirmeyi ifade eder bir �eyin yerini de�i�tiriceksek hareket ettiriceksek kullan�r�z.

                theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, theRB.velocity.y);
                //Yatay harekete g�re yer de�i�tir demi� olduk Unityde tan�nl� axislere g�re.
                //GetAxisRow s�radan axis belirtiyoruz
                //GetAxis ise momentum da verebilemiz gibi �zellikler bar�nd�r�yor.


                //Karekterin animasyonu tek y�nl� oldu�u i�in y�n de�i�tirme
                if (theRB.velocity.x < 0) //sola hareket emri verdiysem
                {
                    transform.localScale = new Vector2(-1, transform.localScale.y);
                }
                else if (theRB.velocity.x > 0)  //sa�a hareket emri verdiysem
                {
                    transform.localScale = new Vector2(1, transform.localScale.y);
                }
                //velocity kullanmam�z�n nedeni h�z�m�z sola do�ru negatiftir.


            }
        }
        else
        {
            theRB.velocity = Vector2.zero;
        }


        //ayakta olma halimiz
        if (standing.activeSelf)
        {
            //Animat�r men�s�nden olu�turdu�umuz de�i�kene i�lem yaparak elde etti�imiz verileri at�yoruz ki animat�r ne zaman �al���ca��n� anlas�n ve dinamik olsun
            anim.SetBool("isOnGround", isOnGround);

            anim.SetFloat("speed", Mathf.Abs(theRB.velocity.x));    //x y�n�ndeki h�z�m�z� at�yoruz yani 0.1 den b�y�k m� k���k m� bakabilsin diye
                                                                                //bunu yaparken mevcut velocitymizin karesini al�yoruz abs ile ��nk� sola hareket edince velocity de�eri negatif oluyor ve biz animat�re 0.1 den b�y�k dedik bunu ��zmek i�in karesini al�yoruz ve her halikurda pozitif oluyor
        }
        if (ball.activeSelf)    //top pozisyonundaysak ballanime h�z�m�z� set ediyoruz
        {
            ballAnim.SetFloat("BallSpeed", Mathf.Abs(theRB.velocity.x));
        }








        //Ate� etme
        if (Input.GetButtonDown("Fire1"))
        {
            if (standing.activeSelf)
            {
                //Vuru� animasyonuna ge�mesi i�in ekledi�imiz trigger� set edicez
                anim.SetTrigger("ShotFire"); 
                Instantiate(shotToFire, shotPoint.position, shotPoint.rotation).moveDirection = new Vector2(transform.localScale.x, 0);

                
            }
            else if (ball.activeSelf && abilities.canDropBomb)
            {
                //bomba f�rlat
                Instantiate(bomb, transform.position, transform.rotation);
            }
            
        }
        //ball mode
        if (!ball.activeSelf)   //top active de�ilse
        {
            if (Input.GetAxisRaw("Vertical") < -.9f && abilities.canBecomeBall)    //dikey konumumuz -.9f alt�na d��erse topa d�n���ecz(yani s tu�una bas�l� tutulunca)
            {
                ballCounter -= Time.deltaTime;  //topa d�n��me s�resi her framede azals�n
                if (ballCounter <= 0)           //topa d�n��me s�rem bittiyse d�n��
                {
                    ball.SetActive(true);
                    standing.SetActive(false);
                    moveSpeed= moveSpeed *1.5f;
                }
            }
            else //s�reyi s�f�rla
            {
                ballCounter = waitToBall;
            }
        }
        else
        {
            if (Input.GetAxisRaw("Vertical") > .9f)    //dikey konumumuz .9f �st�ne ��karsa normale d�n�cez(yani w tu�una bas�l� tutulunca)
            {
                ballCounter -= Time.deltaTime;  //topa d�n��me s�resi her framede azals�n
                if (ballCounter <= 0)           //topa d�n��me s�rem bittiyse d�n��
                {
                    ball.SetActive(false);
                    standing.SetActive(true);
                    moveSpeed= moveSpeed /1.5f;
                }
            }
            else //s�reyi s�f�rla
            {
                ballCounter = waitToBall;
            }
        }

        
        


    }
    //dash izini b�rakmak i�in
    public void ShowAfterImage()
    {
        SpriteRenderer image = Instantiate(afterImage,transform.position,transform.rotation);
        image.sprite = theSR.sprite;    //player�m�n mevcut sprite�n� at�yorum yani dash att���nda hangi y�ne bak�yosa bana onu ver diyorum
        image.transform.localScale = transform.localScale;
        image.color = afterImageColor;
        //belirledi�im s�reden sonra yok olsun
        Destroy(image.gameObject, afterImageLifeTime);
    }

   

}
