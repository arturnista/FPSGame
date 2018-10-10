using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetriNetElement {

	protected string name;

	protected List<PetriNetArc> inputs;
	protected List<PetriNetArc> outputs;

	public string Name {
		get {
			return name;
		}
	}

	public PetriNetElement (string name) {
		outputs = new List<PetriNetArc> ();
		inputs = new List<PetriNetArc> ();

		this.name = name;
	}

	public void AddInput (PetriNetArc arc) {
		inputs.Add (arc);
	}

	public void AddOutput (PetriNetArc arc) {
		outputs.Add (arc);
	}

}