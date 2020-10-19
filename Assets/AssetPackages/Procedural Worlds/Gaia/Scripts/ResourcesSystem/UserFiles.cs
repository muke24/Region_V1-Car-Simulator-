using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gaia
{

    public class UserFiles : ScriptableObject
    { 
        public bool m_autoAddNewFiles = true;
        public bool m_updateFilesWithGaiaUpdate = true;
        public List<BiomePreset> m_gaiaManagerBiomePresets = new List<BiomePreset>();
        public List<SpawnerSettings> m_gaiaManagerSpawnerSettings = new List<SpawnerSettings>();
    }
}