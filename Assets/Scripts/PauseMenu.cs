using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPause;
    [SerializeField] protected GameObject PauseMenuUI;
    [SerializeField] protected GameObject InventoryUI;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESC!");
            if(GameIsPause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        if(InventoryUI.activeSelf)
            InventoryUI.SetActive(false);
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPause = false;
    }
    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPause = true;
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene");
    }

    public void OpenInventory()
    {
        if (InventoryUI.activeSelf)
            InventoryUI.SetActive(false);
        else
            InventoryUI.SetActive(true);
    }

    public void OpenQuests()
    {
        Debug.Log("Missões");
    }


}
