using System;
using System.Collections;
using System.Collections.Generic;
using SystemControllers;
using UnityEngine;
using Random = UnityEngine.Random;

public class MouseTarget : MonoBehaviour
{
    public GameObject self;
    private Vector3 place;
    private GameObject objectToPlace;
    
    private RaycastHit hit;

    public string canPlace;
    
    // Public dispatch delegate declaration
    public static event EventHandler<UserClickEventDispatcher> OnClick;

    // Declaration of the dispatched tick event object itself
    public class UserClickEventDispatcher : EventArgs
    {
        public GameObject Clicked;
    }

    // Start is called before the first frame update
    void Start()
    {
        canPlace = "§";
    }

    // Update is called once per frame
    void Update()
    {
        // If clicked and placeholder is there, then get its name, if not, we know the user wants to place something.
        // So invoke the place method in the latter condition
        if (!Input.GetMouseButtonDown(0)) return;
        if (canPlace != "§")
        {
            PlaceObjectAtMousePos();
        }
        else if (canPlace == "§")
        {
            GetObjectAtMousePos();
        }
    }

    // Method to get the name of the object at the mouse position
    private void GetObjectAtMousePos()
    {
        // If blank space is clicked, exit
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) return; 
        // Ignore land
        if (hit.transform.CompareTag("Land")) return;
        
        // On a valid click, send an event broadcast to all subscribed objects
        OnClick?.Invoke(this, new UserClickEventDispatcher
        {
            Clicked = hit.transform.gameObject
        });
        canPlace = "§";
    }
    
    private void PlaceObjectAtMousePos()
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
                        orientation = Quaternion.Euler(270f, Random.Range(0, 360), Random.Range(0, 360));
                    }
                    Instantiate(objectToPlace, place, orientation);
                    canPlace = "§";
                }
        }
    }

    public void PlaceTree() { canPlace = "Tree"; }

    public void PlaceBerries() { canPlace = "Berries"; }

    public void PlaceAnimal() { canPlace = "Animal"; }
}

