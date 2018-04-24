using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunsFeedItem : MonoBehaviour {

	private Text m_ItemText;
	private Image m_ItemIcon;

	private float m_Duration = 2f;
	[System.Serializable]
	public struct NamedIcons {
		public string name;
		public string hotkey;
		public Sprite icon;
	}
	[SerializeField]
	private NamedIcons[] m_NamedIcons;
	private Dictionary<string, NamedIcons> m_Icons;

	void Awake() {
		m_ItemText = transform.Find("GunsFeedItemText").GetComponent<Text>();
		m_ItemIcon = transform.Find("GunsFeedItemIcon").GetComponent<Image>();
		m_Icons = new Dictionary<string, NamedIcons>();
		foreach(NamedIcons icon in m_NamedIcons) {
			m_Icons.Add(icon.name, icon);
		}
	}

	public NamedIcons Configure (string name) {
		m_ItemText.text = m_Icons[name].hotkey;
		m_ItemIcon.sprite = m_Icons[name].icon;
		
		return m_Icons[name];
	}
	
}
