using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    public GameObject healthEffect;
    public int HealthAmount = 20;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {

            if (PlayerHealthController.instance.currentHealth != PlayerHealthController.instance.maxHealth)
            {
                PlayerHealthController.instance.HealthUp(HealthAmount);

                if (healthEffect != null)
                {
                    Instantiate(healthEffect, transform.position, Quaternion.identity);
                }
                Destroy(gameObject);
            }
            

        }
    }
}
