using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public partial class MainController : MonoBehaviour {

	public PUText CharacterDialog;
	public PUImage CharacterImage;
	public PUImage Spotlight;
	public PUTable ResponsesTable;
	public PUImage PlayerImage;

	private string StartingRoom = "#StartingRoom";
	private string CurrentRoom = null;
	private StoryEngine engine = new StoryEngine ();

	public void Start () {
		engine.LoadStoryFromMarkdown ("StoryEngine/story.md");

		CharacterImage.image.color = Color.black;
		Spotlight.CheckCanvasGroup ();
		Spotlight.canvasGroup.alpha = 0.0f;

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

		if (room is StoryEngine.Cutscene) {
			PerformCutscene (room as StoryEngine.Cutscene);

			CharacterImage.image.color = Color.black;
			Spotlight.CheckCanvasGroup ();
			Spotlight.canvasGroup.alpha = 0.0f;

			AnimateOutPlayer (0);
			return;
		}

		CharacterDialog.text.text = string.Format ("{0}\n\n\"{1}\"", room.character, room.text);

		float currentDelay = 0.0f;

		currentDelay += SwitchToCharacter(room.character);

		currentDelay += DialogTableCell.AnimateText (DialogTableCell.DialogType.Character, CharacterDialog, currentDelay);

		List<object> allItemsForTable = new List<object> ();
		foreach (StoryEngine.Dialog dialog in room.responses) {
			dialog.animationDelay = currentDelay;
			currentDelay += DialogTableCell.AnimateTextDuration (DialogTableCell.DialogType.Response, dialog.text);
			allItemsForTable.Add (dialog);
		}

		ResponsesTable.SetObjectList (allItemsForTable);
		ResponsesTable.ReloadTable ();


	}
		
	private float SwitchToCharacter(string name) {
		string characterPath = "StoryEngine/" + name;
		float animateInTime = 0.6f;
		float animateOutTime = animateInTime * 0.6f;

		// If the character is already there, don't do anything
		if (CharacterImage.resourcePath != null && CharacterImage.resourcePath.Equals (characterPath)) {
			return 0.0f;
		}
			
		// Animate out the current character!
		if (CharacterImage.resourcePath != null) {
			

			if (Spotlight.canvasGroup.alpha > 0.5f) {
				AnimateOutPlayer (0);

				LeanTween.value (Spotlight.gameObject, (v) => {
					Spotlight.canvasGroup.alpha = v;
					CharacterImage.image.color = new Color (v, v, v, 1.0f);
				}, 1.0f, 0.0f, animateOutTime);
			}
		}

		SetPlayerStartAnimationPosition ();

		LeanTween.delayedCall (animateOutTime, () => {

			AnimateInPlayer(0);

			// Load in the new character image
			CharacterImage.LoadImageWithResourcePath (characterPath);
			if (CharacterImage.image.sprite != null) {
				CharacterImage.rectTransform.sizeDelta = CharacterImage.image.sprite.textureRect.size;
			}
		});

		// Animate in the new character
		// animate in the character
		Spotlight.canvasGroup.alpha = 0.0f;
		LeanTween.value (Spotlight.gameObject, (v) => {
			Spotlight.canvasGroup.alpha = v;
			CharacterImage.image.color = new Color (v, v, v, 1.0f);
		}, 0.0f, 1.0f, animateInTime).setDelay (animateOutTime);

		return animateInTime + animateOutTime;
	}


	private void SetPlayerStartAnimationPosition() {
		PlayerImage.rectTransform.anchoredPosition += new Vector2 (PlayerImage.rectTransform.rect.width * 2.0f, 0.0f);
	}

	private float AnimateInPlayer(float delay) {
		float duration = 0.67f;

		float fromX = 775.0f + PlayerImage.rectTransform.rect.width * 2.0f;
		float toX = 775.0f;

		PlayerImage.rectTransform.anchoredPosition = new Vector2 (fromX, PlayerImage.rectTransform.anchoredPosition.y);
		LeanTween.value (Spotlight.gameObject, (v) => {
			PlayerImage.rectTransform.anchoredPosition = new Vector2 (v, PlayerImage.rectTransform.anchoredPosition.y);
		}, fromX, toX, duration).setDelay (delay).setEase (LeanTweenType.easeInQuad);

		return duration;
	}

	private float AnimateOutPlayer(float delay) {
		float duration = 0.47f;

		float toX = 775.0f + PlayerImage.rectTransform.rect.width * 2.0f;
		float fromX = 775.0f;

		PlayerImage.rectTransform.anchoredPosition = new Vector2 (fromX, PlayerImage.rectTransform.anchoredPosition.y);
		LeanTween.value (Spotlight.gameObject, (v) => {
			PlayerImage.rectTransform.anchoredPosition = new Vector2 (v, PlayerImage.rectTransform.anchoredPosition.y);
		}, fromX, toX, duration).setDelay (delay).setEase (LeanTweenType.easeInQuad);

		return duration;
	}

}