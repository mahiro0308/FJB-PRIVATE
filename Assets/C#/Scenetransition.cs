using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Unity��SceneManager���g�p���邽�߂̃C���|�[�g


public class Scenetransition : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        // Unity��SceneManager���g���ăV�[�������[�h
        SceneManager.LoadScene(sceneName);
    }
}
