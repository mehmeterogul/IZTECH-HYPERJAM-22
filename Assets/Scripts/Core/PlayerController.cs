using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speedModifier = 0.01f;
    [SerializeField] float horizontalBoundry = 3f;

    Vector3 lastMousePosition;

    bool canMove = true;
    bool hasHitted = false;

    GameController gameController;
    AudioSource audioSource;
    [SerializeField] Animator animator;
    [SerializeField] AudioClip pickUpSound;
    [SerializeField] AudioClip hitObstacleSound;
    [SerializeField] AudioClip hitWallSound;
    [SerializeField] AudioClip finishSound;

    [SerializeField] GameObject cubePrefab;
    [SerializeField] Transform cubeSpillPosition;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
        audioSource = GetComponent<AudioSource>();

        if (!gameController)
        {
            Debug.Log("There is no game controller in the scene");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameController) return;

        if (!canMove) return;

        if(Input.GetMouseButtonDown(0))
        {
            gameController.StartGame();
            lastMousePosition = Input.mousePosition;
        }

        if(Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, new Vector3(transform.position.x + delta.x * speedModifier, transform.position.y, transform.position.z), 1f);
            transform.position = smoothedPosition;
            lastMousePosition = Input.mousePosition;
        }

        if (transform.position.x > horizontalBoundry)
        {
            transform.position = new Vector3(horizontalBoundry, transform.position.y, transform.position.z);
        }
        if (transform.position.x < -horizontalBoundry)
        {
            transform.position = new Vector3(-horizontalBoundry, transform.position.y, transform.position.z);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.CompareTag("CubeObstacle") || collision.gameObject.CompareTag("NotBreakableCube")) && !hasHitted)
        {
            audioSource.PlayOneShot(hitWallSound, 1f);
            gameController.PlayerHittedWall();
            SetCanMoveFalse();
            animator.SetTrigger("crash");
            hasHitted = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("CubeSelectTrigger"))
        {
            gameController.SetStateToSelecting();
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Collectable"))
        {
            audioSource.PlayOneShot(pickUpSound, 0.85f);
            gameController.PlayerPickedUpCollectable();
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Obstacle"))
        {
            audioSource.PlayOneShot(hitObstacleSound, 1f);

            int decreaseCount = Random.Range(2, 4);
            gameController.PlayerHittedObtsacle(decreaseCount);

            for (int i = 0; i < decreaseCount; i++)
            {
                GameObject temp = Instantiate(cubePrefab, cubeSpillPosition.position, Quaternion.identity);
                temp.GetComponent<Rigidbody>().AddForce(GetRandomForceVector(), ForceMode.Impulse);
                Destroy(temp, 2f);
            }

            transform.DORewind();
            transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.2f);
        }

        if(other.gameObject.CompareTag("SelectCursorTutorial"))
        {
            gameController.ActivateSelectCursor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("FinishLineTrigger"))
        {
            audioSource.PlayOneShot(finishSound, 1f);
            Invoke("SetCanMoveFalse", 0.3f);
            Invoke("FinishLevelAnimationTrigger", 0.35f);
            gameController.LevelFinished();
        }
    }

    public void SetCanMoveTrue()
    {
        canMove = true;
        animator.SetBool("isWalking", true);
    }

    public void SetCanMoveFalse()
    {
        canMove = false;
        animator.SetBool("isWalking", false);
    }

    public void FinishLevelAnimationTrigger()
    {
        animator.SetTrigger("finishLevel");
    }

    Vector3 GetRandomForceVector()
    {
        return new Vector3(GetRandomXValue(), GetRandomYValue(), -10f);
    }

    float GetRandomXValue()
    {
        return Random.Range(2, 4);
    }

    float GetRandomYValue()
    {
        return Random.Range(7, 12);
    }
}