using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //checkpoint objemize player geldiðinde respawncontrollerdaki spawn noktasýný checkpointimiz olarak set ediyoruz.
            RespawnController.instance.SetSpawn(transform.position);
        }
    }
}
