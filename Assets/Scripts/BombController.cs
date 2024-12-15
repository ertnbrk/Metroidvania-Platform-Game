using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    // Start is called before the first frame update

    public float timeToExplode = .5f;
    public GameObject explosion;

    public float blastRange;
    public LayerMask whatIsDestructible ;

    public int damageAmount = 3;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeToExplode -= Time.deltaTime;
        if (timeToExplode <= 0)
        {
            if (explosion != null)
            {
                Instantiate(explosion, transform.position, transform.rotation);
            }
            Destroy(gameObject);

            //overlapcircleall bize bir array olu�turur
            Collider2D[] objectsToRemove = Physics2D.OverlapCircleAll(transform.position, blastRange, whatIsDestructible);

            if (objectsToRemove.Length > 0)
            {
                foreach (Collider2D item in objectsToRemove)
                {
                    Destroy(item.gameObject);
                }
            }
            
        }

    }

   
}
