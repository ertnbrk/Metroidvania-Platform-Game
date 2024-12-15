using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Cameran�n player� takip etmesi laz�m

    private PlayerController player;

    //Kameran�n dikeyde fazla hareketli olmas�n� engellemeliyim.Bunun i�in bir bo� obje olu�turup box colider ekledim ve yeni bir layer olu�turdum project settingsten bu layer�n ba�kalar�yla etkile�ime girmesini engellemek i�in tiklerini kald�rd�m(pyshics 2d).Colider'�n� is trigger yapt�m.
    [SerializeField]
    BoxCollider2D boundsBox;
    
    private float halfHeight,halfWidth;

    void Start()
    {
        //player controller scriptine sahip olan objeyi bul ve bizim player �rne�imize e�itle
        player = FindObjectOfType<PlayerController>();


        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        //kameray� player� takip ettirmek

        if (player != null) //bir �ekilde karakter ortadan kalkarsa diye �nlem ama�l� bo� de�ilse diyorum.
        {
            transform.position = new Vector3(Mathf.Clamp( player.transform.position.x,boundsBox.bounds.min.x,boundsBox.bounds.max.x -halfWidth),
                Mathf.Clamp(player.transform.position.y, boundsBox.bounds.min.y + halfHeight, boundsBox.bounds.max.y),
                transform.position.z);
            //vector3 olu�turuyoruz ��nk� kameram�z z eksenini de tan�yor d�md�z e�itlersek kamera i�eri giricek hi� bir �ey g�r�lm�y�cek x ve y'yi karekterimizin x ve y siyle e�lerken z yi ise sabit b�rak�yoruz
        }
        
    }
}
