using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    ObjectMover objectMover;
    CubeSelector cubeSelector;
    PlayerController playerController;

    bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        objectMover = FindObjectOfType<ObjectMover>();
        cubeSelector = FindObjectOfType<CubeSelector>();
        playerController = FindObjectOfType<PlayerController>();

        cubeSelector.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver) return;

        objectMover.MoveObjects();
    }
}
