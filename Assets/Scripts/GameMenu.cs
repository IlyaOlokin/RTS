using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    
    public void ResumeGame()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    
    
    
}
