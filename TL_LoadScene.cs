using UnityEngine.SceneManagement;
using UnityEngine;

public class TL_LoadScene : MonoBehaviour
{
    public void LoadNextScene(string NextSceneName)
    {
        SceneManager.LoadScene(NextSceneName);
    }

}
