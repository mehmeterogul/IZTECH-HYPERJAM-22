using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class GameController : MonoBehaviour
{
    enum STATE { RUNNING, IDLE }

    ObjectMover objectMover;
    CubeSelector cubeSelector;
    PlayerController playerController;

    bool isGameOver = false;
    STATE state = STATE.RUNNING;

    [SerializeField] TextMeshProUGUI cubeCountText;

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

        cubeCountText.text = "0";
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
            case STATE.IDLE:
                // Do nothing, wait for selection or level finish animation.
                break;
        }
    }

    public void SetStateToSelecting()
    {
        state = STATE.IDLE;
        if (!cubeSelector || !playerController) return;
        cubeSelector.gameObject.SetActive(true);
        cubeSelector.CanSelect();
        playerController.SetCanMoveFalse();
    }

    public void PlayerPickedUpCollectable()
    {
        if (!cubeSelector) return;
        cubeSelector.IncreaseSelectableCubeCount();
        UpdateCubeCountText();
    }

    public void PlayerHittedObtsacle()
    {
        if (!cubeSelector) return;
        cubeSelector.DecreaseSelectableCubeCount();
        UpdateCubeCountText();
    }

    public void SelectionComplete()
    {
        if (!cubeSelector) return;
        UpdateCubeCountText();
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

    public void LevelFinished()
    {
        state = STATE.IDLE;
    }

    void UpdateCubeCountText()
    {
        cubeCountText.transform.DORewind();
        cubeCountText.transform.DOPunchScale(new Vector3(0.3f, 0.5f, 0.3f), .25f);
        cubeCountText.text = cubeSelector.GetCubeCount().ToString();
    }
}
