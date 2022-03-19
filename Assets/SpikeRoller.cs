using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeRoller : MonoBehaviour
{
    [SerializeField] Transform[] paths;
    int currentPathIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, paths[currentPathIndex].position, 1f * Time.deltaTime);
    }
}
