using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PWCommon3;
using Gaia.Internal;
using System;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Gaia
{
    [CustomEditor(typeof(TerrainLoaderManager))]
    public class TerrainLoaderManagerEditor : PWEditor, IPWEditor
    {
        private TerrainLoaderManager m_terrainLoaderManager;
        #if GAIA_PRO_PRESENT
        private TerrainLoader[] m_terrainLoaders;
        #endif
        private EditorUtils m_editorUtils;

        public void OnEnable()
        {
            m_terrainLoaderManager = (TerrainLoaderManager)target;
            #if GAIA_PRO_PRESENT
            m_terrainLoaders = Resources.FindObjectsOfTypeAll<TerrainLoader>();
            #endif
            //m_placeHolders = Resources.FindObjectsOfTypeAll<GaiaTerrainPlaceHolder>();
            //m_placeHolders = m_placeHolders.OrderBy(x => x.name).ToArray();
            //foreach (GaiaTerrainPlaceHolder placeHolder in m_placeHolders)
            //{
            //    placeHolder.UpdateLoadState();
            //}

            //Init editor utils
            if (m_editorUtils == null)
            {
                // Get editor utils for this
                m_editorUtils = PWApp.GetEditorUtils(this);
            }
        }


        public override void OnInspectorGUI()
        {
            //Init editor utils
            if (m_editorUtils == null)
            {
                // Get editor utils for this
                m_editorUtils = PWApp.GetEditorUtils(this);
            }
            m_editorUtils.Initialize(); // Do not remove this!
            if (GaiaUtils.HasDynamicLoadedTerrains())
            {
                m_editorUtils.Panel("LoaderPanel", DrawLoaders, true);
                m_editorUtils.Panel("PlaceholderPanel", DrawTerrains, true);
            }
            else
            {
                EditorGUILayout.HelpBox(m_editorUtils.GetTextValue("NoTerrainLoadingMessage"),MessageType.Info);
            }
        }

        private void DrawTerrains(bool helpEnabled)
        {
            EditorGUILayout.BeginHorizontal();
            if (m_editorUtils.Button("AddToBuildSettings"))
            {
                if (EditorUtility.DisplayDialog(m_editorUtils.GetTextValue("AddToBuildSettingsPopupTitle"), m_editorUtils.GetTextValue("AddToBuildSettingsPopupText"), m_editorUtils.GetTextValue("Continue"), m_editorUtils.GetTextValue("Cancel")))
                {
                    #if GAIA_PRO_PRESENT
                    GaiaSessionManager.AddTerrainScenesToBuildSettings(m_terrainLoaderManager.TerrainSceneStorage.m_terrainScenes);
                    #endif
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (m_editorUtils.Button("UnloadAll"))
            {
                m_terrainLoaderManager.UnloadAll(true);
            }
            if (m_editorUtils.Button("LoadAll"))
            {
                if (EditorUtility.DisplayDialog(m_editorUtils.GetTextValue("LoadAllPopupTitle"), m_editorUtils.GetTextValue("LoadAllPopupText"), m_editorUtils.GetTextValue("Continue"), m_editorUtils.GetTextValue("Cancel")))
                {
                    foreach (TerrainScene terrainScene in m_terrainLoaderManager.TerrainSceneStorage.m_terrainScenes)
                    {
                        terrainScene.AddReference(m_terrainLoaderManager.gameObject);
                    }
                }
            }

            EditorGUILayout.EndHorizontal();
            float buttonWidth1 = 110;
            float buttonWidth2 = 60;
            foreach (TerrainScene terrainScene in m_terrainLoaderManager.TerrainSceneStorage.m_terrainScenes)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(terrainScene.GetTerrainName());

                bool isLoaded = terrainScene.m_loadState == LoadState.Loaded && terrainScene.TerrainObj != null && terrainScene.TerrainObj.activeInHierarchy;

                bool currentGUIState = GUI.enabled;
                GUI.enabled = isLoaded;
                if (m_editorUtils.Button("SelectPlaceholder", GUILayout.Width(buttonWidth1)))
                {
                    Selection.activeGameObject = GameObject.Find(terrainScene.GetTerrainName());
                    EditorGUIUtility.PingObject(Selection.activeObject);
                }
                GUI.enabled = currentGUIState;
                if (isLoaded)
                {
                    if (m_editorUtils.Button("UnloadPlaceholder", GUILayout.Width(buttonWidth2)))
                    {
                        terrainScene.RemoveAllReferences(true);
                    }
                }
                else
                {
                    if (m_editorUtils.Button("LoadPlaceholder", GUILayout.Width(buttonWidth2)))
                    {
                        terrainScene.AddReference(m_terrainLoaderManager.gameObject);
                    }
                }
                EditorGUILayout.EndHorizontal();
                if (terrainScene.References.Count > 0)
                {
                    EditorGUI.indentLevel++;
                    terrainScene.m_isFoldedOut = m_editorUtils.Foldout(terrainScene.m_isFoldedOut, "ShowTerrainReferences");
                    if (terrainScene.m_isFoldedOut)
                    {
                        
                        foreach (GameObject go in terrainScene.References)
                        {
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Space(20);
                            m_editorUtils.Label(new GUIContent(go.name, m_editorUtils.GetTextValue("TerrainReferenceToolTip")));
                            if (m_editorUtils.Button("TerrainReferenceSelect", GUILayout.Width(buttonWidth1)))
                            {
                                Selection.activeObject = go;
                                SceneView.lastActiveSceneView.FrameSelected();
                            }
                            if (m_editorUtils.Button("TerrainReferenceRemove", GUILayout.Width(buttonWidth2)))
                            {
                                terrainScene.RemoveReference(go);
                            }
                            GUILayout.Space(100);
                            EditorGUILayout.EndHorizontal();
                        }
                        
                    }
                    EditorGUI.indentLevel--;
                }
                GUILayout.Space(5f);
            }
        }

        private void DrawLoaders(bool helpEnabled)
        {
            bool oldEnabled = m_terrainLoaderManager.TerrainSceneStorage.m_terrainLoadingEnabled;
            m_terrainLoaderManager.TerrainSceneStorage.m_terrainLoadingEnabled = m_editorUtils.Toggle("TerrainLoadingEnabled", m_terrainLoaderManager.TerrainSceneStorage.m_terrainLoadingEnabled);
            if (!oldEnabled && m_terrainLoaderManager.TerrainSceneStorage.m_terrainLoadingEnabled)
            {
                //User re-enabled the loaders
                m_terrainLoaderManager.UpdateTerrainLoadState();
            }
            if (oldEnabled != m_terrainLoaderManager.TerrainSceneStorage.m_terrainLoadingEnabled)
            {
                //Value was changed, dirty the object to make sure the value is being saved
                EditorUtility.SetDirty(m_terrainLoaderManager.TerrainSceneStorage);
            }
            GUILayout.Space(10);
            m_editorUtils.Heading("TerrainLoaderHeader");
            m_editorUtils.InlineHelp("TerrainLoaderHeader", helpEnabled);
            EditorGUI.indentLevel++;
            EditorGUIUtility.labelWidth = 20;
#if GAIA_PRO_PRESENT
            foreach (TerrainLoader terrainLoader in m_terrainLoaders)
            {
                if (terrainLoader != null)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(terrainLoader.name);
                    terrainLoader.LoadMode = (LoadMode)EditorGUILayout.EnumPopup(terrainLoader.LoadMode);
                    if (m_editorUtils.Button("SelectLoader", GUILayout.MaxWidth(100)))
                    {
                        Selection.activeGameObject = terrainLoader.gameObject;
                        EditorGUIUtility.PingObject(Selection.activeObject);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
#endif
            EditorGUIUtility.labelWidth = 0;
            EditorGUI.indentLevel--;
        }
    }
}
