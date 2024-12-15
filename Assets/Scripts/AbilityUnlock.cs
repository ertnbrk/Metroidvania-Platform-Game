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
        //Objemin colider� trigger oldu�u i�in burda player ile temasa ge�erse e�er diye belirtiyorum ona g�re yetenekleri a��cam.Tag olarak standing spirtea player tag� veriyoruz ball sprite'a da

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

            //Hangi �zelli�i a�t���m�z� yaz�cak bir textimiz var onu yukarda dahil ettik �imdi textini set ediyoruz ve g�r�n�r k�l�yoruz
            //transform.parent diyerek �zellik a�an objemizin �ocu�u olmaktan ��kar�y�oruz.��nk� alt tarafta objeyi yok ediyoruz objeyi yok etti�imizde text gitmesin diye ebeveynin yok diyoruz.
            unlockText.transform.parent.SetParent(null);
            //pozisyonunu objemizin pozisyonuyla e�liyoruz ba�ka yere gitmesin yaz� diye
            unlockText.transform.parent.position = transform.position;
            unlockText.text = UnlockMessage;
            unlockText.gameObject.SetActive(true);
            //5 saniye say sonra yok et
            Destroy(unlockText.transform.parent.gameObject, 5f);

            Destroy(gameObject);
        }
    }
}
