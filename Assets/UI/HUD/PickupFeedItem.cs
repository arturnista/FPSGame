using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupFeedItem : MonoBehaviour {

	private Text m_ItemText;
	private Image m_ItemIcon;

	private float m_Duration = 2f;
	[System.Serializable]
	public struct NamedIcons {
		public string name;
		public Sprite icon;
	}
	[SerializeField]
	private NamedIcons[] m_NamedIcons;
	private Dictionary<string, Sprite> m_Icons;

	void Awake() {
		m_ItemText = transform.Find("PickupFeedItemText").GetComponent<Text>();
		m_ItemIcon = transform.Find("PickupFeedItemIcon").GetComponent<Image>();
		m_Icons = new Dictionary<string, Sprite>();
		foreach(NamedIcons icon in m_NamedIcons) {
			m_Icons.Add(icon.name, icon.icon);
		}
	}

	public void Configure (string name, int amount) {
		Invoke("DestroyItem", m_Duration);
		m_ItemText.text = amount.ToString();
		m_ItemIcon.sprite = m_Icons[name];
	}
	
	void DestroyItem () {
		Destroy(this.gameObject);
	}
}
