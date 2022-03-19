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

        cubeSelector.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver) return;

        switch(state)
        {
            default:
            case STATE.RUNNING:
                objectMover.MoveObjects();
                break;
            case STATE.SELECTING:
                //
                break;
        }
    }

    public void SetStateToSelecting()
    {
        state = STATE.SELECTING;
        cubeSelector.gameObject.SetActive(true);
        playerController.SetCanMoveFalse();
    }

    public void PlayerPickedUpCollectable()
    {
        cubeSelector.IncreaseSelectableCubeCount();
    }

    public void PlayerHittedObtsacle()
    {
        cubeSelector.DecreaseSelectableCubeCount();
    }

    public void SelectionComplete()
    {
        cubeSelector.gameObject.SetActive(false);
        Invoke("SetStateToRunning", 1.5f);
    }

    public void SetStateToRunning()
    {
        state = STATE.RUNNING;
        playerController.SetCanMoveTrue();
    }
}
