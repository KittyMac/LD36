using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class MainController : MonoBehaviour {

	public PUText CharacterDialog;
	public PUImage CharacterImage;
	public PUTable ResponsesTable;

	private string StartingRoom = "#StartingRoom";
	private string CurrentRoom = null;
	private StoryEngine engine = new StoryEngine ();

	public void Start () {
		engine.LoadStoryFromMarkdown ("StoryEngine/story.md");

		LoadRoom (StartingRoom);

		NotificationCenter.addObserver (this, "NavigateToRoom", null, (args, name) => {
			LoadRoom (args ["room"].ToString ());
		});
	}

	public void LoadRoom(string roomName) {
		StoryEngine.Room room = engine.GetRoom (roomName);
		if (room == null) {
			return;
		}

		CurrentRoom = roomName;

		CharacterImage.LoadImageWithResourcePath ("StoryEngine/" + room.character);
		if (CharacterImage.image.sprite != null) {
			CharacterImage.rectTransform.sizeDelta = CharacterImage.image.sprite.textureRect.size;
		}

		CharacterDialog.text.text = string.Format ("{0}\n\n\"{1}\"", room.character, room.text);


		List<object> allItemsForTable = new List<object> ();
		foreach (StoryEngine.Dialog dialog in room.responses) {
			allItemsForTable.Add (dialog);
		}

		ResponsesTable.SetObjectList (allItemsForTable);
		ResponsesTable.ReloadTable ();
	}
}