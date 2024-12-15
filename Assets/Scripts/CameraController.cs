using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Cameranýn playerý takip etmesi lazým

    private PlayerController player;

    //Kameranýn dikeyde fazla hareketli olmasýný engellemeliyim.Bunun için bir boþ obje oluþturup box colider ekledim ve yeni bir layer oluþturdum project settingsten bu layerýn baþkalarýyla etkileþime girmesini engellemek için tiklerini kaldýrdým(pyshics 2d).Colider'ýný is trigger yaptým.
    [SerializeField]
    BoxCollider2D boundsBox;
    
    private float halfHeight,halfWidth;

    void Start()
    {
        //player controller scriptine sahip olan objeyi bul ve bizim player örneðimize eþitle
        player = FindObjectOfType<PlayerController>();


        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        //kamerayý playerý takip ettirmek

        if (player != null) //bir þekilde karakter ortadan kalkarsa diye önlem amaçlý boþ deðilse diyorum.
        {
            transform.position = new Vector3(Mathf.Clamp( player.transform.position.x,boundsBox.bounds.min.x,boundsBox.bounds.max.x -halfWidth),
                Mathf.Clamp(player.transform.position.y, boundsBox.bounds.min.y + halfHeight, boundsBox.bounds.max.y),
                transform.position.z);
            //vector3 oluþturuyoruz çünkü kameramýz z eksenini de tanýyor dümdüz eþitlersek kamera içeri giricek hiç bir þey görülmüyücek x ve y'yi karekterimizin x ve y siyle eþlerken z yi ise sabit býrakýyoruz
        }
        
    }
}
