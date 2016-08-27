using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

[InitializeOnLoad]
[ExecuteInEditMode]
class AddProjectScenes : AssetPostprocessor
{
	static AddProjectScenes ()
	{
		ReloadBuildScenes ();
	}

	static void ReloadBuildScenes ()
	{
		//Debug.Log ("AddProjectScenes");

		List<string> SceneFiles = new List<string> ();

		DirectoryInfo dir = new DirectoryInfo (Application.dataPath + "/Scenes");
		FileInfo[] info = dir.GetFiles("*.*", SearchOption.AllDirectories);
		foreach (FileInfo fileInfo in info)  {
			if (fileInfo.FullName.EndsWith (".unity")) {
				SceneFiles.Add (fileInfo.FullName);
			}
		}

		SceneFiles.Sort(delegate(string x, string y)
			{
				int indexX = 9999;
				int indexY = 9999;

				x = Path.GetFileName(x);
				y = Path.GetFileName(y);

				if(x.Contains("_")){
					if(!int.TryParse(x.Substring(0, x.IndexOf("_")), out indexX)){
						indexX = 9999;
					}
				}

				if(y.Contains("_")){
					if(!int.TryParse(y.Substring(0, y.IndexOf("_")), out indexY)){
						indexY = 9999;
					}
				}

				return indexX.CompareTo(indexY);
			});

		EditorBuildSettingsScene[] newSettings = new EditorBuildSettingsScene[SceneFiles.Count];

		int index = 0;
		foreach (string sceneFile in SceneFiles) {
			newSettings [index] = new EditorBuildSettingsScene (sceneFile.Substring(sceneFile.IndexOf("Assets")), true);
			index++;
		}

		EditorBuildSettings.scenes = newSettings;
	}


	static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromPath)
	{
		ReloadBuildScenes ();
	}
}