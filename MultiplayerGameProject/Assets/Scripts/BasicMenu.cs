using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class BasicMenu : MonoBehaviour
{
    [SerializeField]
    private string lobbyName;//The name of the lobby area

    [SerializeField]
    private string howToPlay;//A string to be used for the how to play screen 


//Goes to the lobby scene 
    public void GoToLobby(){
        SceneManager.LoadScene(lobbyName);
    }

//Quits the Game
    public void QuitGame(){
        Application.Quit(); 
    }


    
}
