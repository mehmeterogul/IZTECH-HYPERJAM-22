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

    bool canSelect = false;
    int selectableCubeCount = 0;
    int selectedCubeCount = 0;
    int maxCubeCount = 0;
    List<GameObject> selectedCubes = new List<GameObject>();

    private void Start()
    {
        mainCamera = Camera.main;
        
        foreach(Transform child in gameObject.transform)
        {
            maxCubeCount += 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            canSelect = true;
        }

        if(Input.GetMouseButton(0) && canSelect)
        {
            RaycastMousePosition();
        }

        if(Input.GetMouseButtonUp(0))
        {
            if(selectedCubes.Count > 1)
            {
                canSelect = false;

                foreach (GameObject cube in selectedCubes)
                {
                    Vector3 pos = new Vector3(cube.transform.position.x, cube.transform.position.y, 0f);
                    Instantiate(cubePrefab, Vector3Int.RoundToInt(pos), Quaternion.identity);
                }

                ClearCubes();
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
            }
        }
    }

    public void IncreaseSelectableCubeCount()
    {
        selectableCubeCount += 1;

        if (selectableCubeCount > maxCubeCount) selectableCubeCount = maxCubeCount;
    }

    public void DecreaseSelectableCubeCount()
    {
        selectableCubeCount -= 1;

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
        }

        selectedCubes.Clear();
        selectedCubeCount = 0;
    }
}
