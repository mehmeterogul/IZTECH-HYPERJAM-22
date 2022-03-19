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

    GameController gameController;
    [SerializeField] Animator animator;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();

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
        if (collision.gameObject.CompareTag("CubeObstacle"))
        {
            gameController.PlayerHittedWall();
            SetCanMoveFalse();
            animator.SetTrigger("crash");
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
            gameController.PlayerPickedUpCollectable();
            StartCoroutine(DestroyGameObject(other.gameObject));
        }

        if (other.gameObject.CompareTag("Obstacle"))
        {
            gameController.PlayerHittedObtsacle();
            StartCoroutine(DestroyGameObject(other.gameObject));
        }
    }

    IEnumerator DestroyGameObject(GameObject obj)
    {
        obj.transform.DORewind();
        obj.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.2f);
        yield return new WaitForSeconds(0.25f);
        Destroy(obj);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("FinishLineTrigger"))
        {

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
}