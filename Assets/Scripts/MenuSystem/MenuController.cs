using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] protected GameObject[] _menuGroups;
    [SerializeField] protected Image _progressBar;
    [SerializeField] protected float _target;
    


    void Start()
    {
        DisableAllGroups();
        _menuGroups[0].SetActive(true);
    }
    void DisableAllGroups()
    {
        foreach (var group in _menuGroups)
            group.gameObject.SetActive(false);
    }

    public void OnBackButton()
    {
        DisableAllGroups();
        _menuGroups[0].SetActive(true);
    }

    public void OnPlayButton()
    {
        Debug.Log("JOGAR");
        StartCoroutine(LoadScene());
    }

    public void OnCreditsButton()
    {
        DisableAllGroups();
        _menuGroups[2].SetActive(true);
    }

    public void OnControlsButton()
    {
        DisableAllGroups();
        _menuGroups[3].SetActive(true );
    }

    public void OnLoadGameButton()
    {
        DisableAllGroups();
        _menuGroups[1].SetActive(true);
    }

    public IEnumerator LoadScene()
    {
        Debug.Log("JOGAR");
        DisableAllGroups();
        _target = 0;
        _progressBar.fillAmount = 0;

        _menuGroups[4].SetActive(true);

        _target = 1;
        yield return new WaitUntil(() => _progressBar.fillAmount >= 0.95);

        _menuGroups[4].SetActive(false);
        SceneManager.LoadScene(1);
    }

    protected void Update()
    {
        _progressBar.fillAmount = Mathf.MoveTowards(_progressBar.fillAmount, _target, 3 * Time.deltaTime);
    }
}
