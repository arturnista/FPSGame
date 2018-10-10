using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetriNet {

	private List<PetriNetElement> elements;
	private List<PetriNetPlace> places;
	private List<PetriNetTransition> transitions;
	private List<PetriNetArc> arcs;

	public List<PetriNetArc> Arcs {
		get {
			return arcs;
		}
	}

	public PetriNet () {
		elements = new List<PetriNetElement> ();

		transitions = new List<PetriNetTransition> ();
		places = new List<PetriNetPlace> ();
		arcs = new List<PetriNetArc> ();
	}

	public PetriNet (string fileData) {
		elements = new List<PetriNetElement> ();

		transitions = new List<PetriNetTransition> ();
		places = new List<PetriNetPlace> ();
		arcs = new List<PetriNetArc> ();

		string[] strElements = fileData.Split('\n');
		foreach(string ele in strElements) {
			string[] data = ele.Split(';');
			switch(data[0]) {
				case "P":
					PetriNetPlace place = CreatePlace(data[1]);
					
					place.AddMarkers(int.Parse(data[2]));
					break;
				case "T":
					CreateTransition(data[1]);
					break;
				case "A":
					CreateArc(data[1], data[2]);
					break;
			}
		}
	}

	public override string ToString() {
		string data = "";
		foreach(PetriNetPlace place in places) {
			data += "P;" + place.Name + ";" + place.Markers + "\n";
		}
		foreach(PetriNetTransition transition in transitions) {
			data += "T;" + transition.Name + "\n";
		}
		foreach(PetriNetArc arc in arcs) {
			data += "A;" + arc.Input.Name + ";" + arc.Output.Name + "\n";
		}

		return data;
	}

	public void Clear () {
		elements.Clear ();
		transitions.Clear ();
		places.Clear ();
		arcs.Clear ();
	}

	public PetriNetElement GetElement (string name) {
		return elements.Find (x => x.Name == name);
	}

	public PetriNetPlace GetPlace (string name) {
		return places.Find (x => x.Name == name);
	}

	public PetriNetTransition GetTransition (string name) {
		return transitions.Find (x => x.Name == name);
	}

	public PetriNetPlace CreatePlace (string name) {
		PetriNetPlace place = new PetriNetPlace (name);
		elements.Add (place);
		places.Add (place);

		return place;
	}

	public void CreateTransition (string name) {
		PetriNetTransition transition = new PetriNetTransition (name);
		elements.Add (transition);
		transitions.Add (transition);
	}

	public bool CreateArc (string input, string output, int weight = 1) {
		// Place => Transition
		PetriNetPlace inputPlace = places.Find (x => x.Name == input);
		if (inputPlace != null) {
			PetriNetTransition outputTransition = transitions.Find (x => x.Name == output);
			if (outputTransition == null) return false;

			PetriNetArc arc = new PetriNetArc (inputPlace, outputTransition, weight);
			arcs.Add (arc);
			return true;
		}

		// Transition => Place
		PetriNetTransition inputTransition = transitions.Find (x => x.Name == input);
		if (inputTransition != null) {
			PetriNetPlace outputPlace = places.Find (x => x.Name == output);
			if (outputPlace == null) return false;

			PetriNetArc arc = new PetriNetArc (inputTransition, outputPlace, weight);
			arcs.Add (arc);
			return true;
		}
		Debug.Log ("3");
		return false;
	}

	public void AddListener (string name, PetriNetTransition.Listener callback) {
		PetriNetTransition transition = transitions.Find (x => x.Name == name);
		if (transition != null) {
			transition.AddListener (callback);
		}
	}

	public void AddMarkers (string name, int amount) {
		PetriNetPlace place = places.Find (x => x.Name == name);
		if (place != null) {
			place.AddMarkers (amount);
			Process ();
		}
	}

	public void Process () {
		foreach (PetriNetTransition transition in transitions) {
			transition.PreProcess ();
		}

		bool atLeastOneProcessed = false;
		foreach (PetriNetTransition transition in transitions) {
			if(transition.Process ()) atLeastOneProcessed = true;
		}

		if(atLeastOneProcessed) this.Process();
	}

}