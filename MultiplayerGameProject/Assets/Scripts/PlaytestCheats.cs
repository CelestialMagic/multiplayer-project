using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaytestCheats : MonoBehaviour
{
    [SerializeField]
    private GameObject canvasToHide;

    private bool uiHidden = false;
    // Start is called before the first frame update
    void Start()
    {
       
        
    }

    // Update is called once per frame
    void Update()
    {
         if(Input.GetKeyDown(KeyCode.Escape)){
            if(uiHidden){
                canvasToHide.SetActive(true);
                uiHidden = false;

            }
            else{
                canvasToHide.SetActive(false);
                uiHidden = true;

            }
                

            
        }
        
        
    }
}
