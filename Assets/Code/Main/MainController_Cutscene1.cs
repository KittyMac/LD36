using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public partial class MainController : MonoBehaviour {

	public void PerformCutscene(StoryEngine.Cutscene cutscene) {
		string sceneName = "Scenes/" + cutscene.hashtag.Trim ("#".ToCharArray ());

		SceneManager.LoadScene (sceneName, LoadSceneMode.Additive);
		LeanTween.delayedCall (4.0f, () => {
			SceneManager.UnloadScene(sceneName);

			LoadRoom(cutscene.room);
		});
	}

}