using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;

// Used for the Hat selection logic
public class PlayerConfiguratorLabel : MonoBehaviour
{
    [SerializeField]
    private Transform m_HatAnchor;

    private GameObject m_HatInstance;

    private AsyncOperationHandle<IList<GameObject>> m_HatsLoadOpHandle;

    // private List<string> m_Keys = new List<string>() { "Hats" };
    private List<string> m_Keys = new List<string>() {"Hats", "Seasonal"};

    void Start()
    {
        // m_HatsLoadOpHandle = Addressables.LoadAssetsAsync<GameObject>(m_Keys, null, Addressables.MergeMode.Union);
        m_HatsLoadOpHandle = Addressables.LoadAssetsAsync<GameObject>(m_Keys, null, Addressables.MergeMode.Intersection);
        m_HatsLoadOpHandle.Completed += OnHatsLoadComplete;
    }

    private void LoadInRandomHat(IList<GameObject> prefabs)
    {
        int randomIndex = Random.Range(0, prefabs.Count);
        GameObject randomHatPrefab = prefabs[randomIndex];
        m_HatInstance = Instantiate(randomHatPrefab, m_HatAnchor);
    }

    private void OnHatsLoadComplete(AsyncOperationHandle<IList<GameObject>> asyncOperationHandle)
    {
        Debug.Log("AsyncOperationHandle Status: " + asyncOperationHandle.Status);

        if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
        {
            IList<GameObject> results = asyncOperationHandle.Result;
            for (int i = 0; i < results.Count; i++)
            {
                Debug.Log("Hat: " + results[i].name);
            }

            LoadInRandomHat(results);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            Destroy(m_HatInstance);

            LoadInRandomHat(m_HatsLoadOpHandle.Result);
        }
    }

    private void OnDisable()
    {
        m_HatsLoadOpHandle.Completed -= OnHatsLoadComplete;
    }
}
