using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class MainController : MonoBehaviour {

	public void Start () {
		StoryEngine engine = new StoryEngine ();

		engine.LoadStoryFromMarkdown ("StoryEngine/story.md");
	}
}