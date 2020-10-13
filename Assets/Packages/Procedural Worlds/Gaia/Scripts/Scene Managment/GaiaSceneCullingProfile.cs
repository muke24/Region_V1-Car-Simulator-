using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Gaia
{
    public class GaiaSceneCullingProfile : ScriptableObject
    {
        [Header("Global Settings")] 
        //public bool m_enableLayerCulling = true;
        public bool m_applyToEditorCamera = false;
        public bool m_realtimeUpdate = false;
        public float[] m_layerDistances = new float[32];
        public float[] m_shadowLayerDistances = new float[32];

        public void UpdateDefaults(GaiaSettings gaiaSettings)
        {
            if (!GaiaUtils.CheckIfSceneProfileExists())
            {
                return;
            }
            if (GaiaGlobal.Instance.m_mainCamera == null)
            {
                GaiaGlobal.Instance.m_mainCamera = GaiaUtils.GetCamera();
            }

            float farClipPlane = 2000f;
            if (GaiaGlobal.Instance.m_mainCamera != null)
            {
                farClipPlane = GaiaGlobal.Instance.m_mainCamera.farClipPlane;
            }

            if (GaiaGlobal.Instance.SceneProfile.m_sunLight == null)
            {
                GaiaGlobal.Instance.SceneProfile.m_sunLight = GaiaUtils.GetMainDirectionalLight();
            }

            Terrain terrain = TerrainHelper.GetActiveTerrain();

            //Objects
            m_layerDistances = new float[32];
            for (int i = 0; i < m_layerDistances.Length; i++)
            {
                string layerName = LayerMask.LayerToName(i);
                if (layerName == "Default")
                {
                    if (GaiaGlobal.Instance.m_mainCamera != null)
                    {
                        m_layerDistances[i] = farClipPlane;
                    }
                    else
                    {
                        m_layerDistances[i] = 2000f;
                    }
                }
                else if (layerName == "Water")
                {
                    if (GaiaGlobal.Instance.m_mainCamera != null)
                    {
                        m_layerDistances[i] = farClipPlane;
                    }
                    else
                    {
                        m_layerDistances[i] = 0f;
                    }
                }
                else if (layerName == "PW_VFX")
                {
                    m_layerDistances[i] = 0f;
                }
                else if (layerName == "PW_Object_Small")
                {
                    m_layerDistances[i] = GaiaUtils.CalculateCameraCullingLayerValue(terrain, gaiaSettings.m_currentEnvironment, 5f);
                }
                else if (layerName == "PW_Object_Medium")
                {
                    m_layerDistances[i] = GaiaUtils.CalculateCameraCullingLayerValue(terrain, gaiaSettings.m_currentEnvironment, 3f);
                }
                else if (layerName == "PW_Object_Large")
                {
                    if (GaiaGlobal.Instance.m_mainCamera != null)
                    {
                        m_layerDistances[i] = GaiaUtils.CalculateCameraCullingLayerValue(terrain, gaiaSettings.m_currentEnvironment);
                    }
                    else
                    {
                        m_layerDistances[i] = 1500f;
                    }
                }
                else
                {
                    m_layerDistances[i] = 0f;
                }
            }

            //Shadows
            m_shadowLayerDistances = new float[32];
            for (int i = 0; i < m_shadowLayerDistances.Length; i++)
            {
                string layerName = LayerMask.LayerToName(i);
                if (layerName == "Default")
                {
                    m_shadowLayerDistances[i] = 0;
                }
                else if (layerName == "Water")
                {
                    m_shadowLayerDistances[i] = 0f;
                }
                else if (layerName == "PW_VFX")
                {
                    m_shadowLayerDistances[i] = 0f;
                }
                else if (layerName == "PW_Object_Small")
                {
                    m_shadowLayerDistances[i] = GaiaUtils.CalculateShadowCullingLayerValue(terrain, gaiaSettings.m_currentEnvironment, 5f);
                }
                else if (layerName == "PW_Object_Medium")
                {
                    m_shadowLayerDistances[i] = GaiaUtils.CalculateShadowCullingLayerValue(terrain, gaiaSettings.m_currentEnvironment, 3f);
                }
                else if (layerName == "PW_Object_Large")
                {
                    if (GaiaGlobal.Instance.m_mainCamera != null)
                    {
                        m_shadowLayerDistances[i] = GaiaUtils.CalculateShadowCullingLayerValue(terrain, gaiaSettings.m_currentEnvironment);
                    }
                    else
                    {
                        m_shadowLayerDistances[i] = 0f;
                    }
                }
                else
                {
                    m_shadowLayerDistances[i] = 0f;
                }
            }
        }

        /// <summary>
        /// Create Gaia Culling System Profile asset
        /// </summary>
#if UNITY_EDITOR
        public static GaiaSceneCullingProfile CreateCullingProfile()
        {
            GaiaSceneCullingProfile asset = ScriptableObject.CreateInstance<GaiaSceneCullingProfile>();
            GaiaSettings gaiaSettings = GaiaUtils.GetGaiaSettings();
            asset.UpdateDefaults(gaiaSettings);
            AssetDatabase.CreateAsset(asset, "Assets/Gaia Scene Culling Profile.asset");
            AssetDatabase.SaveAssets();
            return asset;
        }
        [MenuItem("Assets/Create/Procedural Worlds/Gaia/Gaia Scene Culling Profile")]
        public static void CreateCullingProfileMenu()
        {
            GaiaSceneCullingProfile asset = ScriptableObject.CreateInstance<GaiaSceneCullingProfile>();
            GaiaSettings gaiaSettings = GaiaUtils.GetGaiaSettings();
            asset.UpdateDefaults(gaiaSettings);
            AssetDatabase.CreateAsset(asset, "Assets/Gaia Scene Culling Profile.asset");
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
#endif
    }
}