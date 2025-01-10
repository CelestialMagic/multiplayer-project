using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed, rotationSpeed;

    [SerializeField]
    private InputAction forwardMovement, sideMovement;

    private Vector3 movementForce; 

    [SerializeField]
    private GameObject prefab;

    

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float sideInput = sideMovement.ReadValue<float>();
        float forwardInput = forwardMovement.ReadValue<float>();

        movementForce = new Vector3(forwardInput * moveSpeed * Time.deltaTime, 0, sideInput * moveSpeed * Time.deltaTime);

        Vector3 directionToMove = new Vector3(forwardInput, 0, sideInput);
        transform.Translate(movementForce, Space.World);

        if(movementForce != Vector3.zero && directionToMove != Vector3.zero){
            Quaternion playerRotation = Quaternion.LookRotation(directionToMove, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, playerRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
