using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetriNetArc {

	private PetriNetElement input;
	private PetriNetElement output;
	private int weight;

	public int Weight {
		get {
			return weight;
		}
	}

	public PetriNetElement Input {
		get {
			return input;
		}
	}

	public PetriNetElement Output {
		get {
			return output;
		}
	}

	public PetriNetArc (PetriNetElement input, PetriNetElement output, int weight) {
		this.input = input;
		this.output = output;
		this.weight = weight;

		this.input.AddOutput (this);
		this.output.AddInput (this);
	}

}