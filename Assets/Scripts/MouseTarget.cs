using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTarget : MonoBehaviour
{
    public GameObject self;
    private Vector3 place;
    private GameObject objectToPlace;
    
    private RaycastHit hit;

    public string canPlace;

    // Start is called before the first frame update
    void Start()
    {
        canPlace = "§";
    }

    // Update is called once per frame
    void Update()
    {
        PlaceObjectAtMousePos();
    }

    private void PlaceObjectAtMousePos()
    {
        if (Input.GetMouseButtonDown(0) && canPlace != "§")
        {
            objectToPlace = GameObject.FindGameObjectWithTag(canPlace);
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.transform.CompareTag("Land"))
                {
                    place = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                    Quaternion orientation = Quaternion.identity;
                    if (canPlace == "Animal")
                    {
                        orientation = Quaternion.Euler(270f, 0f, 0f);
                    }
                    Instantiate(objectToPlace, place, orientation);
                    canPlace = "§";
                }
            }
        }
    }

    public void PlaceTree() { canPlace = "Tree"; }

    public void PlaceBerries() { canPlace = "Berries"; }

    public void PlaceAnimal() { canPlace = "Animal"; }
}

