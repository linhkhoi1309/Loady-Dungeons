using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class Loading : MonoBehaviour
{
    private AsyncOperation m_SceneOperation;

    [SerializeField]
    private Slider m_LoadingSlider;

    [SerializeField]
    private GameObject m_PlayButton, m_LoadingText;

    private static AsyncOperationHandle<SceneInstance> m_SceneLoadOpHandle;

    private void Awake()
    {
        StartCoroutine(loadNextLevel("Level_0" + GameManager.s_CurrentLevel));
    }

    private IEnumerator loadNextLevel(string level)
    {
        // m_SceneOperation = SceneManager.LoadSceneAsync(level);
        // m_SceneOperation.allowSceneActivation = false;
        m_SceneLoadOpHandle = Addressables.LoadSceneAsync(level, activateOnLoad: true);

        // while (!m_SceneOperation.isDone)
        while (!m_SceneLoadOpHandle.IsDone)
        {
            // m_LoadingSlider.value = m_SceneOperation.progress;
            m_LoadingSlider.value = m_SceneLoadOpHandle.PercentComplete;

            // if (m_SceneOperation.progress >= 0.9f && !m_PlayButton.activeInHierarchy)
            if(m_SceneLoadOpHandle.PercentComplete >= 0.9f && !m_PlayButton.activeInHierarchy)
                m_PlayButton.SetActive(true);

            yield return null;
        }

        Debug.Log($"Loaded Level {level}");
    }

    // Function to handle which level is loaded next
    // public void GoToNextLevel()
    // {
    //     m_SceneOperation.allowSceneActivation = true;
    // }
}
