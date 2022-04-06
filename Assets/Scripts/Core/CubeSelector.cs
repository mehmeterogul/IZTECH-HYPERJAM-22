using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSelector : MonoBehaviour
{
    [SerializeField] string selectedTag = "Selected";
    [SerializeField] string selectableTag = "Selectable";
    [SerializeField] Material highlightMaterial;
    [SerializeField] Material defaultMaterial;
    [SerializeField] GameObject cubePrefab;
    Camera mainCamera;

    bool isSelecting = false;
    bool canSelect = false;
    int selectableCubeCount = 0;
    int selectedCubeCount = 0;
    int maxCubeCount = 0;
    List<GameObject> selectedCubes = new List<GameObject>();

    AudioSource audioSource;
    [SerializeField] AudioClip selectCubeSound;
    [SerializeField] AudioClip throwCubeSound;

    private void Awake()
    {
        foreach (Transform child in gameObject.transform)
        {
            maxCubeCount += 1;
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(selectableCubeCount == 0)
        {
            CantSelect();
            FindObjectOfType<GameController>().SelectionComplete();
            return;
        }

        if (!canSelect) return;

        if(Input.GetMouseButtonDown(0))
        {
            isSelecting = true;
        }

        if(Input.GetMouseButton(0) && isSelecting)
        {
            RaycastMousePosition();
        }

        if(Input.GetMouseButtonUp(0))
        {
            // instantiate objects in selected area
            if(selectedCubes.Count > 1)
            {
                isSelecting = false;

                foreach (GameObject cube in selectedCubes)
                {
                    Vector3 pos = Vector3Int.RoundToInt(new Vector3(cube.transform.localPosition.x, cube.transform.localPosition.y, 0f));
                    GameObject temp = Instantiate(cubePrefab, pos + (Vector3.up * 0.5f), Quaternion.identity);
                    Destroy(temp, 1f);
                }

                audioSource.PlayOneShot(throwCubeSound, 1f);

                ClearCubes();
                CantSelect();
                FindObjectOfType<GameController>().SelectionComplete();
            }
            else
            {
                // Selection canceled
                ClearCubes();
            }
        }
    }

    void RaycastMousePosition()
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            var selection = hit.transform;

            if (selection.CompareTag(selectableTag) && (selectedCubeCount < selectableCubeCount))
            {
                var selectionRenderer = selection.GetComponent<Renderer>();
                if (selectionRenderer != null)
                {
                    selectionRenderer.material = highlightMaterial;
                }

                selectedCubes.Add(selection.gameObject);
                selection.tag = selectedTag;
                selectedCubeCount++;

                audioSource.PlayOneShot(selectCubeSound, 0.85f);
            }
        }
    }

    public void IncreaseSelectableCubeCount()
    {
        selectableCubeCount += 1;

        if (selectableCubeCount > maxCubeCount) selectableCubeCount = maxCubeCount;
    }

    public void DecreaseSelectableCubeCount(int decreaseCount)
    {
        selectableCubeCount -= decreaseCount;

        if (selectableCubeCount < 0) selectableCubeCount = 0;
    }

    void ClearCubes()
    {
        foreach (GameObject cube in selectedCubes)
        {
            cube.tag = selectableTag;

            var cubeRenderer = cube.GetComponent<Renderer>();
            if (cubeRenderer != null)
            {
                cubeRenderer.material = defaultMaterial;
            }

            DecreaseSelectableCubeCount(1);
        }

        selectedCubes.Clear();
        selectedCubeCount = 0;
    }

    public void CanSelect()
    {
        Invoke("CanSelectInvoke", 0.3f);
    }

    void CanSelectInvoke()
    {
        canSelect = true;
    }

    public void CantSelect()
    {
        canSelect = false;
    }

    public int GetCubeCount()
    {
        return selectableCubeCount;
    }
}
