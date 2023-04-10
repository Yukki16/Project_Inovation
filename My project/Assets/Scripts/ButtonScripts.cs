using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScripts : MonoBehaviour
{
    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
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
