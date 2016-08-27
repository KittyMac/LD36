/*
This is free software distributed under the terms of the MIT license, reproduced below. It may be used for any purpose, including commercial purposes, at absolutely no cost. No paperwork, no royalties, no GNU-like "copyleft" restrictions. Just download and enjoy.

Copyright (c) 2014 Chimera Software, LLC

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/


using UnityEngine;
using System.Xml;
using System.Collections;
using UnityEngine.UI;

public class PUScalar : PUGameObject {

	private Vector2 designedSize;

	public override void gaxb_final(XmlReader reader, object _parent, Hashtable args) {
		base.gaxb_final (reader, _parent, args);
		ScheduleForUpdate ();
	}

	// This is required for application-level subclasses
	public override void gaxb_init ()
	{

		if (title == null) {
			title = "Scalar";
		}
		pivot = new Vector2 (0.5f, 0.5f);
		anchor = "middle,center";

		base.gaxb_init ();
		gaxb_addToParent();

		designedSize = new Vector2(bounds.Value.z, bounds.Value.w);

		NotificationCenter.addImmediateObserver (this, "UI_SCALE_AMOUNT", null, (args, name) => {
			float baseScale = 1.0f;
			size = new Vector2 (designedSize.x * baseScale, designedSize.y * baseScale);
			bounds = new Vector4(0, 0, size.Value.x, size.Value.y);
		});

	}

	public override void gaxb_complete ()
	{
		Update ();
	}

	public override void Update() {
		RectTransform parentTransform = gameObject.transform.parent as RectTransform;
		if (parentTransform.rect.width > 0 && parentTransform.rect.height > 0) {
			float designedAspect = size.Value.x / size.Value.y;
			float screenAspect = parentTransform.rect.width / parentTransform.rect.height;
			float scale = 1.0f;

			if (designedAspect <= screenAspect) {
				scale = parentTransform.rect.width / size.Value.x;
			} else {
				scale = parentTransform.rect.height / size.Value.y;
			}

			gameObject.transform.localScale = new Vector3 (scale, scale, scale);

			rectTransform.sizeDelta = new Vector2 (parentTransform.rect.width / scale, parentTransform.rect.height / scale);
		}
	}


}
