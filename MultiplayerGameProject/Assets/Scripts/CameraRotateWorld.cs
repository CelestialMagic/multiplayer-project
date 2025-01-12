using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotateWorld : MonoBehaviour
{
    [SerializeField]
    private Transform orientation, player, playerObj;

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private float rotationSpeed;

private void Start(){
    Cursor.lockState = CursorLockMode.Locked; 
    
}
    void Update(){
        Vector3 viewDirection = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDirection.normalized;

        float sideInput = player.gameObject.GetComponent<Player>().GetSideInput();
        float forwardInput = player.gameObject.GetComponent<Player>().GetForwardInput();

        Vector3 inputDirection = orientation.forward * forwardInput + orientation.right * sideInput;

        if(inputDirection != Vector3.zero)
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDirection.normalized, Time.deltaTime * rotationSpeed);
    }

}
