using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerInfoDisplay : MonoBehaviour
{
    [SerializeField]
    private TMP_Text shellText, infoText, scoreText;

    public TMP_Text rankText; 

    [SerializeField]
    private Image shellIcon; 

    [SerializeField]
    private GameObject descriptionMenu;

    private bool isMenuActive = false;
    
    public void SetScore(int value){
        scoreText.text = $"{value}"; 

    }



    public void SetCurrentShellDisplay(string shellName, string shellInfo, Sprite shellSprite){
        shellText.text = $"{shellName}";
        infoText.text = $"{shellInfo}";
        shellIcon.sprite = shellSprite; 

    }


    public void OnPointerEnter(PointerEventData eventData){
        Debug.Log("Pointer on Icon");
        descriptionMenu.SetActive(true);

    }
    public void OnPointerExit(PointerEventData eventData){
        descriptionMenu.SetActive(false);
        Debug.Log("Pointer off Icon");

    }

    public void OnMouseOver(){
        Debug.Log("Pointer on Icon");
    }

    public void ViewStats(){
        isMenuActive = !isMenuActive;
        descriptionMenu.SetActive(isMenuActive);

    }

    
}
