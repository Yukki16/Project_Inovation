using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{

    public enum Scene
    {
        MainMenu,
        Lobby,
        TowerClimb,
        Gliding
    }

    public static Scene targetScene;

    public static void Load(Scene targetScene)
    {
        Loader.targetScene = targetScene;

        SceneManager.LoadScene(targetScene.ToString());
    }

    public static void LoaderCallBack()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
