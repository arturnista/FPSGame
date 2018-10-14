using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ButtonText : Button, IPointerEnterHandler, IPointerExitHandler {

    private TextMeshProUGUI mText;

	protected override void Awake () {
        base.Awake();
        mText = GetComponent<TextMeshProUGUI>();
        mText.fontStyle = FontStyles.SmallCaps;
	}
	
    public override void OnPointerEnter(PointerEventData pointerEventData) {
        // mText.fontStyle = FontStyles.Underline;
        mText.fontStyle = (FontStyles) ((int) FontStyles.SmallCaps) + ((int) FontStyles.Underline);
        Debug.Log("Cursor Entering " + name + " GameObject");
    }

    public override void OnPointerExit(PointerEventData pointerEventData) {
        mText.fontStyle = FontStyles.SmallCaps;
        Debug.Log("Cursor Exiting " + name + " GameObject");
    }
}
