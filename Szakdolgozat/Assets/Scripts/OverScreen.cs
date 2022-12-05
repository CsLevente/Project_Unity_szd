using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OverScreen : MonoBehaviour
{
    GameObject player;
    public void Setup(){
        gameObject.SetActive(true);
    }


    public void RestartButton(){
        if(SceneManager.GetActiveScene().name=="Floor1"){
            GameController.ResetStats();
            SceneManager.LoadScene("Floor1");
        }if(SceneManager.GetActiveScene().name=="Floor2"){
            GameController.ResetStats();
            SceneManager.LoadScene("Floor2");
        }
    }

    public void MainMenuButton(){
        player = GameObject.FindGameObjectWithTag("Player");
        SceneManager.LoadScene("Menu");
        GameController.ResetStats();
    }
}
