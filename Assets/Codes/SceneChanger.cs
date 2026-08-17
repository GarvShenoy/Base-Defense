using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string sceneName;

    //Uses sceneName to load a scene when called.
    public void NextScene() 
    {
        SceneManager.LoadScene(sceneName);
    }
}
