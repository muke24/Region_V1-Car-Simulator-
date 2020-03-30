using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class VersionToText : MonoBehaviour
{
	public string version;
	public string path;

    // Start is called before the first frame update
    void Start()
    {
		version = Application.version.ToString();
		CreateText();
	}

	void CreateText()
	{
		path = Application.persistentDataPath + "/CurrentVersion.txt";
		File.WriteAllText(path, version);
	}

}
