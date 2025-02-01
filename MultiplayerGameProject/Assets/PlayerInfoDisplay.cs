using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInfoDisplay : MonoBehaviour
{
    [SerializeField]
    private TMP_Text shellText;
    
    public void SetCurrentShellText(string shellName){
        shellText.text = $"Current Shell: {shellName}";

    }

    
}
