using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerInfoDisplay : MonoBehaviour
{
    [SerializeField]
    private TMP_Text shellText, infoText;

    [SerializeField]
    private Image shellIcon; 
    
    public void SetCurrentShellDisplay(string shellName, string shellInfo, Sprite shellSprite){
        shellText.text = $"{shellName}";
        infoText.text = $"{shellInfo}";
        shellIcon.sprite = shellSprite; 

    }

    
}
