using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialTypesController : MonoBehaviour {

	public static MaterialTypesController i_Instance;

	[SerializeField]
	private List<MaterialTypeConfig> materialsConfigList;
	private Dictionary<string, MaterialTypeConfig> materialsConfig;

	void Awake() {
		if(i_Instance == null) i_Instance = this;
		materialsConfig = new Dictionary<string, MaterialTypeConfig>();
		foreach(MaterialTypeConfig c in materialsConfigList) {
			materialsConfig.Add(c.name, c);
		}
	}
	
	public MaterialTypeConfig GetConfig(string name) {
		if(!materialsConfig.ContainsKey(name)) return null;
		return materialsConfig[name];
	}
	
}
