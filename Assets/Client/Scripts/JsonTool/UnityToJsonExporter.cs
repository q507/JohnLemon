using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
public class UnityToJsonExporter
{
	[MenuItem("JsonTool/ExportNewJson")]
	static void ExportActiveScene() 
	{
		GameObject obj = GameObject.Find("JsonTool");

		ToJsonGameObject tool = obj.GetComponent<ToJsonGameObject>();

		if (tool != null)
        {
			tool.ExportSceneListJson();
			Debug.Log("生成Json");
        }
	}
}
