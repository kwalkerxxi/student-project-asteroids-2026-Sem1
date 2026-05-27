using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigation : MonoBehaviour
{
    [SerializeField] string additionalSceneToLoad = "";

    private void Start()
    {
        if(additionalSceneToLoad != "")
        {
            SceneAdd(additionalSceneToLoad);
        }
    }
    public void SceneChange(string sceneToChangeTo)
    {
        SceneManager.LoadSceneAsync(sceneToChangeTo, LoadSceneMode.Single);

    }

    public void SceneAdd(string sceneToAdd)
    {
        SceneManager.LoadSceneAsync(sceneToAdd, LoadSceneMode.Additive);

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
