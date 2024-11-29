using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // UnityのSceneManagerを使用するためのインポート


public class Scenetransition : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        // UnityのSceneManagerを使ってシーンをロード
        SceneManager.LoadScene(sceneName);
    }
}
