using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class VersionToText : MonoBehaviour
{
	public string version = "1.0";
	public string path;

    // Start is called before the first frame update
    void Start()
    {
		
		CreateText();
	}

	void CreateText()
	{
		path = Application.persistentDataPath + "/Version.txt";
		File.WriteAllText(path, version);
	}

}
