using UnityEngine;
using System.Xml;
using System.Collections;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ScalarComponent : MonoBehaviour {

	public Vector2 size = new Vector2(1024,768);

	private RectTransform rectTransform;

	public void Start ()
	{
		rectTransform = transform as RectTransform;
		rectTransform.pivot = new Vector2 (0.5f, 0.5f);
		rectTransform.anchorMax = new Vector2 (0.5f, 0.5f);
		rectTransform.anchorMin = new Vector2 (0.5f, 0.5f);

		Update ();
	}

	public void Update() {
		RectTransform parentTransform = gameObject.transform.parent as RectTransform;
		if (parentTransform.rect.width > 0 && parentTransform.rect.height > 0) {
			float designedAspect = size.x / size.y;
			float screenAspect = parentTransform.rect.width / parentTransform.rect.height;
			float scale = 1.0f;

			if (designedAspect > screenAspect) {
				scale = parentTransform.rect.width / size.x;
			} else {
				scale = parentTransform.rect.height / size.y;
			}

			gameObject.transform.localScale = new Vector3 (scale, scale, scale);
		}
	}


}
