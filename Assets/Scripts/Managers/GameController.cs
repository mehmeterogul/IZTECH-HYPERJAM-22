using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    enum STATE { RUNNING, IDLE }

    ObjectMover objectMover;
    CubeSelector cubeSelector;
    PlayerController playerController;

    bool isGameOver = false;
    STATE state = STATE.IDLE;

    [SerializeField] TextMeshProUGUI cubeCountText;
    [SerializeField] GameObject gameOverText;
    [SerializeField] GameObject restartButton;
    [SerializeField] GameObject nextLevelText;
    [SerializeField] GameObject nextLevelButton;
    [SerializeField] GameObject cursor;
    [SerializeField] GameObject selectCursor;

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

    public void StartGame()
    {
        state = STATE.RUNNING;
        playerController.SetCanMoveTrue();
        cursor.SetActive(false);
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
        gameOverText.SetActive(true);
        restartButton.SetActive(true);
    }

    public void LevelFinished()
    {
        state = STATE.IDLE;
        FindObjectOfType<CameraSwitcher>().SwitchCamera();
        Invoke("ActivateNextLevelComponents", 0.5f);
    }

    void ActivateNextLevelComponents()
    {
        nextLevelText.SetActive(true);
        nextLevelButton.SetActive(true);
    }

    void UpdateCubeCountText()
    {
        cubeCountText.transform.DORewind();
        cubeCountText.transform.DOPunchScale(new Vector3(0.3f, 0.5f, 0.3f), .25f);
        cubeCountText.text = cubeSelector.GetCubeCount().ToString();
    }

    public void RestartButton()
    {
        restartButton.GetComponent<Button>().interactable = false;
        StartCoroutine(RestartLevel());
    }

    public IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevelButton()
    {
        nextLevelButton.GetComponent<Button>().interactable = false;
        StartCoroutine(NextLevel());
    }

    public IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ActivateSelectCursor()
    {
        selectCursor.SetActive(true);
        Invoke("DeactivateSelectCursor", 5f);
    }

    public void DeactivateSelectCursor()
    {
        selectCursor.SetActive(false);
    }
}
