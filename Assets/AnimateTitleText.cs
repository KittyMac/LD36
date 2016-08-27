using UnityEngine;
using System.Collections;

public class AnimateTitleText : MonoBehaviour {

	private bool canAdvance = false;

	void Start () {
		RectTransform rectTransform = transform as RectTransform;

		LeanTween.value (gameObject, (v) => {
			rectTransform.anchoredPosition = new Vector2(0, v);
		}, -682, 0, 8);

		LeanTween.delayedCall (5, () => {
			canAdvance = true;
		});
	}

	void Update() {
		if (canAdvance && Input.GetMouseButton (0)) {
			NotificationCenter.postNotification (null, "FinishCutscene");
		}
	}
}
