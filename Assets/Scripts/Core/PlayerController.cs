using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speedModifier = 0.01f;
    [SerializeField] float horizontalBoundry = 3f;

    Vector3 lastMousePosition;

    bool canMove = true;

    GameController gameController;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove) return;

        if(Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }

        if(Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            transform.position = new Vector3(transform.position.x + delta.x * speedModifier, transform.position.y, transform.position.z);
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("CubeSelectTrigger"))
        {
            gameController.SetStateToSelecting();
        }

        if (other.gameObject.CompareTag("Collectable"))
        {
            gameController.PlayerPickedUpCollectable();
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Obstacle"))
        {
            gameController.PlayerHittedObtsacle();
            Destroy(other.gameObject);
        }
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
    }

    public void SetCanMoveFalse()
    {
        canMove = false;
    }
}