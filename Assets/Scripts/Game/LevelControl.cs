using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelControl : MonoBehaviour
{
    [SerializeField] private string _nextScene;

    public void LoadNextScene()
    {
        SceneManager.LoadSceneAsync(_nextScene);
    }
}