using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //checkpoint objemize player geldi�inde respawncontrollerdaki spawn noktas�n� checkpointimiz olarak set ediyoruz.
            RespawnController.instance.SetSpawn(transform.position);
        }
    }
}
