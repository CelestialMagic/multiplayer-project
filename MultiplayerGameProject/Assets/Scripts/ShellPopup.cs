using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShellPopup : MonoBehaviour
{
    [SerializeField]
    private GameObject shellCanvas; 

    [SerializeField]
    private TMP_Text nameText, infoText;

    [SerializeField]
    private Shell shellToDisplay;




    // Start is called before the first frame update
    void Start()
    {
        shellCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        nameText.text = shellToDisplay.GetName();
        infoText.text = shellToDisplay.GetInfo();
    }

    private void OnTriggerEnter(Collider collision){
        shellCanvas.SetActive(true);
    }
    private void OnTriggerExit(Collider collision){
        shellCanvas.SetActive(false);
    }
}
