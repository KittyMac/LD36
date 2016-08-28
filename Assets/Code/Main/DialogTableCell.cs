using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


public class DialogTableCell : PUTableCell {

	public enum DialogType {
		Character,
		Response,
		Header
	}

	static public float AnimateTextDuration(DialogType type, string content) {
		if (type == DialogType.Character) {
			return content.Length * 0.05f;
		} else if (type == DialogType.Response) {
			return 0.47f;
		}

		return 0;
	}

	static public float AnimateText(DialogType type, PUText text, float delay) {
		if (type == DialogType.Character) {
			return AnimateTextTyping (type, text, delay);
		} else if (type == DialogType.Response) {
			return AnimateTextMoveIn (type, text, delay);
		}

		return AnimateTextMoveIn (type, text, delay);
	}


	static private float AnimateTextMoveIn(DialogType type, PUText text, float delay) {

		float duration = AnimateTextDuration (type, text.text.text);

		if (type == DialogType.Response) {
			// animate in from the right

			text.rectTransform.anchoredPosition = new Vector2(text.rectTransform.rect.width, 0);
			LeanTween.value (text.gameObject, (v) => {
				text.rectTransform.anchoredPosition = new Vector2(v, 0);
			}, text.rectTransform.rect.width, 0, duration).setEase (LeanTweenType.easeOutCubic).setDelay (delay);

		} else if(type == DialogType.Character) {
			// animate in from the bottom

			text.rectTransform.anchoredPosition = new Vector2(0, -text.rectTransform.rect.height);
			LeanTween.value (text.gameObject, (v) => {
				text.rectTransform.anchoredPosition = new Vector2(0, -v);
			}, text.rectTransform.rect.height, 0, duration).setEase (LeanTweenType.easeOutCubic).setDelay (delay);

		}

		return duration + delay;
	}

	static private float AnimateTextTyping(DialogType type, PUText text, float delay) {
		string content = text.text.text;

		text.text.text = "";
		LeanTween.value (text.gameObject, (v) => {
			text.text.text = content.Substring(0, Mathf.RoundToInt(v));
		}, 0, content.Length, AnimateTextDuration(type, content)).setEase (LeanTweenType.linear).setDelay (delay);

		return AnimateTextDuration(type, content) + delay;
	}

	// **************************************************************************************************************

	public override void UnloadContents() {
		LeanTween.cancel (responseText.gameObject);
	}

	private PUTextButton responseText;

	public override void UpdateContents() {
		StoryEngine.Dialog data = cellData as StoryEngine.Dialog;

		responseText = new PUTextButton ();

		responseText.font = "Fonts/PressStart2P";
		responseText.value = data.text;
		responseText.fontSize = 22;
		if (data.isHelpHeader) {
			responseText.fontSize = 14;
		}
		responseText.lineSpacing = 1.4f;
		responseText.fontColor = Color.white;
		responseText.alignment = PlanetUnity2.TextAlignment.upperLeft;
		responseText.SetFrame (0, 0, 378, 100, 0.5f, 0.5f, "stretch,stretch");
		responseText.LoadIntoPUGameObject (puGameObject);

		responseText.SetStretchStretch (30, 0, 30, 0);

		TextGenerator textGen = new TextGenerator();
		TextGenerationSettings generationSettings = responseText.text.GetGenerationSettings(responseText.rectTransform.rect.size); 
		float height = textGen.GetPreferredHeight(data.text, generationSettings) + 60;

		puGameObject.rectTransform.sizeDelta = new Vector2 (responseText.rectTransform.rect.width, height);

		if (data.isHelpHeader) {
			responseText.text.color = Color.yellow;

			LeanTween.value (responseText.gameObject, (v) => {
				responseText.rectTransform.localScale = new Vector3 (v, v, 1.0f);
			}, 0.98f, 1.02f, 2.0f).setLoopCount (-1).setLoopPingPong ().setEase (LeanTweenType.easeInOutCirc);

		} else {
			responseText.button.onClick.AddListener (() => {
				NotificationCenter.postNotification (null, "NavigateToRoom", NotificationCenter.Args ("room", data.room));
			});
		}

		AnimateText (DialogType.Response, responseText, data.animationDelay);

		/*
		responseText.rectTransform.anchoredPosition = new Vector2(responseText.rectTransform.rect.width, 0);
		LeanTween.value (puGameObject.gameObject, (v) => {
			responseText.rectTransform.anchoredPosition = new Vector2(v, 0);
		}, responseText.rectTransform.rect.width, 0, 0.47f).setDelay (UnityEngine.Random.Range (0.67f, 1.32f)).setEase (LeanTweenType.easeInOutQuad);
		*/
	}

}