using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnAfterTime : MonoBehaviour
{
    public float despawnTime = 10f; // Time in seconds before the glowstick disappears

    void Start()
    {
        // Destroy the GameObject after the specified time
        Destroy(gameObject, despawnTime);
    }
}