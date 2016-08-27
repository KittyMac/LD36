using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class DialogTableCell : PUTableCell {

	public override void UpdateContents() {
		StoryEngine.Dialog data = cellData as StoryEngine.Dialog;

		PUTextButton responseText = new PUTextButton ();

		responseText.font = "Fonts/PressStart2P";
		responseText.value = data.text;
		responseText.fontSize = 24;
		responseText.lineSpacing = 1.4f;
		responseText.fontColor = Color.white;
		responseText.alignment = PlanetUnity2.TextAlignment.upperLeft;
		responseText.SetFrame (0, 0, 378, 100, 0, 0, "stretch,stretch");
		responseText.LoadIntoPUGameObject (puGameObject);

		responseText.SetStretchStretch (30, 0, 30, 0);

		TextGenerator textGen = new TextGenerator();
		TextGenerationSettings generationSettings = responseText.text.GetGenerationSettings(responseText.rectTransform.rect.size); 
		float height = textGen.GetPreferredHeight(data.text, generationSettings) + 60;

		puGameObject.rectTransform.sizeDelta = new Vector2 (responseText.rectTransform.rect.width, height);

		//<Text title="CharacterDialog" font="Fonts/PressStart2P" sizeToFit="true" maxFontSize="24" lineSpacing="1.4" fontColor="#FFFFFFFF" alignment="upperCenter" bounds="0,0,546,220" />

		responseText.button.onClick.AddListener (() => {
			NotificationCenter.postNotification(null, "NavigateToRoom", NotificationCenter.Args("room", data.room));
		});

	}

}