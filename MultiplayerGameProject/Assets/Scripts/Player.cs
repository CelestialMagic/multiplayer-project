using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    private float x;
    private float y;
    [SerializeField]
    private float sensitivity; 

    private Vector3 rotate;

    [SerializeField]
    private float moveSpeed, rotationSpeed, jumpHeight;

    [SerializeField]
    private InputAction forwardMovement, sideMovement;

    private Vector3 movementForce; 

    [SerializeField]
    private GameObject prefab;

    

    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

    }

     void OnEnable(){
        forwardMovement.Enable();
        sideMovement.Enable();
    }

     void OnDisable(){
        forwardMovement.Disable();
        sideMovement.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        float sideInput = sideMovement.ReadValue<float>();
        float forwardInput = forwardMovement.ReadValue<float>();

        Vector3 directionToMove = new Vector3(sideInput, 0, forwardInput);

        movementForce = new Vector3(sideInput * moveSpeed * Time.deltaTime, 0, forwardInput * moveSpeed * Time.deltaTime);
        transform.Translate(movementForce, Space.World);

        //RotateWithMouse();

    }

    void RotateWithMouse(){
        y = Input.GetAxis("Mouse X");
        x = Input.GetAxis("Mouse Y");
        rotate = new Vector3(x, y * sensitivity, 0);
        Quaternion playerRotation = Quaternion.LookRotation(rotate, Vector3.up);
        
        transform.eulerAngles = transform.eulerAngles - rotate;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, playerRotation, rotationSpeed * Time.deltaTime);
        
        
    }
}
