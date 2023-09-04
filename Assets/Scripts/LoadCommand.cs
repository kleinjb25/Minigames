using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadCommand : MonoBehaviour
{
    public void LoadCmdGame()
    {
        SceneManager.LoadScene("CommandTyping");
    }
}
