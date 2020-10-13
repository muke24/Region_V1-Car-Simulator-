using Gaia.Internal;
using ProcedualWorlds.WaterSystem;
using UnityEditor;
using UnityEngine;
using UnityEditor.Rendering;
using UnityEngine.Rendering;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.Characters.ThirdPerson;
using System;
using Gaia.Pipeline.HDRP;
using UnityEditor.SceneManagement;
#if HDPipeline
using UnityEngine.Rendering.HighDefinition;
#endif

namespace Gaia
{
    /// <summary>
    /// This class manages all the water and lighting setup using Scene Profiles to manage each scene profiles and profile values.
    /// This allows you to save and load these into Scene Profile Assets and use them in many scenes or share settings in different scene to get the same look and atmosphere feel.
    /// </summary>
    public static class GaiaSceneManagement
    {
        #region Save and Load

        /// <summary>
        /// Load from a profile
        /// </summary>
        /// <param name="path"></param>
        public static void LoadFile(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                path = path.Replace(Application.dataPath, "Assets/");
                SceneProfile asset = AssetDatabase.LoadAssetAtPath<SceneProfile>(path);
                if (asset != null)
                {
                    GaiaSettings settings = GaiaUtils.GetGaiaSettings();
                    GaiaUtils.GetRuntimeSceneObject();
                    if (GaiaGlobal.Instance != null)
                    {
                        if (EditorUtility.DisplayDialog("Loading Profile",
                            "You are about to load a new profile this will override any settings you have already saved in this scene. We recommend saving your profile in this scene before loading from file. Are you sure you want to proceed?",
                            "Yes", "No"))
                        {

                            SceneProfile newSceneProfile = ScriptableObject.CreateInstance<SceneProfile>();
                            if (asset.ProfileVersion != PWApp.CONF.Version)
                            {
                                EditorUtility.DisplayDialog("Incorrect Version",
                                    "The profile you loaded up was built on a older version of Gaia. This will not effect the loading of the profile settings but you may not get some of the new default settings with the new version of Gaia." +
                                    " We recommend you resave this profile on the newest version of Gaia to avoid this message in the future.",
                                    "Ok");
                            }

                            CopyFromProfileToNewProfile(asset, newSceneProfile);
#if GAIA_PRO_PRESENT
                            if (ProceduralWorldsGlobalWeather.Instance != null)
                            {
                                GameObject.DestroyImmediate(ProceduralWorldsGlobalWeather.Instance.gameObject);
                            }
#endif

                            GaiaSessionManager session = GaiaSessionManager.GetSessionManager();
#if UPPipeline
                            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(GaiaGlobal.Instance.SceneProfile.m_universalPostFXProfile));
                            AssetDatabase.Refresh();
                            if (session != null)
                            {
                                string newPath = GaiaDirectories.GetSceneProfilesFolderPath(session.m_session);
                                newPath += "/" + asset.m_universalPostFXProfile.name + ".asset";
                                if (!string.IsNullOrEmpty(newPath))
                                {
                                    FileUtil.CopyFileOrDirectory(AssetDatabase.GetAssetPath(asset.m_universalPostFXProfile), newPath);
                                    AssetDatabase.ImportAsset(newPath);

                                    newSceneProfile.m_universalPostFXProfile = null;
                                    newSceneProfile.m_universalPostFXProfile = AssetDatabase.LoadAssetAtPath<VolumeProfile>(newPath);
                                }
                            }
#endif

#if HDPipeline
                            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(GaiaGlobal.Instance.SceneProfile.m_highDefinitionPostFXProfile));
                            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(GaiaGlobal.Instance.SceneProfile.m_highDefinitionLightingProfile));
                            AssetDatabase.Refresh();
                            if (session != null)
                            {
                                //Post FX
                                string newPath = GaiaDirectories.GetSceneProfilesFolderPath(session.m_session);
                                newPath += "/" + asset.m_highDefinitionPostFXProfile.name + ".asset";
                                if (!string.IsNullOrEmpty(newPath))
                                {
                                    if (string.IsNullOrEmpty(AssetDatabase.GetAssetPath(asset.m_highDefinitionPostFXProfile)))
                                    {
                                        FileUtil.CopyFileOrDirectory(AssetDatabase.GetAssetPath(asset.m_highDefinitionPostFXProfile), newPath);
                                        AssetDatabase.ImportAsset(newPath);
                                    }

                                    newSceneProfile.m_highDefinitionPostFXProfile = null;
                                    newSceneProfile.m_highDefinitionPostFXProfile = AssetDatabase.LoadAssetAtPath<VolumeProfile>(newPath);
                                }

                                //Lighting
                                string newPath1 = GaiaDirectories.GetSceneProfilesFolderPath(session.m_session);
                                newPath1 += "/" + asset.m_highDefinitionLightingProfile.name + ".asset";
                                if (!string.IsNullOrEmpty(newPath1))
                                {
                                    if (string.IsNullOrEmpty(AssetDatabase.GetAssetPath(asset.m_highDefinitionPostFXProfile)))
                                    {
                                        FileUtil.CopyFileOrDirectory(AssetDatabase.GetAssetPath(asset.m_highDefinitionLightingProfile), newPath1);
                                        AssetDatabase.ImportAsset(newPath1);
                                    }

                                    newSceneProfile.m_highDefinitionLightingProfile = null;
                                    newSceneProfile.m_highDefinitionLightingProfile = AssetDatabase.LoadAssetAtPath<VolumeProfile>(newPath1);
                                }
                            }
#endif
                            if (settings != null)
                            {
                                SetSceneProfile(newSceneProfile);
                                LoadGlobalSettings(GaiaGlobal.Instance.SceneProfile);

                                if (GaiaGlobal.Instance.SceneProfile.m_lightSystemMode !=
                                    GaiaConstants.GlobalSystemMode.ThirdParty)
                                {
                                    GaiaLighting.GetProfile(GaiaGlobal.Instance.SceneProfile,
                                        settings.m_pipelineProfile,
                                        settings.m_pipelineProfile.m_activePipelineInstalled);
                                    GaiaGlobal.Instance.SceneProfile.DefaultLightingSet = true;
                                }

                                if (GaiaGlobal.Instance.SceneProfile.m_waterSystemMode !=
                                    GaiaConstants.GlobalSystemMode.ThirdParty)
                                {
                                    Material waterMat = GaiaWater.GetGaiaOceanMaterial();
                                    GaiaWater.GetProfile(
                                        GaiaGlobal.Instance.SceneProfile.m_selectedWaterProfileValuesIndex, waterMat,
                                        GaiaGlobal.Instance.SceneProfile, true, false);
                                    GaiaGlobal.Instance.SceneProfile.DefaultWaterSet = true;
                                }

                                EditorUtility.SetDirty(GaiaGlobal.Instance.SceneProfile);

                                Debug.Log("File loaded sucessfully");
                            }
                        }
                    }
                }
                else
                {
                    Debug.LogError("Unable to load asset from path. Please make sure you load up a Scene Profile file");
                }
            }
            else
            {
                Debug.LogError("Load file path returns empty. Please provide a valid path");
            }
        }
        /// <summary>
        /// Load from a profile
        /// </summary>
        /// <param name="path"></param>
        public static void LoadLighting(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                path = path.Replace(Application.dataPath, "Assets/");
                SceneProfile asset = AssetDatabase.LoadAssetAtPath<SceneProfile>(path);
                if (asset != null)
                {
                    GaiaSettings settings = GaiaUtils.GetGaiaSettings();
                    GaiaUtils.GetRuntimeSceneObject();
                    if (GaiaGlobal.Instance != null)
                    {
                        if (EditorUtility.DisplayDialog("Loading Profile",
                            "You are about to load lighting from this profile this will override any settings you have already saved in this scene. We recommend saving your profile in this scene before loading from file. Are you sure you want to proceed?",
                            "Yes", "No"))
                        {
                            SceneProfile newSceneProfile = ScriptableObject.CreateInstance<SceneProfile>();
                            if (asset.ProfileVersion != PWApp.CONF.Version)
                            {
                                EditorUtility.DisplayDialog("Incorrect Version",
                                    "The profile you loaded up was built on a older version of Gaia. This will not effect the loading of the profile settings but you may not get some of the new default settings with the new version of Gaia." +
                                    " We recommend you resave this profile on the newest version of Gaia to avoid this message in the future.",
                                    "Ok");
                            }
                            CopyFromProfileToNewProfileLighting(asset, newSceneProfile);
                            CopyFromProfileToNewProfileWater(GaiaGlobal.Instance.SceneProfile, newSceneProfile);
                            if (settings != null)
                            {
                                SetSceneProfile(newSceneProfile);
                                LoadGlobalSettings(GaiaGlobal.Instance.SceneProfile);
                                GaiaLighting.GetProfile(GaiaGlobal.Instance.SceneProfile, settings.m_pipelineProfile, settings.m_pipelineProfile.m_activePipelineInstalled);
                                GaiaGlobal.Instance.SceneProfile.DefaultLightingSet = true;

                                Debug.Log("File loaded sucessfully");
                            }
                        }
                    }
                }
                else
                {
                    Debug.LogError("Unable to load asset from path. Please make sure you load up a Scene Profile file");
                }
            }
            else
            {
                Debug.LogError("Load file path return back null. Please provide a valid path");
            }
        }
        /// <summary>
        /// Load from a profile
        /// </summary>
        /// <param name="path"></param>
        public static void LoadWater(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                path = path.Replace(Application.dataPath, "Assets/");
                SceneProfile asset = AssetDatabase.LoadAssetAtPath<SceneProfile>(path);
                if (asset != null)
                {
                    GaiaSettings settings = GaiaUtils.GetGaiaSettings();
                    GaiaUtils.GetRuntimeSceneObject();
                    if (GaiaGlobal.Instance != null)
                    {
                        if (EditorUtility.DisplayDialog("Loading Profile",
                            "You are about to load water from this profile this will override any settings you have already saved in this scene. We recommend saving your profile in this scene before loading from file. Are you sure you want to proceed?",
                            "Yes", "No"))
                        {

                            SceneProfile newSceneProfile = ScriptableObject.CreateInstance<SceneProfile>();
                            if (asset.ProfileVersion != PWApp.CONF.Version)
                            {
                                EditorUtility.DisplayDialog("Incorrect Version",
                                    "The profile you loaded up was built on a older version of Gaia. This will not effect the loading of the profile settings but you may not get some of the new default settings with the new version of Gaia." +
                                    " We recommend you resave this profile on the newest version of Gaia to avoid this message in the future.",
                                    "Ok");
                            }
                            CopyFromProfileToNewProfileWater(asset, newSceneProfile);
                            CopyFromProfileToNewProfileLighting(GaiaGlobal.Instance.SceneProfile, newSceneProfile);
                            if (settings != null)
                            {
                                SetSceneProfile(newSceneProfile);

                                Material waterMat = GaiaWater.GetGaiaOceanMaterial();
                                GaiaWater.GetProfile(GaiaGlobal.Instance.SceneProfile.m_selectedWaterProfileValuesIndex, waterMat, GaiaGlobal.Instance.SceneProfile, true, false);
                                GaiaGlobal.Instance.SceneProfile.DefaultWaterSet = true;

                                Debug.Log("File loaded sucessfully");
                            }
                        }
                    }
                }
                else
                {
                    Debug.LogError("Unable to load asset from path. Please make sure you load up a Scene Profile file");
                }
            }
            else
            {
                Debug.LogError("Load file path return back null. Please provide a valid path");
            }
        }
        /// <summary>
        /// Saves the profile
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="waterProfile"></param>
        /// <param name="path"></param>
        public static void SaveFile(SceneProfile profile, string path)
        {
            if (profile != null)
            {
                if (!string.IsNullOrEmpty(path))
                {
                    path = path.Replace(Application.dataPath, "Assets/");
                    SceneProfile asset = ScriptableObject.CreateInstance<SceneProfile>();
                    asset.ProfileVersion = PWApp.CONF.Version;
                    asset.m_savedFromScene = EditorSceneManager.GetActiveScene().name;
                    CopyFromProfileToNewProfile(profile, asset);

                    AssetDatabase.CreateAsset(asset, path);
                    AssetDatabase.ImportAsset(path);

#if UPPipeline
                    if (profile.m_universalPostFXProfile != null)
                    {
                        string sourcePath = GaiaUtils.GetAssetPath(profile.m_universalPostFXProfile.name + ".asset");
                        path = path.Replace(".asset", " Post FX Profile.asset");
                        VolumeProfile oldProfile = AssetDatabase.LoadAssetAtPath<VolumeProfile>(path);
                        if (oldProfile != null)
                        {
                            AssetDatabase.DeleteAsset(path);
                            AssetDatabase.Refresh();
                        }
                        FileUtil.CopyFileOrDirectory(sourcePath, path);
                        AssetDatabase.ImportAsset(path);
                        asset.m_universalPostFXProfile = AssetDatabase.LoadAssetAtPath<VolumeProfile>(path);
                    }
#endif

#if HDPipeline

                    if (profile.m_highDefinitionPostFXProfile != null)
                    {
                        string sourcePath = GaiaUtils.GetAssetPath(profile.m_highDefinitionPostFXProfile.name + ".asset");
                        path = path.Replace(".asset", " Post FX Profile.asset");
                        VolumeProfile oldProfile = AssetDatabase.LoadAssetAtPath<VolumeProfile>(path);
                        if (oldProfile != null)
                        {
                            AssetDatabase.DeleteAsset(path);
                            AssetDatabase.Refresh();
                        }
                        FileUtil.CopyFileOrDirectory(sourcePath, path);
                        AssetDatabase.ImportAsset(path);
                        asset.m_highDefinitionPostFXProfile = AssetDatabase.LoadAssetAtPath<VolumeProfile>(path);
                    }

                    if (profile.m_highDefinitionLightingProfile != null)
                    {
                        string sourcePath = GaiaUtils.GetAssetPath(profile.m_highDefinitionLightingProfile.name + ".asset");
                        path = path.Replace("Post FX ", "Lighting ");
                        VolumeProfile oldProfile = AssetDatabase.LoadAssetAtPath<VolumeProfile>(path);
                        if (oldProfile != null)
                        {
                            AssetDatabase.DeleteAsset(path);
                            AssetDatabase.Refresh();
                        }
                        FileUtil.CopyFileOrDirectory(sourcePath, path);
                        AssetDatabase.ImportAsset(path);
                        asset.m_highDefinitionLightingProfile = AssetDatabase.LoadAssetAtPath<VolumeProfile>(path);
                    }
#endif

                    EditorUtility.SetDirty(asset);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    EditorUtility.FocusProjectWindow();

                    Debug.Log("File saved sucessfully");
                }
                else
                {
                    Debug.LogError("Save path is empty please choose a valid path");
                }
            }
            else
            {
                Debug.LogError("Scene profile was null while trying to save to file. Please go to gaia Lighting default profile and apply any changes to update the Scene Profile in your current scene.");
            }
        }
        /// <summary>
        /// Saves the lighting profile to gaia global
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="waterProfile"></param>
        public static void SaveToProfile(GaiaLightingProfile profile)
        {
            if (profile != null)
            {
                if (GaiaGlobal.Instance != null)
                {
                    if (!GaiaGlobal.Instance.SceneProfile.DefaultLightingSet)
                    {
                        SceneProfile sceneProfile = ScriptableObject.CreateInstance<SceneProfile>();
                        CopySettingsTo(profile, sceneProfile);
                        if (GaiaGlobal.Instance.SceneProfile.DefaultWaterSet)
                        {
                            CopyFromProfileToNewProfileWater(GaiaGlobal.Instance.SceneProfile, sceneProfile);
                        }
                        SetSceneProfile(sceneProfile);
                        GaiaGlobal.Instance.SceneProfile.ProfileVersion = PWApp.CONF.Version;
                        GaiaGlobal.Instance.SceneProfile.DefaultLightingSet = true;
                    }

                    if (GaiaGlobal.Instance.SceneProfile.ProfileVersion != PWApp.CONF.Version)
                    {
                        Debug.Log("Scene Profile version updated from " + GaiaGlobal.Instance.SceneProfile.ProfileVersion + " To " + PWApp.CONF.Version);
                        GaiaGlobal.Instance.SceneProfile.ProfileVersion = PWApp.CONF.Version;
                    }

                    EditorUtility.SetDirty(GaiaGlobal.Instance);
                }
            }
        }
        /// <summary>
        /// Saves the water profile to gaia global
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="waterProfile"></param>
        public static void SaveToProfile(GaiaWaterProfile profile)
        {
            if (profile != null)
            {
                if (GaiaGlobal.Instance != null)
                {
                    if (!GaiaGlobal.Instance.SceneProfile.DefaultWaterSet)
                    {
                        SceneProfile sceneProfile = ScriptableObject.CreateInstance<SceneProfile>();
                        CopySettingsTo(profile, sceneProfile);
                        if (GaiaGlobal.Instance.SceneProfile.DefaultLightingSet)
                        {
                            CopyFromProfileToNewProfileLighting(GaiaGlobal.Instance.SceneProfile, sceneProfile);
                        }
                        SetSceneProfile(sceneProfile);
                        GaiaGlobal.Instance.SceneProfile.ProfileVersion = PWApp.CONF.Version;
                        GaiaGlobal.Instance.SceneProfile.DefaultWaterSet = true;
                    }

                    if (GaiaGlobal.Instance.SceneProfile.ProfileVersion != PWApp.CONF.Version)
                    {
                        Debug.Log("Scene Profile version updated from " + GaiaGlobal.Instance.SceneProfile.ProfileVersion + " To " + PWApp.CONF.Version);
                        GaiaGlobal.Instance.SceneProfile.ProfileVersion = PWApp.CONF.Version;
                    }

                    EditorUtility.SetDirty(GaiaGlobal.Instance);
                }
            }
        }
        /// <summary>
        /// Reverts all settings in the scene back to gaia profile defaults
        /// </summary>
        /// <param name="profile"></param>
        public static void Revert(SceneProfile profile)
        {
            if (profile == null)
            {
                return;
            }

            GaiaSettings settings = GaiaUtils.GetGaiaSettings();
            if (settings == null)
            {
                return;
            }

            if (EditorUtility.DisplayDialog("Reverting",
                "You are about to revert your settings back to the defaults of Gaia Profiles. Are you sure you want to proceed?",
                "Yes", "No"))
            {

                SceneProfile newSceneProfile = ScriptableObject.CreateInstance<SceneProfile>();
                CopySettingsTo(settings.m_gaiaLightingProfile, profile);
                CopySettingsTo(settings.m_gaiaWaterProfile, profile);

                CopyFromProfileToNewProfile(profile, newSceneProfile);
                SetSceneProfile(newSceneProfile);

                if (GaiaGlobal.Instance != null)
                {
                    GaiaUtils.GetRuntimeSceneObject();
                    GaiaLighting.GetProfile(GaiaGlobal.Instance.SceneProfile, settings.m_pipelineProfile, settings.m_pipelineProfile.m_activePipelineInstalled);
                    GaiaGlobal.Instance.SceneProfile.DefaultLightingSet = true;

                    Material waterMat = GaiaWater.GetGaiaOceanMaterial();
                    GaiaWater.GetProfile(GaiaGlobal.Instance.SceneProfile.m_selectedWaterProfileValuesIndex, waterMat, GaiaGlobal.Instance.SceneProfile, true, false);
                    GaiaGlobal.Instance.SceneProfile.DefaultWaterSet = true;

                    Debug.Log("Revert sucessful");
                }
            }
        }

        #endregion

        #region Utils

        /// <summary>
        /// Selects HDRP lighting gameobject
        /// </summary>
        public static void SelectHDRPLighting()
        {
            GameObject hdrpLighting = GameObject.Find("HD Environment Volume");
            if (hdrpLighting != null)
            {
                Selection.activeGameObject = hdrpLighting;
            }
        }
        /// <summary>
        /// Create a player
        /// </summary>
        public static GameObject CreatePlayer(GaiaSettings gaiaSettings, string overrideName = "", bool spawnAtLocation = false, GameObject customPlayerObject = null, Camera customPlayerCamera = null, bool updateAutoDOF = false)
        {
            //Gaia Settings to check pipeline selected
            if (gaiaSettings == null)
            {
                Debug.LogWarning("Gaia Settings are missing from your project, please make sure Gaia settings is in your project.");
                return null;
            }

            //Only do this if we have 1 terrain
            if (DisplayErrorIfNotMinimumTerrainCount(1))
            {
                return null;
            }

            GaiaSceneInfo sceneinfo = GaiaSceneInfo.GetSceneInfo();
            GameObject playerObj = null;
            string playerPrefabName = gaiaSettings.m_currentPlayerPrefabName;
            if (overrideName.Length > 0)
            {
                playerPrefabName = overrideName;
            }

            //If nothing selected then make the default the fly cam
            if (string.IsNullOrEmpty(playerPrefabName))
            {
                playerPrefabName = "FlyCam";
            }

            //Get the centre of world at game height plus a bit
            Vector3 location = GetLocation(playerPrefabName, spawnAtLocation, sceneinfo, null);
            if (location == Vector3.zero)
            {
                Debug.LogWarning("There was no active terrain in the scene to place the player on. The player will be created at the scene origin (X=0 Y=0 Z=0)");
            }

            bool dynamicLoadedTerrains = GaiaUtils.HasDynamicLoadedTerrains();
            if (playerPrefabName == "Custom")
            {
                SetupPlayer(customPlayerObject, dynamicLoadedTerrains);
                SetupCamera(customPlayerCamera);
            }
            else
            {
                GameObject mainCam = GameObject.Find("Main Camera");
                if (mainCam == null)
                {
                    mainCam = GameObject.Find("Camera");
                }
                GameObject firstPersonController = GameObject.Find("FPSController");
                GameObject thirdPersonController = GameObject.Find("ThirdPersonController");
                GameObject flyCamController = GameObject.Find("FlyCam");
                GameObject flyCamControllerUI = GameObject.Find("FlyCamera UI");
#if GAIA_XR
                GameObject XRController = GameObject.Find("XRController");
#endif

                if (mainCam != null)
                {
                    GameObject.DestroyImmediate(mainCam);
                }
                if (firstPersonController != null)
                {
                    location = GetLocation(playerPrefabName, spawnAtLocation, sceneinfo, firstPersonController);
                    GameObject.DestroyImmediate(firstPersonController);
                }
                if (thirdPersonController != null)
                {
                    location = GetLocation(playerPrefabName, spawnAtLocation, sceneinfo, thirdPersonController);
                    GameObject.DestroyImmediate(thirdPersonController);
                }
                if (flyCamController != null)
                {
                    location = GetLocation(playerPrefabName, spawnAtLocation, sceneinfo, flyCamController);
                    GameObject.DestroyImmediate(flyCamController);
                }
                if (flyCamControllerUI != null)
                {
                    GameObject.DestroyImmediate(flyCamControllerUI);
                }
#if GAIA_XR
                if (XRController != null)
                {
                    GameObject.DestroyImmediate(XRController);
                }
#endif


                //fixed Distance of 2000 for the sky dome to be visible always.
                float cameraDistance = 2000;
                switch (gaiaSettings.m_currentController)
                {
                    //Create the player
                    case GaiaConstants.EnvironmentControllerType.FlyingCamera:
                        playerObj = SetupFlyCam(playerObj, flyCamControllerUI, cameraDistance, gaiaSettings, location, sceneinfo);
                        break;
                    case GaiaConstants.EnvironmentControllerType.FirstPerson:
                        playerObj = SetupFPSController(playerPrefabName, playerObj, location, cameraDistance, gaiaSettings);
                        break;
                    case GaiaConstants.EnvironmentControllerType.ThirdPerson:
                        playerObj = SetupThirdPersonController(playerPrefabName, playerObj, mainCam, location, cameraDistance, gaiaSettings, dynamicLoadedTerrains);
                        break;
#if GAIA_XR
                    case GaiaConstants.EnvironmentControllerType.XRController:
                        cameraDistance = 2500;
                        playerObj = SetupXRController(playerPrefabName, playerObj, mainCam, location, cameraDistance, gaiaSettings, dynamicLoadedTerrains);
                        break;
#endif
                }

                if (dynamicLoadedTerrains)
                {
#if GAIA_PRO_PRESENT
                    Terrain terrain = TerrainHelper.GetActiveTerrain();
                    TerrainLoader loader = playerObj.GetComponent<TerrainLoader>();
                    if (loader == null)
                    {
                        loader = playerObj.AddComponent<TerrainLoader>();
                    }
                    loader.LoadMode = LoadMode.RuntimeAlways;
                    float size = terrain.terrainData.size.x * 1.25f * 2f;
                    loader.m_loadingBounds = new BoundsDouble(playerObj.transform.position, new Vector3(size, size, size));
#endif
                }
            }

            SetupFloatingPoint(playerObj, gaiaSettings);
            if (gaiaSettings.m_enableLocationManager)
            {
                LocationManagerEditor.AddLocationSystem();
            }
            else
            {
                LocationManagerEditor.RemoveLocationSystem();
            }

            if (GaiaUtils.CheckIfSceneProfileExists())
            {
                if (updateAutoDOF)
                {
#if UNITY_POST_PROCESSING_STACK_V2
                    SetupAutoDepthOfField(GaiaGlobal.Instance.SceneProfile);
#endif
                }
               
                GaiaGlobal.Instance.SceneProfile.m_controllerType = gaiaSettings.m_currentController;
            }

            if (!Application.isPlaying)
            {
                GaiaScenePlayer.SetCurrentControllerType(gaiaSettings.m_currentController);
            }

            GaiaEditorUtils.MarkSceneDirty();

            return playerObj;
        }
#if UNITY_POST_PROCESSING_STACK_V2
        public static AutoDepthOfField SetupAutoDepthOfField(SceneProfile profile)
        {
            if (profile != null)
            {
                Camera camera = null;
                if (GaiaUtils.CheckIfSceneProfileExists())
                {
                    camera = GaiaGlobal.Instance.m_mainCamera;
                }
                if (camera != null)
                {
                    AutoDepthOfField autoDepth = camera.GetComponent<AutoDepthOfField>();
                    if (profile.m_enableAutoDOF)
                    {
                        if (autoDepth == null)
                        {
                            autoDepth = camera.gameObject.AddComponent<AutoDepthOfField>();
                            autoDepth.SetupAutoFocus();
                            if (!Application.isPlaying)
                            {
                                autoDepth.GetCurrentDepthOfFieldSettings();
                            }
                        }

                        if (autoDepth.m_sourceCamera == null)
                        {
                            autoDepth.m_sourceCamera = camera;
                        }

                        autoDepth.m_renderPipeLine = GaiaUtils.GetActivePipeline();
                    }
                    else
                    {
                        if (autoDepth != null)
                        {
                            GameObject.DestroyImmediate(autoDepth);
                        }
                    }

                    return autoDepth;
                }
            }

            return null;
        }
#endif
        public static void MovePlayerToCamera(GaiaSettings gaiaSettings)
        {
            GameObject playerObject = null;
            GameObject cameraObject = null;
            bool isThirdPerson = false;
            switch (gaiaSettings.m_currentController)
            {
                case GaiaConstants.EnvironmentControllerType.Custom:
                    if (gaiaSettings.m_customPlayerObject != null)
                    {
                        playerObject = GameObject.Find(gaiaSettings.m_customPlayerObject.name);
                    }
                    break;
                case GaiaConstants.EnvironmentControllerType.FirstPerson:
                    playerObject = GameObject.Find("FPSController");
                    break;
                case GaiaConstants.EnvironmentControllerType.ThirdPerson:
                    playerObject = GameObject.Find("ThirdPersonController");
                    cameraObject = GameObject.Find("Main Camera");
                    if (cameraObject == null)
                    {
                        cameraObject = GameObject.Find("Camera");
                    }

                    isThirdPerson = true;
                    break;
                case GaiaConstants.EnvironmentControllerType.FlyingCamera:
                    playerObject = GameObject.Find("FlyCam");
                    break;
            }

            if (playerObject != null)
            {
                if (SceneView.lastActiveSceneView != null)
                {
                    playerObject.transform.position = SceneView.lastActiveSceneView.camera.transform.position;
                    playerObject.transform.eulerAngles = new Vector3(0f, SceneView.lastActiveSceneView.camera.transform.eulerAngles.y, 0f);
                }

                if (isThirdPerson)
                {
                    if (cameraObject != null)
                    {
                        cameraObject.transform.position = new Vector3(playerObject.transform.position.x, playerObject.transform.position.y + 5f, playerObject.transform.position.z - 7f);
                    }
                }
            }

            GaiaEditorUtils.MarkSceneDirty();
        }
        public static void MoveCameraToPlayer(GaiaSettings gaiaSettings)
        {
            GameObject playerObject = null;
            switch (gaiaSettings.m_currentController)
            {
                case GaiaConstants.EnvironmentControllerType.Custom:
                    if (gaiaSettings.m_customPlayerObject != null)
                    {
                        playerObject = GameObject.Find(gaiaSettings.m_customPlayerObject.name);
                    }
                    break;
                case GaiaConstants.EnvironmentControllerType.FirstPerson:
                    playerObject = GameObject.Find("FPSController");
                    break;
                case GaiaConstants.EnvironmentControllerType.ThirdPerson:
                    playerObject = GameObject.Find("ThirdPersonController");
                    break;
                case GaiaConstants.EnvironmentControllerType.FlyingCamera:
                    playerObject = GameObject.Find("FlyCam");
                    break;
            }

            if (playerObject != null)
            {
                if (SceneView.lastActiveSceneView != null)
                {
                    SceneView.lastActiveSceneView.LookAt(playerObject.transform.localPosition, playerObject.transform.localRotation);
                }
            }
        }

        private static GameObject SetupFlyCam(GameObject playerObj, GameObject flyCamControllerUI, float cameraDistance, GaiaSettings gaiaSettings, Vector3 location, GaiaSceneInfo sceneInfo)
        {
            playerObj = new GameObject { name = GaiaConstants.playerFlyCamName, tag = "MainCamera" };
            playerObj.AddComponent<FlareLayer>();
#if !UNITY_2017_1_OR_NEWER
            playerObj.AddComponent<GUILayer>();
#endif
            playerObj.AddComponent<AudioListener>();
            playerObj.AddComponent<FreeCamera>();

            Camera cameraComponent = playerObj.GetComponent<Camera>();
            cameraComponent.farClipPlane = cameraDistance;
            cameraComponent.depthTextureMode = DepthTextureMode.Depth;
            if (gaiaSettings.m_currentRenderer == GaiaConstants.EnvironmentRenderer.Lightweight)
            {
                cameraComponent.allowHDR = false;
                cameraComponent.allowMSAA = true;
            }
            else
            {
                cameraComponent.allowHDR = true;

                var tier1 = EditorGraphicsSettings.GetTierSettings(EditorUserBuildSettings.selectedBuildTargetGroup, GraphicsTier.Tier1);
                var tier2 = EditorGraphicsSettings.GetTierSettings(EditorUserBuildSettings.selectedBuildTargetGroup, GraphicsTier.Tier2);
                var tier3 = EditorGraphicsSettings.GetTierSettings(EditorUserBuildSettings.selectedBuildTargetGroup, GraphicsTier.Tier3);
                if (tier1.renderingPath == RenderingPath.DeferredShading || tier2.renderingPath == RenderingPath.DeferredShading || tier3.renderingPath == RenderingPath.DeferredShading)
                {
                    cameraComponent.allowMSAA = false;
                }
                else
                {
                    cameraComponent.allowMSAA = true;
                }
            }

            //Adds character controller to allow triggers to be used
            CharacterController characterController = playerObj.GetComponent<CharacterController>();
            if (characterController == null)
            {
                characterController = playerObj.AddComponent<CharacterController>();
                characterController.radius = 0.5f;
                characterController.height = 0.5f;
            }

#if GAIA_PRO_PRESENT
            if (GaiaUtils.CheckIfSceneProfileExists())
            {
                GaiaGlobal.Instance.SceneProfile.m_terrainCullingEnabled = true;
            }
#endif
            playerObj.transform.position = location;

            //Set up UI
            if (gaiaSettings.m_flyCamUI == null)
            {
                Debug.LogError("[CreatePlayer()] Fly cam UI has not been assigned in the gaia settings. Assign it then try again");
            }
            else
            {
                flyCamControllerUI = PrefabUtility.InstantiatePrefab(gaiaSettings.m_flyCamUI) as GameObject;
                flyCamControllerUI.name = "FlyCamera UI";
                flyCamControllerUI.transform.SetParent(GaiaUtils.GetPlayerObject().transform);
            }

            FinalizePlayerObject(playerObj, gaiaSettings);
            FinalizeCameraObject(cameraComponent);

            return playerObj;
        }
        private static GameObject SetupFPSController(string playerPrefabName, GameObject playerObj, Vector3 location, float cameraDistance, GaiaSettings gaiaSettings)
        {
            GameObject playerPrefab = PWCommon3.AssetUtils.GetAssetPrefab(playerPrefabName);
            if (playerPrefab != null)
            {
                playerObj = GameObject.Instantiate(playerPrefab, location, Quaternion.identity) as GameObject;
                playerObj.name = GaiaConstants.playerFirstPersonName;
                playerObj.tag = "Player";
                playerObj.transform.position = location;
                if (playerObj.GetComponent<AudioSource>() != null)
                {
                    AudioSource theAudioSource = playerObj.GetComponent<AudioSource>();
                    theAudioSource.volume = 0.125f;
                }
                Camera cameraComponent = playerObj.GetComponentInChildren<Camera>();
                if (cameraComponent != null)
                {
                    cameraComponent.farClipPlane = cameraDistance;
                    if (gaiaSettings.m_currentRenderer == GaiaConstants.EnvironmentRenderer.Lightweight)
                    {
                        cameraComponent.allowHDR = false;
                        cameraComponent.allowMSAA = true;
                    }
                    else
                    {
                        cameraComponent.allowHDR = true;

                        var tier1 = EditorGraphicsSettings.GetTierSettings(EditorUserBuildSettings.selectedBuildTargetGroup, GraphicsTier.Tier1);
                        var tier2 = EditorGraphicsSettings.GetTierSettings(EditorUserBuildSettings.selectedBuildTargetGroup, GraphicsTier.Tier2);
                        var tier3 = EditorGraphicsSettings.GetTierSettings(EditorUserBuildSettings.selectedBuildTargetGroup, GraphicsTier.Tier3);
                        if (tier1.renderingPath == RenderingPath.DeferredShading || tier2.renderingPath == RenderingPath.DeferredShading || tier3.renderingPath == RenderingPath.DeferredShading)
                        {
                            cameraComponent.allowMSAA = false;
                        }
                        else
                        {
                            cameraComponent.allowMSAA = true;
                        }
                    }
                }
#if GAIA_PRO_PRESENT
                //Add the simple terrain culling script, useful in any case
                if (GaiaUtils.CheckIfSceneProfileExists())
                {
                    GaiaGlobal.Instance.SceneProfile.m_terrainCullingEnabled = true;
                }
#endif
                FinalizePlayerObject(playerObj, gaiaSettings);
                FinalizeCameraObject(cameraComponent);

                return playerObj;
            }

            return null;
        }
        private static GameObject SetupThirdPersonController(string playerPrefabName, GameObject playerObj, GameObject mainCam, Vector3 location, float cameraDistance, GaiaSettings gaiaSettings, bool dynamicLoadedTerrains)
        {
            GameObject playerPrefab = PWCommon3.AssetUtils.GetAssetPrefab(playerPrefabName);
            if (playerPrefab != null)
            {
                playerObj = GameObject.Instantiate(playerPrefab, location, Quaternion.identity) as GameObject;
                playerObj.name = GaiaConstants.playerThirdPersonName;
                playerObj.tag = "Player";
                playerObj.transform.position = location;
            }

            mainCam = new GameObject("Main Camera");
            mainCam.transform.position = new Vector3(location.x - 5f, location.y + 5f, location.z - 5f);
            Camera cameraComponent = mainCam.AddComponent<Camera>();
            cameraComponent.farClipPlane = cameraDistance;
            if (gaiaSettings.m_currentRenderer == GaiaConstants.EnvironmentRenderer.Lightweight)
            {
                cameraComponent.allowHDR = false;
                cameraComponent.allowMSAA = true;
            }
            else
            {
                cameraComponent.allowHDR = true;

                var tier1 = EditorGraphicsSettings.GetTierSettings(EditorUserBuildSettings.selectedBuildTargetGroup, GraphicsTier.Tier1);
                var tier2 = EditorGraphicsSettings.GetTierSettings(EditorUserBuildSettings.selectedBuildTargetGroup, GraphicsTier.Tier2);
                var tier3 = EditorGraphicsSettings.GetTierSettings(EditorUserBuildSettings.selectedBuildTargetGroup, GraphicsTier.Tier3);
                if (tier1.renderingPath == RenderingPath.DeferredShading || tier2.renderingPath == RenderingPath.DeferredShading || tier3.renderingPath == RenderingPath.DeferredShading)
                {
                    cameraComponent.allowMSAA = false;
                }
                else
                {
                    cameraComponent.allowMSAA = true;
                }
            }

            CharacterController characterController = mainCam.GetComponent<CharacterController>();
            if (characterController == null)
            {
                characterController = mainCam.AddComponent<CharacterController>();
                characterController.height = 0.5f;
            }
#if GAIA_PRO_PRESENT
            //Add the simple terrain culling script, useful in any case
            if (GaiaUtils.CheckIfSceneProfileExists())
            {
                GaiaGlobal.Instance.SceneProfile.m_terrainCullingEnabled = true;
            }
            //Add the "Wait for terrain loading" script, otherwise character might fall through the terrain
            if (dynamicLoadedTerrains)
            {
                RigidbodyWaitForTerrainLoad waitForLoad = playerObj.GetComponent<RigidbodyWaitForTerrainLoad>();
                if (waitForLoad == null)
                {
                    waitForLoad = playerObj.AddComponent<RigidbodyWaitForTerrainLoad>();
                }
                ThirdPersonCharacter tpc = playerObj.GetComponent<ThirdPersonCharacter>();
                if (tpc != null)
                {
                    tpc.enabled = false;
                    if (!waitForLoad.m_componentsToActivate.Contains(tpc))
                    {
                        waitForLoad.m_componentsToActivate.Add(tpc);
                    }
                }
                ThirdPersonUserControl tpuc = playerObj.GetComponent<ThirdPersonUserControl>();
                if (tpuc != null)
                {
                    tpuc.enabled = false;
                    if (!waitForLoad.m_componentsToActivate.Contains(tpuc))
                    {
                        waitForLoad.m_componentsToActivate.Add(tpuc);
                    }
                }
            }
#endif

#if !UNITY_2017_1_OR_NEWER
            mainCam.AddComponent<GUILayer>();
#endif
            mainCam.AddComponent<FlareLayer>();
            mainCam.AddComponent<AudioListener>();
            mainCam.tag = "MainCamera";

            CameraController cameraController = mainCam.AddComponent<CameraController>();
            cameraController.target = playerObj;
            cameraController.targetHeight = 1.8f;
            cameraController.distance = 5f;
            cameraController.maxDistance = 20f;
            cameraController.minDistance = 2.5f;
            cameraController.Apply();

            FinalizePlayerObject(playerObj, gaiaSettings);
            FinalizeCameraObject(cameraComponent);

            return playerObj;
        }

#if GAIA_XR
        private static GameObject SetupXRController(string playerPrefabName, GameObject playerObj, GameObject mainCam, Vector3 location, float cameraDistance, GaiaSettings gaiaSettings, bool dynamicLoadedTerrains)
        {
            GameObject playerPrefab = PWCommon3.AssetUtils.GetAssetPrefab(playerPrefabName);
            if (playerPrefab != null)
            {
                playerObj = GameObject.Instantiate(playerPrefab, location, Quaternion.identity) as GameObject;
                playerObj.name = GaiaConstants.playerXRName;
                playerObj.tag = "Player";
                playerObj.transform.position = location;
                if (playerObj.GetComponent<AudioSource>() != null)
                {
                    AudioSource theAudioSource = playerObj.GetComponent<AudioSource>();
                    theAudioSource.volume = 0.125f;
                }
                Camera cameraComponent = playerObj.GetComponentInChildren<Camera>();
                if (cameraComponent != null)
                {
                    cameraComponent.farClipPlane = cameraDistance;
                    if (gaiaSettings.m_currentRenderer == GaiaConstants.EnvironmentRenderer.Lightweight)
                    {
                        cameraComponent.allowHDR = false;
                        cameraComponent.allowMSAA = true;
                    }
                    else
                    {
                        cameraComponent.allowHDR = true;

                        var tier1 = EditorGraphicsSettings.GetTierSettings(EditorUserBuildSettings.selectedBuildTargetGroup, GraphicsTier.Tier1);
                        var tier2 = EditorGraphicsSettings.GetTierSettings(EditorUserBuildSettings.selectedBuildTargetGroup, GraphicsTier.Tier2);
                        var tier3 = EditorGraphicsSettings.GetTierSettings(EditorUserBuildSettings.selectedBuildTargetGroup, GraphicsTier.Tier3);
                        if (tier1.renderingPath == RenderingPath.DeferredShading || tier2.renderingPath == RenderingPath.DeferredShading || tier3.renderingPath == RenderingPath.DeferredShading)
                        {
                            cameraComponent.allowMSAA = false;
                        }
                        else
                        {
                            cameraComponent.allowMSAA = true;
                        }
                    }
                }

#if GAIA_PRO_PRESENT
                if (GaiaUtils.CheckIfSceneProfileExists())
                {
                    GaiaGlobal.Instance.SceneProfile.m_terrainCullingEnabled = true;
                }
#endif

                FinalizePlayerObject(playerObj, gaiaSettings);
                FinalizeCameraObject(cameraComponent);
                //Disable Water Reflections in current profile & on exisitng water surfaces
                //gaiaSettings.m_gaiaWaterProfile.m_enableReflections = false;
                //Gaia.GX.ProceduralWorlds.GaiaWaterGX.GX_WaterReflections_DisableReflections();
                AddXRTeleportToTerrains();

                return playerObj;
            }

            return null;
        }

        private static void AddXRTeleportToTerrains()
        {
            if (GaiaUtils.HasDynamicLoadedTerrains())
            {
                GaiaUtils.CallFunctionOnDynamicLoadedTerrains(AddXRTeleportToTerrain, true, null, "Adding XR Teleport Areas...");
            }
            else
            {
                foreach (Terrain t in Terrain.activeTerrains)
                {
                    AddXRTeleportToTerrain(t);
                }
            }
        }

        private static void AddXRTeleportToTerrain(Terrain t)
        {
            var telArea = t.gameObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.TeleportationArea>();

            if (telArea == null)
            {
                telArea = t.gameObject.AddComponent<UnityEngine.XR.Interaction.Toolkit.TeleportationArea>();
            }

            if (telArea != null && telArea.colliders!=null)
            {
                if (!telArea.colliders.Contains(t.GetComponent<Collider>()))
                    telArea.colliders.Add(t.GetComponent<Collider>());
            }
        }
#endif
        private static void FinalizePlayerObject(GameObject playerObj, GaiaSettings gaiaSettings)
        {
            if (playerObj != null)
            {
                playerObj.transform.SetParent(GaiaUtils.GetPlayerObject().transform);
                //Adjust the scene view to see the camera
                if (SceneView.lastActiveSceneView != null)
                {
                    if (gaiaSettings.m_focusPlayerOnSetup)
                    {
                        SceneView.lastActiveSceneView.LookAtDirect(playerObj.transform.position, playerObj.transform.rotation);
                    }
                }
            }

            GaiaSessionManager session = GaiaSessionManager.GetSessionManager();
            if (session != null)
            {
                if (session.m_session != null)
                {
                    if (playerObj.transform.position.y < session.m_session.m_seaLevel)
                    {
                        playerObj.transform.position = new Vector3(playerObj.transform.position.x, session.m_session.m_seaLevel + 5f, playerObj.transform.position.z);
                    }
                }
            }
        }
        private static void FinalizeCameraObject(Camera cameraObject)
        {
            if (cameraObject == null)
            {
                return;
            }

            CharacterController controller = GameObject.FindObjectOfType<CharacterController>();
#if GAIA_PRO_PRESENT
            if (PW_VFX_Clouds.Instance != null)
            {
                PW_VFX_Clouds.Instance.GameCam = cameraObject;
            }

            if (ProceduralWorldsGlobalWeather.Instance != null)
            {
                ProceduralWorldsGlobalWeather.Instance.m_player = cameraObject.transform;
            }

            if (GaiaAudioManager.Instance != null)
            {
                if (controller != null)
                {
                    GaiaAudioManager.Instance.m_player = controller.gameObject;
                }
            }
#endif
            if (PWS_WaterSystem.Instance != null)
            {
                PWS_WaterSystem.Instance.m_RenderCamera = cameraObject;
                PWS_WaterSystem.Instance.m_gameCamera = cameraObject;
                if (controller != null)
                {
                    PWS_WaterSystem.Instance.m_player = controller.transform;
                }
            }

            if (GaiaUnderwaterEffects.Instance != null)
            {
                GaiaUnderwaterEffects.Instance.m_playerCamera = cameraObject.transform;
            }

            if (GaiaGlobal.Instance != null)
            {
                GaiaGlobal.Instance.m_mainCamera = cameraObject;
            }

            ScreenShotter screenShotter = GameObject.FindObjectOfType<ScreenShotter>();
            if (screenShotter != null)
            {
                screenShotter.m_mainCamera = cameraObject;
            }

            FollowPlayerSystem[] followPlayerSystems = GameObject.FindObjectsOfType<FollowPlayerSystem>();
            foreach (var system in followPlayerSystems)
            {
                system.m_player = cameraObject.transform;
            }
        }
        private static Vector3 GetLocation(string playerPrefabType, bool spawnAtLocation, GaiaSceneInfo sceneInfo, GameObject playerObj)
        {
            Vector3 location = Gaia.TerrainHelper.GetWorldCenter(true);
            switch (playerPrefabType)
            {
                //Create the player
                case "FlyCam":
                    //Lift it to about eye height above terrain
                    location.y += 1.8f;
                    if (sceneInfo != null)
                    {
                        if (location.y < sceneInfo.m_seaLevel)
                        {
                            location.y = sceneInfo.m_seaLevel + 1.8f;
                        }
                    }
                    break;
                case "FPSController":
                    location.y += 1f;
                    break;
                case "ThirdPersonController":
                    location.y += 1.5f;
                    location.z -= 5f;
                    break;
            }

            if (spawnAtLocation)
            {
                if (playerObj != null)
                {
                    location = playerObj.transform.position;

                    Terrain t = TerrainHelper.GetTerrain(location);
                    if (t != null)
                    {
                        float height = t.SampleHeight(location);
                        height += 2f;
                        if (playerPrefabType == "ThirdPersonController")
                        {
                            location.z -= 5f;
                        }
                        location = new Vector3(location.x, height, location.z);
                    }
                }
            }

            return location;
        }
        private static void SetupPlayer(GameObject playerObject, bool dynamicLoadedTerrains)
        {
            if (playerObject == null)
            {
                return;
            }

            if (dynamicLoadedTerrains)
            {
#if GAIA_PRO_PRESENT
                RigidbodyWaitForTerrainLoad waitForLoad = playerObject.GetComponent<RigidbodyWaitForTerrainLoad>();
                if (waitForLoad == null)
                {
                    waitForLoad = playerObject.AddComponent<RigidbodyWaitForTerrainLoad>();
                }

                //Add the "Wait for terrain loading" script, otherwise character might fall through the terrain
                FirstPersonController fpsc = playerObject.GetComponent<FirstPersonController>();
                if (fpsc != null)
                {
                    fpsc.enabled = false;
                    if (!waitForLoad.m_componentsToActivate.Contains(fpsc))
                    {
                        waitForLoad.m_componentsToActivate.Add(fpsc);
                    }
                }
#endif
            }
        }
        private static void SetupCamera(Camera cameraObject)
        {
            if (cameraObject == null)
            {
                return;
            }

            if (cameraObject.GetComponent<FlareLayer>() == null)
            {
                cameraObject.gameObject.AddComponent<FlareLayer>();
            }
            if (cameraObject.GetComponent<AudioListener>() == null)
            {
                cameraObject.gameObject.AddComponent<AudioListener>();
            }

#if GAIA_PRO_PRESENT
            //Add the simple terrain culling script, useful in any case
            if (GaiaUtils.CheckIfSceneProfileExists())
            {
                GaiaGlobal.Instance.SceneProfile.m_terrainCullingEnabled = true;
            }
#endif
            FinalizeCameraObject(cameraObject);
        }
        private static void SetupFloatingPoint(GameObject playerObject, GaiaSettings gaiaSettings)
        {
#if GAIA_PRO_PRESENT
            //Do we require the floating point fix component? 
            if (GaiaUtils.UsesFloatingPointFix())
            {
                FloatingPointFix fix = playerObject.GetComponent<FloatingPointFix>();
                if (fix == null)
                {
                    fix = playerObject.AddComponent<FloatingPointFix>();
                }
                fix.threshold = gaiaSettings.m_FPFDefaultThreshold;
            }
#endif
        }
        /// <summary>
        /// Display an error if there is not exactly one terrain
        /// </summary>
        /// <param name="requiredTerrainCount">The amount required</param>
        /// <param name="feature">The feature name</param>
        /// <returns>True if an error, false otherwise</returns>
        public static bool DisplayErrorIfNotMinimumTerrainCount(int requiredTerrainCount, string feature = "")
        {
            int actualTerrainCount = Gaia.TerrainHelper.GetActiveTerrainCount();
            if (actualTerrainCount < requiredTerrainCount)
            {
                if (string.IsNullOrEmpty(feature))
                {
                    if (actualTerrainCount < requiredTerrainCount)
                    {
                        EditorUtility.DisplayDialog("OOPS!", string.Format("You currently have {0} active terrains in your scene, but to " + "use this feature you need at least {1}. Please load in unloaded terrains or create a terrain!", actualTerrainCount, requiredTerrainCount), "OK");
                    }
                }
                else
                {
                    if (actualTerrainCount < requiredTerrainCount)
                    {
                        EditorUtility.DisplayDialog("OOPS!", string.Format("You currently have {0} active terrains in your scene, but to " + "use {2} you need at least {1}.Please load in unloaded terrains or create a terrain!", actualTerrainCount, requiredTerrainCount, feature), "OK");
                    }
                }

                return true;
            }
            return false;
        }
        /// <summary>
        /// Copies the settings from the loaded profile profile to new profile lighting profile
        /// This is used to copy all the settings over from the loaded profile to apply changes and apply to the scene
        /// </summary>
        /// <param name="sourceProfile"></param>
        /// <param name="newProfile"></param>
        public static void CopySettingsFrom(SceneProfile sourceProfile, GaiaLightingProfile newProfile)
        {
            if (sourceProfile == null || newProfile == null)
            {
                return;
            }

            newProfile.m_multiSceneLightingSupport = sourceProfile.m_lightingMultiSceneLightingSupport;
            newProfile.m_updateInRealtime = sourceProfile.m_lightingUpdateInRealtime;
            newProfile.m_selectedLightingProfileValuesIndex = sourceProfile.m_selectedLightingProfileValuesIndex;
            newProfile.m_renamingProfile = sourceProfile.m_renamingProfile;
            newProfile.m_editSettings = sourceProfile.m_lightingEditSettings;
            newProfile.m_lightingBakeMode = sourceProfile.m_lightingBakeMode;
            newProfile.m_lightmappingMode = sourceProfile.m_lightmappingMode;
            newProfile.m_masterSkyboxMaterial = sourceProfile.m_masterSkyboxMaterial;
            newProfile.m_lightingProfiles = sourceProfile.m_lightingProfiles;

            newProfile.m_parentObjects = sourceProfile.m_parentObjects;
            newProfile.m_hideProcessVolume = sourceProfile.m_hideProcessVolume;
            newProfile.m_enablePostProcessing = sourceProfile.m_enablePostProcessing;
            newProfile.m_enableAmbientAudio = sourceProfile.m_enableAmbientAudio;
            newProfile.m_enableFog = sourceProfile.m_enableFog;
            newProfile.m_antiAliasingMode = sourceProfile.m_antiAliasingMode;
            newProfile.m_antiAliasingTAAStrength = sourceProfile.m_antiAliasingTAAStrength;
            newProfile.m_cameraDithering = sourceProfile.m_cameraDithering;
            newProfile.m_cameraAperture = sourceProfile.m_cameraAperture;
            newProfile.m_usePhysicalCamera = sourceProfile.m_usePhysicalCamera;
            newProfile.m_cameraSensorSize = sourceProfile.m_cameraSensorSize;
            newProfile.m_cameraFocalLength = sourceProfile.m_cameraFocalLength;
            newProfile.m_globalReflectionProbe = sourceProfile.m_globalReflectionProbe;
            newProfile.m_enableAutoDOF = sourceProfile.m_enableAutoDOF;
            newProfile.m_dofLayerDetection = sourceProfile.m_dofLayerDetection;

            EditorUtility.SetDirty(newProfile);
        }
        /// <summary>
        /// Copies the settings from the loaded profile profile to new profile water profile
        /// This is used to copy all the settings over from the loaded profile to apply changes and apply to the scene
        /// </summary>
        /// <param name="sourceProfile"></param>
        /// <param name="newProfile"></param>
        public static void CopySettingsFrom(SceneProfile sourceProfile, GaiaWaterProfile newProfile)
        {
            if (sourceProfile == null || newProfile == null)
            {
                return;
            }

            newProfile.m_multiSceneLightingSupport = sourceProfile.m_waterMultiSceneLightingSupport;
            newProfile.m_renamingProfile = sourceProfile.m_renamingProfile;
            newProfile.m_updateInRealtime = sourceProfile.m_waterUpdateInRealtime;
            newProfile.m_allowMSAA = sourceProfile.m_allowMSAA;
            newProfile.m_useHDR = sourceProfile.m_useHDR;
            newProfile.m_enableDisabeHeightFeature = sourceProfile.m_enableDisabeHeightFeature;
            newProfile.m_disableHeight = sourceProfile.m_disableHeight;
            newProfile.m_selectedProfile = sourceProfile.m_selectedProfile;
            newProfile.m_interval = sourceProfile.m_interval;
            newProfile.m_useCustomRenderDistance = sourceProfile.m_useCustomRenderDistance;
            newProfile.m_enableLayerDistances = sourceProfile.m_enableLayerDistances;
            newProfile.m_customRenderDistance = sourceProfile.m_customRenderDistance;
            newProfile.m_customRenderDistances = sourceProfile.m_customRenderDistances;
            newProfile.m_reflectionSettingsData = sourceProfile.m_reflectionSettingsData;
            newProfile.m_editSettings = sourceProfile.m_waterEditSettings;
            newProfile.m_selectedWaterProfileValuesIndex = sourceProfile.m_selectedWaterProfileValuesIndex;
            newProfile.m_useCastics = sourceProfile.m_useCastics;
            newProfile.m_mainCausticLight = sourceProfile.m_mainCausticLight;
            newProfile.m_causticFramePerSecond = sourceProfile.m_causticFramePerSecond;
            newProfile.m_causticSize = sourceProfile.m_causticSize;

            newProfile.m_waterPrefab = sourceProfile.m_waterPrefab;
            newProfile.m_supportUnderwaterParticles = sourceProfile.m_supportUnderwaterParticles;
            newProfile.m_underwaterHorizonPrefab = sourceProfile.m_underwaterHorizonPrefab;
            newProfile.m_hdPlanarReflections = sourceProfile.m_hdPlanarReflections;
            newProfile.m_transitionFXPrefab = sourceProfile.m_transitionFXPrefab;
            newProfile.m_waterProfiles = sourceProfile.m_waterProfiles;
            newProfile.m_activeWaterMaterial = sourceProfile.m_activeWaterMaterial;
            newProfile.m_enableWaterMeshQuality = sourceProfile.m_enableWaterMeshQuality;
            newProfile.m_waterMeshQuality = sourceProfile.m_waterMeshQuality;
            newProfile.m_meshType = sourceProfile.m_meshType;
            newProfile.m_zSize = sourceProfile.m_zSize;
            newProfile.m_xSize = sourceProfile.m_xSize;
            newProfile.m_customMeshQuality = sourceProfile.m_customMeshQuality;
            newProfile.m_enableReflections = sourceProfile.m_enableReflections;
            newProfile.m_disablePixelLights = sourceProfile.m_disablePixelLights;
            newProfile.m_reflectionResolution = sourceProfile.m_reflectionResolution;
            newProfile.m_textureResolution = sourceProfile.m_textureResolution;
            newProfile.m_clipPlaneOffset = sourceProfile.m_clipPlaneOffset;
            newProfile.m_reflectedLayers = sourceProfile.m_reflectedLayers;
            newProfile.m_hdrpReflectionIntensity = sourceProfile.m_hdrpReflectionIntensity;
            newProfile.m_enableOceanFoam = sourceProfile.m_enableOceanFoam;
            newProfile.m_enableBeachFoam = sourceProfile.m_enableBeachFoam;
            newProfile.m_enableGPUInstancing = sourceProfile.m_enableGPUInstancing;
            newProfile.m_autoWindControlOnWater = sourceProfile.m_autoWindControlOnWater;
            newProfile.m_supportUnderwaterEffects = sourceProfile.m_supportUnderwaterEffects;
            newProfile.m_supportUnderwaterPostProcessing = sourceProfile.m_supportUnderwaterPostProcessing;
            newProfile.m_supportUnderwaterFog = sourceProfile.m_supportUnderwaterFog;
            newProfile.m_supportUnderwaterParticles = sourceProfile.m_supportUnderwaterParticles;

            EditorUtility.SetDirty(newProfile);
        }
        /// <summary>
        /// Copies the settings from source lighting profile to the scene profile
        /// This is used to setup some defaults in the scene
        /// </summary>
        /// <param name="sourceProfile"></param>
        /// <param name="newProfile"></param>
        public static void CopySettingsTo(GaiaLightingProfile sourceProfile, SceneProfile newProfile)
        {
            if (sourceProfile == null || newProfile == null)
            {
                return;
            }

            newProfile.m_cullingProfile = GaiaGlobal.Instance.SceneProfile.m_cullingProfile;
            newProfile.m_sunLight = GaiaGlobal.Instance.SceneProfile.m_sunLight;

            newProfile.m_lightingMultiSceneLightingSupport = sourceProfile.m_multiSceneLightingSupport;
            newProfile.m_lightingUpdateInRealtime = sourceProfile.m_updateInRealtime;
            newProfile.m_selectedLightingProfileValuesIndex = sourceProfile.m_selectedLightingProfileValuesIndex;
            newProfile.m_renamingProfile = sourceProfile.m_renamingProfile;
            newProfile.m_lightingEditSettings = sourceProfile.m_editSettings;
            newProfile.m_lightingBakeMode = sourceProfile.m_lightingBakeMode;
            newProfile.m_lightmappingMode = sourceProfile.m_lightmappingMode;
            newProfile.m_masterSkyboxMaterial = sourceProfile.m_masterSkyboxMaterial;
            newProfile.m_lightingProfiles = sourceProfile.m_lightingProfiles;
            newProfile.m_parentObjects = sourceProfile.m_parentObjects;
            newProfile.m_hideProcessVolume = sourceProfile.m_hideProcessVolume;
            newProfile.m_enablePostProcessing = sourceProfile.m_enablePostProcessing;
            newProfile.m_enableAmbientAudio = sourceProfile.m_enableAmbientAudio;
            newProfile.m_enableFog = sourceProfile.m_enableFog;
            newProfile.m_antiAliasingMode = sourceProfile.m_antiAliasingMode;
            newProfile.m_antiAliasingTAAStrength = sourceProfile.m_antiAliasingTAAStrength;
            newProfile.m_cameraDithering = sourceProfile.m_cameraDithering;
            newProfile.m_cameraAperture = sourceProfile.m_cameraAperture;
            newProfile.m_usePhysicalCamera = sourceProfile.m_usePhysicalCamera;
            newProfile.m_cameraSensorSize = sourceProfile.m_cameraSensorSize;
            newProfile.m_cameraFocalLength = sourceProfile.m_cameraFocalLength;
            newProfile.m_globalReflectionProbe = sourceProfile.m_globalReflectionProbe;
            newProfile.m_enableAutoDOF = sourceProfile.m_enableAutoDOF;
            newProfile.m_dofLayerDetection = sourceProfile.m_dofLayerDetection;

            EditorUtility.SetDirty(newProfile);
        }
        /// <summary>
        /// Copies the settings from source water profile to the scene profile
        /// This is used to setup some defaults in the scene
        /// </summary>
        /// <param name="sourceProfile"></param>
        /// <param name="newProfile"></param>
        public static void CopySettingsTo(GaiaWaterProfile sourceProfile, SceneProfile newProfile)
        {
            if (sourceProfile == null || newProfile == null)
            {
                return;
            }

            newProfile.m_waterMultiSceneLightingSupport = sourceProfile.m_multiSceneLightingSupport;
            newProfile.m_renamingProfile = sourceProfile.m_renamingProfile;
            newProfile.m_waterUpdateInRealtime = sourceProfile.m_updateInRealtime;
            newProfile.m_allowMSAA = sourceProfile.m_allowMSAA;
            newProfile.m_useHDR = sourceProfile.m_useHDR;
            newProfile.m_enableDisabeHeightFeature = sourceProfile.m_enableDisabeHeightFeature;
            newProfile.m_disableHeight = sourceProfile.m_disableHeight;
            newProfile.m_selectedProfile = sourceProfile.m_selectedProfile;
            newProfile.m_interval = sourceProfile.m_interval;
            newProfile.m_useCustomRenderDistance = sourceProfile.m_useCustomRenderDistance;
            newProfile.m_enableLayerDistances = sourceProfile.m_enableLayerDistances;
            newProfile.m_customRenderDistance = sourceProfile.m_customRenderDistance;
            newProfile.m_customRenderDistances = sourceProfile.m_customRenderDistances;
            newProfile.m_reflectionSettingsData = sourceProfile.m_reflectionSettingsData;
            newProfile.m_waterEditSettings = sourceProfile.m_editSettings;
            newProfile.m_selectedWaterProfileValuesIndex = sourceProfile.m_selectedWaterProfileValuesIndex;
            newProfile.m_useCastics = sourceProfile.m_useCastics;
            newProfile.m_mainCausticLight = sourceProfile.m_mainCausticLight;
            newProfile.m_causticFramePerSecond = sourceProfile.m_causticFramePerSecond;
            newProfile.m_causticSize = sourceProfile.m_causticSize;
            newProfile.m_waterPrefab = sourceProfile.m_waterPrefab;
            newProfile.m_supportUnderwaterParticles = sourceProfile.m_supportUnderwaterParticles;
            newProfile.m_underwaterHorizonPrefab = sourceProfile.m_underwaterHorizonPrefab;
            newProfile.m_hdPlanarReflections = sourceProfile.m_hdPlanarReflections;
            newProfile.m_transitionFXPrefab = sourceProfile.m_transitionFXPrefab;
            newProfile.m_waterProfiles = sourceProfile.m_waterProfiles;
            newProfile.m_activeWaterMaterial = sourceProfile.m_activeWaterMaterial;
            newProfile.m_enableWaterMeshQuality = sourceProfile.m_enableWaterMeshQuality;
            newProfile.m_waterMeshQuality = sourceProfile.m_waterMeshQuality;
            newProfile.m_meshType = sourceProfile.m_meshType;
            newProfile.m_zSize = sourceProfile.m_zSize;
            newProfile.m_xSize = sourceProfile.m_xSize;
            newProfile.m_customMeshQuality = sourceProfile.m_customMeshQuality;
            newProfile.m_enableReflections = sourceProfile.m_enableReflections;
            newProfile.m_disablePixelLights = sourceProfile.m_disablePixelLights;
            newProfile.m_reflectionResolution = sourceProfile.m_reflectionResolution;
            newProfile.m_textureResolution = sourceProfile.m_textureResolution;
            newProfile.m_clipPlaneOffset = sourceProfile.m_clipPlaneOffset;
            newProfile.m_reflectedLayers = sourceProfile.m_reflectedLayers;
            newProfile.m_hdrpReflectionIntensity = sourceProfile.m_hdrpReflectionIntensity;
            newProfile.m_enableOceanFoam = sourceProfile.m_enableOceanFoam;
            newProfile.m_enableBeachFoam = sourceProfile.m_enableBeachFoam;
            newProfile.m_enableGPUInstancing = sourceProfile.m_enableGPUInstancing;
            newProfile.m_autoWindControlOnWater = sourceProfile.m_autoWindControlOnWater;
            newProfile.m_supportUnderwaterEffects = sourceProfile.m_supportUnderwaterEffects;
            newProfile.m_supportUnderwaterPostProcessing = sourceProfile.m_supportUnderwaterPostProcessing;
            newProfile.m_supportUnderwaterFog = sourceProfile.m_supportUnderwaterFog;
            newProfile.m_underwaterParticles = sourceProfile.m_underwaterParticles;

            EditorUtility.SetDirty(newProfile);
        }
        /// <summary>
        /// Copies the settings from the loaded profile (source) to the profile in gaia global to load up all the settings
        /// </summary>
        /// <param name="sourceProfile"></param>
        /// <param name="newProfile"></param>
        private static void CopyFromProfileToNewProfile(SceneProfile sourceProfile, SceneProfile newProfile)
        {
            if (sourceProfile == null || newProfile == null)
            {
                return;
            }

            //Global
            newProfile.m_gaiaTimeOfDay.m_todDayTimeScale = sourceProfile.m_gaiaTimeOfDay.m_todDayTimeScale;
            newProfile.m_gaiaTimeOfDay.m_todEnabled = sourceProfile.m_gaiaTimeOfDay.m_todEnabled;
            newProfile.m_gaiaTimeOfDay.m_todHour = sourceProfile.m_gaiaTimeOfDay.m_todHour;
            newProfile.m_gaiaTimeOfDay.m_todMinutes = sourceProfile.m_gaiaTimeOfDay.m_todMinutes;
            newProfile.m_gaiaTimeOfDay.m_todStartingType = sourceProfile.m_gaiaTimeOfDay.m_todStartingType;

            newProfile.m_cullingProfile = sourceProfile.m_cullingProfile;
            newProfile.m_sunLight = sourceProfile.m_sunLight;

            newProfile.m_gaiaWeather.m_season = sourceProfile.m_gaiaWeather.m_season;
            newProfile.m_gaiaWeather.m_windDirection = sourceProfile.m_gaiaWeather.m_windDirection;

            //Lighting
            newProfile.DefaultLightingSet = sourceProfile.DefaultLightingSet;
            newProfile.m_lightingMultiSceneLightingSupport = sourceProfile.m_lightingMultiSceneLightingSupport;
            newProfile.m_lightingUpdateInRealtime = sourceProfile.m_lightingUpdateInRealtime;
            newProfile.m_selectedLightingProfileValuesIndex = sourceProfile.m_selectedLightingProfileValuesIndex;
            newProfile.m_renamingProfile = sourceProfile.m_renamingProfile;
            newProfile.m_lightingEditSettings = sourceProfile.m_lightingEditSettings;
            newProfile.m_lightingBakeMode = sourceProfile.m_lightingBakeMode;
            newProfile.m_lightmappingMode = sourceProfile.m_lightmappingMode;
            newProfile.m_masterSkyboxMaterial = sourceProfile.m_masterSkyboxMaterial;
            newProfile.m_lightingProfiles = sourceProfile.m_lightingProfiles;
            newProfile.m_parentObjects = sourceProfile.m_parentObjects;
            newProfile.m_hideProcessVolume = sourceProfile.m_hideProcessVolume;
            newProfile.m_enablePostProcessing = sourceProfile.m_enablePostProcessing;
            newProfile.m_enableAmbientAudio = sourceProfile.m_enableAmbientAudio;
            newProfile.m_enableFog = sourceProfile.m_enableFog;
            newProfile.m_antiAliasingMode = sourceProfile.m_antiAliasingMode;
            newProfile.m_antiAliasingTAAStrength = sourceProfile.m_antiAliasingTAAStrength;
            newProfile.m_cameraDithering = sourceProfile.m_cameraDithering;
            newProfile.m_cameraAperture = sourceProfile.m_cameraAperture;
            newProfile.m_usePhysicalCamera = sourceProfile.m_usePhysicalCamera;
            newProfile.m_cameraSensorSize = sourceProfile.m_cameraSensorSize;
            newProfile.m_cameraFocalLength = sourceProfile.m_cameraFocalLength;
            newProfile.m_globalReflectionProbe = sourceProfile.m_globalReflectionProbe;
            newProfile.m_enableAutoDOF = sourceProfile.m_enableAutoDOF;
            newProfile.m_dofLayerDetection = sourceProfile.m_dofLayerDetection;
            //Water
            newProfile.DefaultWaterSet = sourceProfile.DefaultWaterSet;
            newProfile.m_waterMultiSceneLightingSupport = sourceProfile.m_waterMultiSceneLightingSupport;
            newProfile.m_renamingProfile = sourceProfile.m_renamingProfile;
            newProfile.m_waterUpdateInRealtime = sourceProfile.m_waterUpdateInRealtime;
            newProfile.m_allowMSAA = sourceProfile.m_allowMSAA;
            newProfile.m_useHDR = sourceProfile.m_useHDR;
            newProfile.m_enableDisabeHeightFeature = sourceProfile.m_enableDisabeHeightFeature;
            newProfile.m_disableHeight = sourceProfile.m_disableHeight;
            newProfile.m_selectedProfile = sourceProfile.m_selectedProfile;
            newProfile.m_interval = sourceProfile.m_interval;
            newProfile.m_useCustomRenderDistance = sourceProfile.m_useCustomRenderDistance;
            newProfile.m_enableLayerDistances = sourceProfile.m_enableLayerDistances;
            newProfile.m_customRenderDistance = sourceProfile.m_customRenderDistance;
            newProfile.m_customRenderDistances = sourceProfile.m_customRenderDistances;
            newProfile.m_reflectionSettingsData = sourceProfile.m_reflectionSettingsData;
            newProfile.m_waterEditSettings = sourceProfile.m_waterEditSettings;
            newProfile.m_selectedWaterProfileValuesIndex = sourceProfile.m_selectedWaterProfileValuesIndex;
            newProfile.m_useCastics = sourceProfile.m_useCastics;
            newProfile.m_mainCausticLight = sourceProfile.m_mainCausticLight;
            newProfile.m_causticFramePerSecond = sourceProfile.m_causticFramePerSecond;
            newProfile.m_causticSize = sourceProfile.m_causticSize;
            newProfile.m_waterPrefab = sourceProfile.m_waterPrefab;
            newProfile.m_supportUnderwaterParticles = sourceProfile.m_supportUnderwaterParticles;
            newProfile.m_underwaterHorizonPrefab = sourceProfile.m_underwaterHorizonPrefab;
            newProfile.m_hdPlanarReflections = sourceProfile.m_hdPlanarReflections;
            newProfile.m_transitionFXPrefab = sourceProfile.m_transitionFXPrefab;
            newProfile.m_waterProfiles = sourceProfile.m_waterProfiles;
            newProfile.m_activeWaterMaterial = sourceProfile.m_activeWaterMaterial;
            newProfile.m_enableWaterMeshQuality = sourceProfile.m_enableWaterMeshQuality;
            newProfile.m_waterMeshQuality = sourceProfile.m_waterMeshQuality;
            newProfile.m_meshType = sourceProfile.m_meshType;
            newProfile.m_zSize = sourceProfile.m_zSize;
            newProfile.m_xSize = sourceProfile.m_xSize;
            newProfile.m_customMeshQuality = sourceProfile.m_customMeshQuality;
            newProfile.m_enableReflections = sourceProfile.m_enableReflections;
            newProfile.m_disablePixelLights = sourceProfile.m_disablePixelLights;
            newProfile.m_reflectionResolution = sourceProfile.m_reflectionResolution;
            newProfile.m_textureResolution = sourceProfile.m_textureResolution;
            newProfile.m_clipPlaneOffset = sourceProfile.m_clipPlaneOffset;
            newProfile.m_reflectedLayers = sourceProfile.m_reflectedLayers;
            newProfile.m_hdrpReflectionIntensity = sourceProfile.m_hdrpReflectionIntensity;
            newProfile.m_enableOceanFoam = sourceProfile.m_enableOceanFoam;
            newProfile.m_enableBeachFoam = sourceProfile.m_enableBeachFoam;
            newProfile.m_enableGPUInstancing = sourceProfile.m_enableGPUInstancing;
            newProfile.m_autoWindControlOnWater = sourceProfile.m_autoWindControlOnWater;
            newProfile.m_supportUnderwaterEffects = sourceProfile.m_supportUnderwaterEffects;
            newProfile.m_supportUnderwaterPostProcessing = sourceProfile.m_supportUnderwaterPostProcessing;
            newProfile.m_supportUnderwaterFog = sourceProfile.m_supportUnderwaterFog;
            newProfile.m_underwaterParticles = sourceProfile.m_underwaterParticles;
        }
        /// <summary>
        /// Copies the settings from the loaded profile (source) to the profile in gaia global to load up all the settings
        /// </summary>
        /// <param name="sourceProfile"></param>
        /// <param name="newProfile"></param>
        private static void CopyFromProfileToNewProfileLighting(SceneProfile sourceProfile, SceneProfile newProfile)
        {
            if (sourceProfile == null || newProfile == null)
            {
                return;
            }

            newProfile.m_cullingProfile = GaiaGlobal.Instance.SceneProfile.m_cullingProfile;
            newProfile.m_sunLight = GaiaGlobal.Instance.SceneProfile.m_sunLight;

            //Global
            newProfile.m_gaiaTimeOfDay.m_todDayTimeScale = sourceProfile.m_gaiaTimeOfDay.m_todDayTimeScale;
            newProfile.m_gaiaTimeOfDay.m_todEnabled = sourceProfile.m_gaiaTimeOfDay.m_todEnabled;
            newProfile.m_gaiaTimeOfDay.m_todHour = sourceProfile.m_gaiaTimeOfDay.m_todHour;
            newProfile.m_gaiaTimeOfDay.m_todMinutes = sourceProfile.m_gaiaTimeOfDay.m_todMinutes;
            newProfile.m_gaiaTimeOfDay.m_todStartingType = sourceProfile.m_gaiaTimeOfDay.m_todStartingType;

            newProfile.m_gaiaWeather.m_season = sourceProfile.m_gaiaWeather.m_season;
            newProfile.m_gaiaWeather.m_windDirection = sourceProfile.m_gaiaWeather.m_windDirection;

            //Lighting
            newProfile.DefaultLightingSet = sourceProfile.DefaultLightingSet;
            newProfile.m_lightingMultiSceneLightingSupport = sourceProfile.m_lightingMultiSceneLightingSupport;
            newProfile.m_lightingUpdateInRealtime = sourceProfile.m_lightingUpdateInRealtime;
            newProfile.m_selectedLightingProfileValuesIndex = sourceProfile.m_selectedLightingProfileValuesIndex;
            newProfile.m_renamingProfile = sourceProfile.m_renamingProfile;
            newProfile.m_lightingEditSettings = sourceProfile.m_lightingEditSettings;
            newProfile.m_lightingBakeMode = sourceProfile.m_lightingBakeMode;
            newProfile.m_lightmappingMode = sourceProfile.m_lightmappingMode;
            newProfile.m_masterSkyboxMaterial = sourceProfile.m_masterSkyboxMaterial;
            newProfile.m_lightingProfiles = sourceProfile.m_lightingProfiles;
            newProfile.m_parentObjects = sourceProfile.m_parentObjects;
            newProfile.m_hideProcessVolume = sourceProfile.m_hideProcessVolume;
            newProfile.m_enablePostProcessing = sourceProfile.m_enablePostProcessing;
            newProfile.m_enableAmbientAudio = sourceProfile.m_enableAmbientAudio;
            newProfile.m_enableFog = sourceProfile.m_enableFog;
            newProfile.m_antiAliasingMode = sourceProfile.m_antiAliasingMode;
            newProfile.m_antiAliasingTAAStrength = sourceProfile.m_antiAliasingTAAStrength;
            newProfile.m_cameraDithering = sourceProfile.m_cameraDithering;
            newProfile.m_cameraAperture = sourceProfile.m_cameraAperture;
            newProfile.m_usePhysicalCamera = sourceProfile.m_usePhysicalCamera;
            newProfile.m_cameraSensorSize = sourceProfile.m_cameraSensorSize;
            newProfile.m_cameraFocalLength = sourceProfile.m_cameraFocalLength;
            newProfile.m_globalReflectionProbe = sourceProfile.m_globalReflectionProbe;
            newProfile.m_enableAutoDOF = sourceProfile.m_enableAutoDOF;
            newProfile.m_dofLayerDetection = sourceProfile.m_dofLayerDetection;
        }
        /// <summary>
        /// Copies the settings from the loaded profile (source) to the profile in gaia global to load up all the settings
        /// </summary>
        /// <param name="sourceProfile"></param>
        /// <param name="newProfile"></param>
        private static void CopyFromProfileToNewProfileWater(SceneProfile sourceProfile, SceneProfile newProfile)
        {
            if (sourceProfile == null || newProfile == null)
            {
                return;
            }

            newProfile.m_cullingProfile = GaiaGlobal.Instance.SceneProfile.m_cullingProfile;
            newProfile.m_sunLight = GaiaGlobal.Instance.SceneProfile.m_sunLight;

            //Water
            newProfile.DefaultWaterSet = sourceProfile.DefaultWaterSet;
            newProfile.m_waterMultiSceneLightingSupport = sourceProfile.m_waterMultiSceneLightingSupport;
            newProfile.m_renamingProfile = sourceProfile.m_renamingProfile;
            newProfile.m_waterUpdateInRealtime = sourceProfile.m_waterUpdateInRealtime;
            newProfile.m_allowMSAA = sourceProfile.m_allowMSAA;
            newProfile.m_useHDR = sourceProfile.m_useHDR;
            newProfile.m_enableDisabeHeightFeature = sourceProfile.m_enableDisabeHeightFeature;
            newProfile.m_disableHeight = sourceProfile.m_disableHeight;
            newProfile.m_selectedProfile = sourceProfile.m_selectedProfile;
            newProfile.m_interval = sourceProfile.m_interval;
            newProfile.m_useCustomRenderDistance = sourceProfile.m_useCustomRenderDistance;
            newProfile.m_enableLayerDistances = sourceProfile.m_enableLayerDistances;
            newProfile.m_customRenderDistance = sourceProfile.m_customRenderDistance;
            newProfile.m_customRenderDistances = sourceProfile.m_customRenderDistances;
            newProfile.m_reflectionSettingsData = sourceProfile.m_reflectionSettingsData;
            newProfile.m_waterEditSettings = sourceProfile.m_waterEditSettings;
            newProfile.m_selectedWaterProfileValuesIndex = sourceProfile.m_selectedWaterProfileValuesIndex;
            newProfile.m_useCastics = sourceProfile.m_useCastics;
            newProfile.m_mainCausticLight = sourceProfile.m_mainCausticLight;
            newProfile.m_causticFramePerSecond = sourceProfile.m_causticFramePerSecond;
            newProfile.m_causticSize = sourceProfile.m_causticSize;
            newProfile.m_waterPrefab = sourceProfile.m_waterPrefab;
            newProfile.m_supportUnderwaterParticles = sourceProfile.m_supportUnderwaterParticles;
            newProfile.m_underwaterHorizonPrefab = sourceProfile.m_underwaterHorizonPrefab;
            newProfile.m_hdPlanarReflections = sourceProfile.m_hdPlanarReflections;
            newProfile.m_transitionFXPrefab = sourceProfile.m_transitionFXPrefab;
            newProfile.m_waterProfiles = sourceProfile.m_waterProfiles;
            newProfile.m_activeWaterMaterial = sourceProfile.m_activeWaterMaterial;
            newProfile.m_enableWaterMeshQuality = sourceProfile.m_enableWaterMeshQuality;
            newProfile.m_waterMeshQuality = sourceProfile.m_waterMeshQuality;
            newProfile.m_meshType = sourceProfile.m_meshType;
            newProfile.m_zSize = sourceProfile.m_zSize;
            newProfile.m_xSize = sourceProfile.m_xSize;
            newProfile.m_customMeshQuality = sourceProfile.m_customMeshQuality;
            newProfile.m_enableReflections = sourceProfile.m_enableReflections;
            newProfile.m_disablePixelLights = sourceProfile.m_disablePixelLights;
            newProfile.m_reflectionResolution = sourceProfile.m_reflectionResolution;
            newProfile.m_textureResolution = sourceProfile.m_textureResolution;
            newProfile.m_clipPlaneOffset = sourceProfile.m_clipPlaneOffset;
            newProfile.m_reflectedLayers = sourceProfile.m_reflectedLayers;
            newProfile.m_hdrpReflectionIntensity = sourceProfile.m_hdrpReflectionIntensity;
            newProfile.m_enableOceanFoam = sourceProfile.m_enableOceanFoam;
            newProfile.m_enableBeachFoam = sourceProfile.m_enableBeachFoam;
            newProfile.m_enableGPUInstancing = sourceProfile.m_enableGPUInstancing;
            newProfile.m_autoWindControlOnWater = sourceProfile.m_autoWindControlOnWater;
            newProfile.m_supportUnderwaterEffects = sourceProfile.m_supportUnderwaterEffects;
            newProfile.m_supportUnderwaterPostProcessing = sourceProfile.m_supportUnderwaterPostProcessing;
            newProfile.m_supportUnderwaterFog = sourceProfile.m_supportUnderwaterFog;
            newProfile.m_underwaterParticles = sourceProfile.m_underwaterParticles;
        }
        /// <summary>
        /// Sets the scene profile in the current scene
        /// </summary>
        /// <param name="profile"></param>
        private static void SetSceneProfile(SceneProfile profile)
        {
            if (profile == null)
            {
                return;
            }

            if (GaiaGlobal.Instance == null)
            {
                return;
            }

            GaiaGlobal.Instance.SceneProfile = null;
            GaiaGlobal.Instance.SceneProfile = UnityEngine.Object.Instantiate(profile);
        }

        private static void LoadGlobalSettings(SceneProfile profile)
        {
            if (profile == null)
            {
                return;
            }

            if (GaiaGlobal.Instance != null)
            {
                GaiaGlobal.Instance.GaiaTimeOfDayValue.m_todHour = profile.m_gaiaTimeOfDay.m_todHour;
                GaiaGlobal.Instance.GaiaTimeOfDayValue.m_todMinutes = profile.m_gaiaTimeOfDay.m_todMinutes;
                GaiaGlobal.Instance.GaiaTimeOfDayValue.m_todDayTimeScale = profile.m_gaiaTimeOfDay.m_todDayTimeScale;
                GaiaGlobal.Instance.GaiaTimeOfDayValue.m_todStartingType = profile.m_gaiaTimeOfDay.m_todStartingType;
                GaiaGlobal.Instance.GaiaTimeOfDayValue.m_todEnabled = profile.m_gaiaTimeOfDay.m_todEnabled;

                GaiaGlobal.Instance.GaiaWeather.m_season = profile.m_gaiaWeather.m_season;
                GaiaGlobal.Instance.GaiaWeather.m_windDirection = profile.m_gaiaWeather.m_windDirection;
            }
        }

        #endregion

        #region Scene Fetch Utils

        public static void FetchSceneSettigns(SceneProfile profile, GaiaLightingProfileValues profileValues)
        {
            try
            {
                if (profile == null || profileValues == null || profile.m_lightSystemMode == GaiaConstants.GlobalSystemMode.ThirdParty)
                {
                    return;
                }

                GaiaConstants.EnvironmentRenderer renderPipeline = GaiaUtils.GetActivePipeline();
                if (renderPipeline != GaiaConstants.EnvironmentRenderer.HighDefinition)
                {
                    if (profileValues != null)
                    {
                        if (profileValues.m_profileType != GaiaConstants.GaiaLightingProfileType.ProceduralWorldsSky)
                        {
                            GetSunSettings(profileValues, renderPipeline);
                            GetFogSettings(profileValues);
                            GetAmbientSettings(profileValues);
                            GetSkyboxSettings(profileValues);
                            GetLightmapSettings(profile);
                        }

                        GetLODBias(profile);
                    }
                }
                else
                {
                    //Get HDRP
                    GetHDRPSunSettings(profileValues);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        /// <summary>
        /// Gets and sets the sun settings in the selected profile
        /// </summary>
        /// <param name="profile"></param>
        private static void GetSunSettings(GaiaLightingProfileValues profile, GaiaConstants.EnvironmentRenderer renderPipeline)
        {
            try
            {
                if (profile == null)
                {
                    return;
                }

                Light light = GaiaUtils.GetMainDirectionalLight();
                if (light != null)
                {
                    switch (renderPipeline)
                    {
                        case GaiaConstants.EnvironmentRenderer.BuiltIn:
                            profile.m_sunColor = light.color;
                            profile.m_sunIntensity = light.intensity;
                            break;
                        case GaiaConstants.EnvironmentRenderer.Universal:
                            profile.m_lWSunColor = light.color;
                            profile.m_lWSunIntensity = light.intensity;
                            break;
                    }

                    profile.m_shadowCastingMode = light.shadows;
                    profile.m_shadowStrength = light.shadowStrength;
                    profile.m_sunShadowResolution = light.shadowResolution;

                    if (light.transform.localEulerAngles.y < 0)
                    {
                        light.transform.eulerAngles = new Vector3(light.transform.eulerAngles.x, light.transform.eulerAngles.y % 360f, light.transform.eulerAngles.z);
                    }
                    else if (light.transform.localEulerAngles.y > 360f)
                    {
                        light.transform.eulerAngles = new Vector3(light.transform.eulerAngles.x, light.transform.eulerAngles.y % -360f, light.transform.eulerAngles.z);
                    }

                    if (light.transform.localEulerAngles.x < 0)
                    {
                        light.transform.eulerAngles = new Vector3(light.transform.eulerAngles.x % 360f, light.transform.eulerAngles.y, light.transform.eulerAngles.z);
                    }
                    else if (light.transform.localEulerAngles.x > 360f)
                    {
                        light.transform.eulerAngles = new Vector3(light.transform.eulerAngles.x % -360f, light.transform.eulerAngles.y, light.transform.eulerAngles.z);
                    }

                    profile.m_sunPitch = light.transform.eulerAngles.x;
                    profile.m_sunRotation = light.transform.eulerAngles.y;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        /// <summary>
        /// Gets and sets the HDRP sun settings in the selected profile
        /// </summary>
        /// <param name="profile"></param>
        private static void GetHDRPSunSettings(GaiaLightingProfileValues profile)
        {
            try
            {
                if (profile == null)
                {
                    return;
                }
#if HDPipeline
                Light light = GaiaUtils.GetMainDirectionalLight();
                if (light != null)
                {
                    HDAdditionalLightData lightData = GaiaHDRPRuntimeUtils.GetHDLightData(light);
                    if (lightData != null)
                    {
                        profile.m_hDSunColor = lightData.color;
                        profile.m_hDSunIntensity = lightData.intensity;
                        profile.m_hDSunVolumetricMultiplier = lightData.volumetricDimmer;
                    }

                    if (light.transform.localEulerAngles.y < 0)
                    {
                        light.transform.eulerAngles = new Vector3(light.transform.eulerAngles.x, light.transform.eulerAngles.y % 360f, light.transform.eulerAngles.z);
                    }
                    else if (light.transform.localEulerAngles.y > 360f)
                    {
                        light.transform.eulerAngles = new Vector3(light.transform.eulerAngles.x, light.transform.eulerAngles.y % -360f, light.transform.eulerAngles.z);
                    }

                    if (light.transform.localEulerAngles.x < 0)
                    {
                        light.transform.eulerAngles = new Vector3(light.transform.eulerAngles.x % 360f, light.transform.eulerAngles.y, light.transform.eulerAngles.z);
                    }
                    else if (light.transform.localEulerAngles.x > 360f)
                    {
                        light.transform.eulerAngles = new Vector3(light.transform.eulerAngles.x % -360f, light.transform.eulerAngles.y, light.transform.eulerAngles.z);
                    }

                    profile.m_sunPitch = light.transform.eulerAngles.x;
                    profile.m_sunRotation = light.transform.eulerAngles.y;
                }
#endif
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        /// <summary>
        /// Gets and sets the fog settings in the selected profile
        /// </summary>
        /// <param name="profile"></param>
        private static void GetFogSettings(GaiaLightingProfileValues profile)
        {
            try
            {
                if (profile == null)
                {
                    return;
                }

                profile.m_fogMode = RenderSettings.fogMode;
                profile.m_fogColor = RenderSettings.fogColor;
                profile.m_fogDensity = RenderSettings.fogDensity;
                profile.m_fogStartDistance = RenderSettings.fogStartDistance;
                profile.m_fogEndDistance = RenderSettings.fogEndDistance;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        /// <summary>
        /// Gets and sets the ambient settings in the selected profile
        /// </summary>
        /// <param name="profile"></param>
        private static void GetAmbientSettings(GaiaLightingProfileValues profile)
        {
            try
            {
                if (profile == null)
                {
                    return;
                }

                profile.m_ambientMode = RenderSettings.ambientMode;
                profile.m_ambientIntensity = RenderSettings.ambientIntensity;
                profile.m_skyAmbient = RenderSettings.ambientSkyColor;
                profile.m_equatorAmbient = RenderSettings.ambientEquatorColor;
                profile.m_groundAmbient = RenderSettings.ambientGroundColor;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        /// <summary>
        /// Gets and sets the skybox settings in the selected profile
        /// </summary>
        /// <param name="profile"></param>
        private static void GetSkyboxSettings(GaiaLightingProfileValues profile)
        {
            try
            {
                if (profile == null)
                {
                    return;
                }

                Material skyboxMaterial = RenderSettings.skybox;
                if (skyboxMaterial != null)
                {
                    if (skyboxMaterial.shader == Shader.Find(GaiaShaderID.m_unitySkyboxShader))
                    {
                        if (skyboxMaterial.HasProperty(GaiaShaderID.m_unitySkyboxSunSize))
                        {
                            profile.m_sunSize = skyboxMaterial.GetFloat(GaiaShaderID.m_unitySkyboxSunSize);
                        }
                        if (skyboxMaterial.HasProperty(GaiaShaderID.m_unitySkyboxSunSizeConvergence))
                        {
                            profile.m_sunConvergence = skyboxMaterial.GetFloat(GaiaShaderID.m_unitySkyboxSunSizeConvergence);
                        }
                        if (skyboxMaterial.HasProperty(GaiaShaderID.m_unitySkyboxAtmosphereThickness))
                        {
                            profile.m_atmosphereThickness = skyboxMaterial.GetFloat(GaiaShaderID.m_unitySkyboxAtmosphereThickness);
                        }
                        if (skyboxMaterial.HasProperty(GaiaShaderID.m_unitySkyboxTint))
                        {
                            profile.m_skyboxTint = skyboxMaterial.GetColor(GaiaShaderID.m_unitySkyboxTint);
                        }
                        if (skyboxMaterial.HasProperty(GaiaShaderID.m_unitySkyboxGroundColor))
                        {
                            profile.m_groundColor = skyboxMaterial.GetColor(GaiaShaderID.m_unitySkyboxGroundColor);
                        }
                        if (skyboxMaterial.HasProperty(GaiaShaderID.m_unitySkyboxExposure))
                        {
                            profile.m_skyboxExposure = skyboxMaterial.GetFloat(GaiaShaderID.m_unitySkyboxExposure);
                        }
                    }
                    else if (skyboxMaterial.shader == Shader.Find(GaiaShaderID.m_unitySkyboxShaderHDRI))
                    {
                        if (skyboxMaterial.HasProperty(GaiaShaderID.m_unitySkyboxTintHDRI))
                        {
                            profile.m_skyboxTint = skyboxMaterial.GetColor(GaiaShaderID.m_unitySkyboxTintHDRI);
                        }
                        if (skyboxMaterial.HasProperty(GaiaShaderID.m_unitySkyboxExposure))
                        {
                            profile.m_skyboxExposure = skyboxMaterial.GetFloat(GaiaShaderID.m_unitySkyboxExposure);
                        }
                        if (skyboxMaterial.HasProperty(GaiaShaderID.m_unitySkyboxCubemap))
                        {
                            profile.m_skyboxHDRI = skyboxMaterial.GetTexture(GaiaShaderID.m_unitySkyboxCubemap) as Cubemap;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        /// <summary>
        /// Gets and sets the lightmap settings in the master profile
        /// </summary>
        /// <param name="profile"></param>
        private static void GetLightmapSettings(SceneProfile profile)
        {

            try
            {
                if (profile == null)
                {
                    return;
                }

#if !UNITY_2020_1_OR_NEWER
                profile.m_lightmappingMode = LightmapEditorSettings.lightmapper;
#else
                if (Lightmapping.lightingSettings != null)
                {
                    profile.m_lightmappingMode = Lightmapping.lightingSettings.lightmapper;
                }
#endif
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        /// <summary>
        /// Gets the LOD Bias settings
        /// </summary>
        /// <param name="profile"></param>
        private static void GetLODBias(SceneProfile profile)
        {
            profile.m_lodBias = QualitySettings.lodBias;
        }

        #endregion
    }
}