using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneHandler : MonoBehaviour
{
    public Button playButton;
    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.RemoveAllListeners();
        playButton.onClick.AddListener(() =>
            {
                LoadScene();
            });
    }

    private void LoadScene()
    {
        SceneManager.LoadSceneAsync("GamePlay");
    }
}
