using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Gaia
{
    [ExecuteAlways]
    public class GaiaScenePlayer : MonoBehaviour
    {
        private Camera m_camera;
        private float m_cameraFOV;
        private Vector3 m_FOVCenter;
        private Bounds m_worldSpaceBounds = new Bounds();
        private Plane[] m_planes = new Plane[6];
        private Terrain[] m_allTerrains = new Terrain[0];

        private void Start()
        {
            if (!GaiaUtils.CheckIfSceneProfileExists())
            {
                return;
            }

            m_camera = GaiaGlobal.Instance.m_mainCamera;
            m_allTerrains = Terrain.activeTerrains;
            if (GaiaGlobal.Instance.SceneProfile.m_terrainCullingEnabled)
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
                SceneManager.sceneLoaded += OnSceneLoaded;
                SceneManager.sceneUnloaded -= OnSceneUnLoaded;
                SceneManager.sceneUnloaded += OnSceneUnLoaded;
            }
            else
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
                SceneManager.sceneUnloaded -= OnSceneUnLoaded;
            }

            UpdateCullingDistances();
        }
        private void Update()
        {
            if (!Application.isPlaying)
            {
                return;
            }
            if (!GaiaUtils.CheckIfSceneProfileExists())
            {
                return;
            }
            if (!GaiaGlobal.Instance.SceneProfile.m_terrainCullingEnabled || m_camera == null)
            {
                return;
            }

            Vector3 cameraForward = m_camera.transform.forward;
            float cameraViewDistance = m_camera.farClipPlane;
            m_cameraFOV = m_camera.fieldOfView;

            m_FOVCenter = new Vector3(cameraForward.x, cameraForward.z).normalized * cameraViewDistance;
            for (int i = 0; i < m_allTerrains.Length; i++)
            {
                Terrain terrain = m_allTerrains[i];
                if(terrain==null)
                {
                    continue;
                }
                //Check needs to performed in world space, terrain bounds are in local space of the terrain
                m_worldSpaceBounds = terrain.terrainData.bounds;
                m_worldSpaceBounds.center = new Vector3(m_worldSpaceBounds.center.x + terrain.transform.position.x, m_worldSpaceBounds.center.y + terrain.transform.position.y, m_worldSpaceBounds.center.z + terrain.transform.position.z);

                GeometryUtility.CalculateFrustumPlanes(m_camera, m_planes);
                if (GeometryUtility.TestPlanesAABB(m_planes, m_worldSpaceBounds))
                {
                    terrain.drawHeightmap = true;
                    terrain.drawTreesAndFoliage = true;
                }
                else
                {
                    terrain.drawHeightmap = false;
                    terrain.drawTreesAndFoliage = false;
                }
            }
        }
        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnLoaded;
        }

        private void OnEnable()
        {
            if (!GaiaUtils.CheckIfSceneProfileExists())
            {
                return;
            }
            if (Application.isPlaying)
            {
                UpdateCullingDistances();
            }
            else
            {
                if (GaiaGlobal.Instance.SceneProfile.m_cullingProfile != null)
                {
                    ApplySceneSetup(GaiaGlobal.Instance.SceneProfile.m_cullingProfile.m_applyToEditorCamera);
                }
            }
        }

        //Terrain Culling
        private void OnSceneUnLoaded(Scene arg0)
        {
            Invoke("UpdateTerrains", 0.5f);
        }
        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            Invoke("UpdateTerrains", 0.5f);
        }
        private void UpdateTerrains()
        {
            m_allTerrains = Terrain.activeTerrains;
        }

        //Camera Culling
        public static void UpdateCullingDistances()
        {
            if (!GaiaUtils.CheckIfSceneProfileExists())
            {
                return;
            }

            if (GaiaGlobal.Instance.SceneProfile.m_cullingProfile == null)
            {
                return;
            }

#if GAIA_PRO_PRESENT
            if (ProceduralWorldsGlobalWeather.Instance != null)
            {
                if (ProceduralWorldsGlobalWeather.Instance.CheckIsNight())
                {
                    GaiaGlobal.Instance.SceneProfile.m_sunLight = ProceduralWorldsGlobalWeather.Instance.m_moonLight;
                }
                else
                {
                    GaiaGlobal.Instance.SceneProfile.m_sunLight = ProceduralWorldsGlobalWeather.Instance.m_sunLight;
                }
            }
            else
            {
                if (GaiaGlobal.Instance.SceneProfile.m_sunLight == null)
                {
                    GaiaGlobal.Instance.SceneProfile.m_sunLight = GaiaUtils.GetMainDirectionalLight();
                }
            }
#else
            if (GaiaGlobal.Instance.SceneProfile.m_sunLight == null)
            {
                GaiaGlobal.Instance.SceneProfile.m_sunLight = GaiaUtils.GetMainDirectionalLight();
            }
#endif

            //Make sure we have distances
            if (GaiaGlobal.Instance.SceneProfile.m_cullingProfile.m_layerDistances == null || GaiaGlobal.Instance.SceneProfile.m_cullingProfile.m_layerDistances.Length != 32)
            {
                return;
            }

            if (GaiaGlobal.Instance.SceneProfile.m_enableLayerCulling)
            {
                //Apply to main camera
                GaiaGlobal.Instance.m_mainCamera.layerCullDistances = GaiaGlobal.Instance.SceneProfile.m_cullingProfile.m_layerDistances;

                if (GaiaGlobal.Instance.SceneProfile.m_sunLight != null)
                {
                    GaiaGlobal.Instance.SceneProfile.m_sunLight.layerShadowCullDistances = GaiaGlobal.Instance.SceneProfile.m_cullingProfile.m_shadowLayerDistances;
                }
            }
            else
            {
                float[] layerCulls = new float[32];
                for (int i = 0; i < layerCulls.Length; i++)
                {
                    layerCulls[i] = 0f;
                }

                //Apply to main camera
                GaiaGlobal.Instance.m_mainCamera.layerCullDistances = layerCulls;

                if (GaiaGlobal.Instance.SceneProfile.m_sunLight != null)
                {
                    GaiaGlobal.Instance.SceneProfile.m_sunLight.layerShadowCullDistances = layerCulls;
                }
            }
        }
        public static void ApplySceneSetup(bool active)
        {
            //Apply to editor camera
#if UNITY_EDITOR
            if (GaiaGlobal.Instance.SceneProfile.m_enableLayerCulling)
            {
                if (active)
                {
                    foreach (var sceneCamera in SceneView.GetAllSceneCameras())
                    {
                        sceneCamera.layerCullDistances = GaiaGlobal.Instance.SceneProfile.m_cullingProfile.m_layerDistances;
                    }

                    if (GaiaGlobal.Instance.SceneProfile.m_sunLight != null)
                    {
                        GaiaGlobal.Instance.SceneProfile.m_sunLight.layerShadowCullDistances = GaiaGlobal.Instance.SceneProfile.m_cullingProfile.m_shadowLayerDistances;
                    }
                }
                else
                {
                    foreach (var sceneCamera in SceneView.GetAllSceneCameras())
                    {
                        float[] layers = new float[32];
                        for (int i = 0; i < layers.Length; i++)
                        {
                            layers[i] = 0f;
                        }

                        sceneCamera.layerCullDistances = layers;
                    }

                    if (GaiaGlobal.Instance.SceneProfile.m_sunLight != null)
                    {
                        float[] layers = new float[32];
                        for (int i = 0; i < layers.Length; i++)
                        {
                            layers[i] = 0f;
                        }
                        GaiaGlobal.Instance.SceneProfile.m_sunLight.layerShadowCullDistances = layers;
                    }
                }
            }
            else
            {
                foreach (var sceneCamera in SceneView.GetAllSceneCameras())
                {
                    float[] layers = new float[32];
                    for (int i = 0; i < layers.Length; i++)
                    {
                        layers[i] = 0f;
                    }

                    sceneCamera.layerCullDistances = layers;
                }

                if (GaiaGlobal.Instance.SceneProfile.m_sunLight != null)
                {
                    float[] layers = new float[32];
                    for (int i = 0; i < layers.Length; i++)
                    {
                        layers[i] = 0f;
                    }
                    GaiaGlobal.Instance.SceneProfile.m_sunLight.layerShadowCullDistances = layers;
                }
            }
#endif
        }

        //Controller Setup
        /// <summary>
        /// Sets the current controller type
        /// </summary>
        /// <param name="type"></param>
        public static void SetCurrentControllerType(GaiaConstants.EnvironmentControllerType type)
        {
            LocationSystem system = FindObjectOfType<LocationSystem>();
            if (system != null)
            {
                if (system.m_locationProfile != null)
                {
                    system.m_locationProfile.m_currentControllerType = type;
                }
            }
        }
    }
}