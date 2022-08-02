using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class LevelChange : MonoBehaviour
{
    [SerializeField] private SceneAsset _nextScene;

    public void LoadNextScene()
    {
        Debug.Log($"Initiated level change.");
        SceneManager.LoadSceneAsync(_nextScene.name);
    }
}