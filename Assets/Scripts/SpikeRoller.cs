using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeRoller : MonoBehaviour
{
    [SerializeField] Transform[] paths;
    [SerializeField] float moveSpeed = 2f;
    int currentPathIndex = 0;

    // Update is called once per frame
    void Update()
    {
        if(transform.position == paths[currentPathIndex].position)
        {
            if (currentPathIndex + 1 <= paths.Length - 1)
                currentPathIndex++;
            else
                currentPathIndex = 0;
        }

        transform.position = Vector3.MoveTowards(transform.position, paths[currentPathIndex].position, moveSpeed * Time.deltaTime);
    }
}
