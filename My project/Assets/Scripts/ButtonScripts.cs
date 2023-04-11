using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScripts : MonoBehaviour
{
    public void LoadScene(string scene)
    {
        if (Enum.TryParse(scene, out Loader.Scene newScene))
        {
            Loader.Load(newScene);
        }
        else
        {
            Debug.LogError("Scene couldn't be loaded because Parse failed.");
        }
    }

    public void CreateLobby()
    {
        GameLobby.Instance.CreateLobbby();
    }

    public void QuickJoin()
    {
        GameLobby.Instance.QuickJoin();
    }
}
