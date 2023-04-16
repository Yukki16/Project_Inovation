using System;
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

    public void DisconnectServer()
    {
        NetworkManager.Instance.DisconnectServer();
        Loader.Load(Loader.Scene.Lobby);
    }

    public void SetReady()
    {
        NetworkManager.Instance.ServerIsReady();
    }
}
