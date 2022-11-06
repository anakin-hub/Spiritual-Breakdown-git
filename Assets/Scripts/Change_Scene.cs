using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Change_Scene : MonoBehaviour
{
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);//vai carregar a proxima cena
    }

    public void LoadExploreScene()
    {
        SceneManager.LoadScene(1);
    }


}
