using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtsacleDestroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("Collectable"))
        {
            Destroy(other.gameObject);
        }
    }
}
