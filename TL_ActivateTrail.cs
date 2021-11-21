using System.Collections;
using UnityEngine;

public class TL_ActivateTrail : MonoBehaviour
{
    private Collider2D BoxCollider;


    void Start()
    {
        BoxCollider = GetComponent<Collider2D>();
        StartCoroutine(ActivateCollider());
    }

    IEnumerator ActivateCollider()
    {
        //Wait for 0.5 seconds
        yield return new WaitForSeconds(0.5f);

        //Enable the collder
        BoxCollider.enabled = true;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            //Obtain the pathfinder script from the enemy
            TL_Pathfinder PathfinderScript = collision.GetComponent<TL_Pathfinder>();

            //When the character steps on the slime trail, reduce speed
            PathfinderScript.SetSpeed(1.5f);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            //Obtain the pathfinder script from the enemy
            TL_Pathfinder PathfinderScript = collision.GetComponent<TL_Pathfinder>();

            //When the character steps off the slime trail, return speed back to normal
            PathfinderScript.SetSpeed(3f);
        }
    }
}
