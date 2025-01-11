using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

/*
This code largely pertains to a tutorial I found on rotating the player with
the mouse.
*/
    private float x;//X of Mouse
    private float y;//Y of Mouse
    [SerializeField]
    private float sensitivity;//Camera sensitivity 

    private Vector3 rotate;//Stores x + y vector to rotate by


/*
This code largely pertains to a tutorial I found on rotating the player with
the mouse.
*/
    [SerializeField]
    private float moveSpeed, rotationSpeed;//floats representing speed to move by

    [SerializeField]
    private InputAction forwardMovement, sideMovement, meleeMovement;//InputActions to be mapped to player actions

    private Vector3 movementForce;//A force to move by

    [SerializeField]
    private GameObject prefab;//The player's prefab (to be used later for Photon)

    [SerializeField]
    private Color color;//The player color 

    // Start is called before the first frame update
    void Start()
    {
        //Locks cursor for rotating with mouse
        //Cursor.lockState = CursorLockMode.Locked;

//This code will be useful later for when we send RPC calls for players
//joining online 
        MeshRenderer[] childrenRenderers = GetComponentsInChildren<MeshRenderer>();
        Debug.Log(childrenRenderers);
        foreach (MeshRenderer m in childrenRenderers)
        {
            if(m.material.name == "Body (Instance)")
                m.material.color = color;
        }
        
    }

//Enables Input Actions
     void OnEnable(){
        forwardMovement.Enable();
        sideMovement.Enable();
        meleeMovement.Enable();
    }

//Disables Input Actions
     void OnDisable(){
        forwardMovement.Disable();
        sideMovement.Disable();
        meleeMovement.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        //Gets value of key presses for side and forward movement
        float sideInput = sideMovement.ReadValue<float>();
        float forwardInput = forwardMovement.ReadValue<float>();

//Converts this to a vector based on the movement
        Vector3 directionToMove = new Vector3(sideInput, 0, forwardInput);

//Multiplies movement vector by deltaTime and the speed to move by
        movementForce = new Vector3(sideInput * moveSpeed * Time.deltaTime, 0, forwardInput * moveSpeed * Time.deltaTime);
        transform.Translate(movementForce, Space.World);

        //RotateWithMouse();

    }

//Rotates the player with the mouse
    void RotateWithMouse(){
        y = Input.GetAxis("Mouse X");
        x = Input.GetAxis("Mouse Y");
        rotate = new Vector3(x, y * sensitivity, 0);
        Quaternion playerRotation = Quaternion.LookRotation(rotate, Vector3.up);
        
        transform.eulerAngles = transform.eulerAngles - rotate;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, playerRotation, rotationSpeed * Time.deltaTime);
        
        
    }
}
