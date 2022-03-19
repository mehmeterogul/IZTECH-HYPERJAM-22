using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDestroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CubeObstacle"))
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
        }

        if(other.gameObject.CompareTag("NotBreakableCube"))
        {
            Destroy(gameObject.GetComponent<Cube>());

            if(!gameObject.GetComponent<Rigidbody>())
            {
                Rigidbody rb = gameObject.AddComponent<Rigidbody>();
                rb.AddForce(transform.position - other.gameObject.transform.position * 3f, ForceMode.Impulse);
            }

            Destroy(gameObject, 2f);
        }
    }
}
