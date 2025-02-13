using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ThrowObject : MonoBehaviour
{
    [SerializeField]
    private string prefabName; 
    public GameObject objectPrefab; // Assign your glowstick prefab in the Inspector
    public Transform throwPoint; // Assign a transform where the object will be spawned (e.g., player hand)
    public float throwForce = 10f; // Adjust for how far the object is thrown
    public float upwardForce = 5f; // Adjust for arc height

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Throw();
            Debug.Log("Call Throw");
        }
    }

    void Throw()
    {
        if (objectPrefab != null && throwPoint != null)
        {
            // Instantiate the object at the throw point's position and rotation
            GameObject thrownObject = PhotonNetwork.Instantiate(objectPrefab.name, throwPoint.position, throwPoint.rotation);

            // Get the Rigidbody component to apply force
            Rigidbody rb = thrownObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Calculate the throw direction
                Vector3 throwDirection = throwPoint.forward * throwForce + Vector3.up * upwardForce;
                rb.AddForce(throwDirection, ForceMode.Impulse);
            }
        }
    }
}
