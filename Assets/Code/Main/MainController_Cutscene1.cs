using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public partial class MainController : MonoBehaviour {

	public void PerformCutscene(StoryEngine.Cutscene cutscene) {
		string sceneName = "Scenes/" + cutscene.hashtag.Trim ("#".ToCharArray ());

		SceneManager.LoadScene (sceneName, LoadSceneMode.Additive);

		NotificationCenter.addObserver (this, "FinishCutscene", null, (args, name) => {
			SceneManager.UnloadScene(sceneName);
			LoadRoom(cutscene.room);
			NotificationCenter.removeObserver(this, "FinishCutscene");
		});

		// quick hack to get past the cutscene
		if (cutscene.hashtag.Equals ("##Cutscene1")) {
			LeanTween.delayedCall (4, () => {
				NotificationCenter.postNotification (null, "FinishCutscene");
			});
		}
	}

}