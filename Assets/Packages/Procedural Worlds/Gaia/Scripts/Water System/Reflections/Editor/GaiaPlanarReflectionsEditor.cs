using UnityEditor;

namespace Gaia
{
    [CustomEditor(typeof(GaiaPlanarReflections))]
    public class GaiaPlanarReflectionsEditor : Editor
    {
        private const string m_helpText = "Gaia Planar Reflections is the reflection system for SRP this system only work in URP/HDRP. This system uses the SRP camera commands to render reflections in realtime. This system also supports shadows, custom render distances and unity layers.";

        /// <summary>
        /// Custom editor for PWS_WaterReflections
        /// </summary
        public override void OnInspectorGUI()
        {
            //Initialization
            EditorGUILayout.HelpBox(m_helpText, MessageType.Info);
        }
    }
}