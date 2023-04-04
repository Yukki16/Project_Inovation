using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssetBundle : MonoBehaviour
{
    [MenuItem("Assets/Create/AssetBuilding")]
    static void BuildMapABs()
    {
        // Create the array of bundle build details.
        AssetBundleBuild[] buildMap = new AssetBundleBuild[3];

        buildMap[0].assetBundleName = "GlidingGameBundle";

        string[] glidingAssets = new string[1];
        glidingAssets[0] = "Assets/Prefabs/Cube.prefab";

        buildMap[0].assetNames = glidingAssets;

        //////////////////////////////////////////////
        buildMap[1].assetBundleName = "TowerClimb";

        string[] tower = new string[1];
        tower[0] = "Assets/Prefabs/Player.prefab";

        buildMap[1].assetNames = tower;

        ///////////////////////////////////////////////////
        buildMap[2].assetBundleName = "MaterialsDependecies";
        string[] materials = new string[1];
        materials[0] = "Assets/Materials/TestingMaterial.mat";


        buildMap[2].assetNames = materials;

        /////////////////////////////////////////
        /*buildMap[3].assetBundleName = "ScriptDependencies";
        string[] scripts = new string[3];
        scripts[0] = "Assets/Scripts/TowerClimb/Player.cs";
        scripts[1] = "Assets/Mirror/Components/NetworkTransformReliable.cs";
        scripts[2] = "Assets/Mirror/Core/NetworkIdentity.cs";


        buildMap[3].assetNames = scripts;*/


        BuildPipeline.BuildAssetBundles("Assets/StreamingAssets", buildMap, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows | BuildTarget.Android);
    }
}
