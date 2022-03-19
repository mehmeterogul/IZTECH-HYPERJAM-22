using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    enum STATE { RUNNING, SELECTING }

    ObjectMover objectMover;
    CubeSelector cubeSelector;
    PlayerController playerController;

    bool isGameOver = false;
    STATE state = STATE.RUNNING;

    // Start is called before the first frame update
    void Start()
    {
        objectMover = FindObjectOfType<ObjectMover>();
        cubeSelector = FindObjectOfType<CubeSelector>();
        playerController = FindObjectOfType<PlayerController>();

        if(!objectMover)
        {
            Debug.Log("There is no object mover in the scene");
        }

        if (!cubeSelector)
        {
            Debug.Log("There is no cube selector in the scene");
        }
        else
        {
            cubeSelector.gameObject.SetActive(false);
        }

        if (!playerController)
        {
            Debug.Log("There is no player controller in the scene");
        }
        else
        {
            playerController.SetCanMoveTrue();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!objectMover || !cubeSelector || !playerController) return;
        if (isGameOver) return;

        switch(state)
        {
            default:
            case STATE.RUNNING:
                objectMover.MoveObjects();
                break;
            case STATE.SELECTING:
                // Do nothing, wait for selection
                break;
        }
    }

    public void SetStateToSelecting()
    {
        state = STATE.SELECTING;
        if (!cubeSelector || !playerController) return;
        cubeSelector.gameObject.SetActive(true);
        playerController.SetCanMoveFalse();
    }

    public void PlayerPickedUpCollectable()
    {
        if (!cubeSelector) return;
        cubeSelector.IncreaseSelectableCubeCount();
    }

    public void PlayerHittedObtsacle()
    {
        if (!cubeSelector) return;
        cubeSelector.DecreaseSelectableCubeCount();
    }

    public void SelectionComplete()
    {
        if (!cubeSelector) return;
        cubeSelector.gameObject.SetActive(false);
        Invoke("SetStateToRunning", 0.3f);
    }

    public void SetStateToRunning()
    {
        state = STATE.RUNNING;
        if (!playerController) return;
        playerController.SetCanMoveTrue();
    }

    public void PlayerHittedWall()
    {
        isGameOver = true;
    }
}
