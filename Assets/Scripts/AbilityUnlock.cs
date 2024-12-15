using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class AbilityUnlock : MonoBehaviour
{
    

    public bool unlockDoubleJump,unlockDash,unlockBecomeBall,unlockDropBomb;


    public GameObject pickUpEffect;

    public string UnlockMessage;
    public TMP_Text unlockText;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Objemin coliderý trigger olduðu için burda player ile temasa geçerse eðer diye belirtiyorum ona göre yetenekleri açýcam.Tag olarak standing spirtea player tagý veriyoruz ball sprite'a da

        if (collision.tag == "Player")  
        {
            PlayerAbilityTracker player = collision.GetComponentInParent<PlayerAbilityTracker>();

            if (unlockDoubleJump)
            {
                player.canDoubleJump= true;
            }
            if (unlockDash)
            {
                player.canDash = true;
            }
            if (unlockBecomeBall)
            {
                player.canBecomeBall = true;
            }
            if (unlockDropBomb)
            {
                player.canDropBomb = true;
            }


            Instantiate(pickUpEffect,transform.position,Quaternion.identity);

            //Hangi özelliði açtýðýmýzý yazýcak bir textimiz var onu yukarda dahil ettik þimdi textini set ediyoruz ve görünür kýlýyoruz
            //transform.parent diyerek özellik açan objemizin çocuðu olmaktan çýkarýyýoruz.Çünkü alt tarafta objeyi yok ediyoruz objeyi yok ettiðimizde text gitmesin diye ebeveynin yok diyoruz.
            unlockText.transform.parent.SetParent(null);
            //pozisyonunu objemizin pozisyonuyla eþliyoruz baþka yere gitmesin yazý diye
            unlockText.transform.parent.position = transform.position;
            unlockText.text = UnlockMessage;
            unlockText.gameObject.SetActive(true);
            //5 saniye say sonra yok et
            Destroy(unlockText.transform.parent.gameObject, 5f);

            Destroy(gameObject);
        }
    }
}
