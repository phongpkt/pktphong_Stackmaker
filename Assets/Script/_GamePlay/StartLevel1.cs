using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartLevel1 : MonoBehaviour
{
    public void StartGameLevel1()
    {
        SceneManager.LoadScene("SceneLevel1");
    }
}
