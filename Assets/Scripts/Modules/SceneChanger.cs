/*
========================
| Scene changer module |
========================
*/

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    //Simple scene changing
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName); //loading scene by name
    }

    //Scene changing after delay (coroutine).
    public IEnumerator ChangeSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay); //delay
        SceneManager.LoadScene(sceneName); //loading scene by name
    }
}
