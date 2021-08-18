using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void NextScene()
    {
        SceneManager.LoadScene("Game");
    }
    public void StartScene()
    {
        SceneManager.LoadScene("Menu");
    }
}
