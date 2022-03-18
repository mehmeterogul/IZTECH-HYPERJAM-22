using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    // Update is called once per frame
    public void MoveObjects()
    {
        transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime);
    }
}