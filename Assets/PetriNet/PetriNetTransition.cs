using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetriNetTransition : PetriNetElement {

    public delegate void Listener ();

    private List<Listener> listeners;

    private bool canProcess;

    public PetriNetTransition (string name) : base (name) {
        listeners = new List<Listener> ();
    }

    public void AddListener (Listener listener) {
        listeners.Add (listener);
    }

    public void PreProcess () {
        canProcess = false;
        if (inputs.Count == 0) return;

        foreach (PetriNetArc inputArc in inputs) {
            PetriNetPlace place = inputArc.Input as PetriNetPlace;
            if (place.Markers < inputArc.Weight) return;
        }
        canProcess = true;
    }

    public bool Process () {
        if (!canProcess) return false;

        foreach (PetriNetArc inputArc in inputs) {
            PetriNetPlace place = inputArc.Input as PetriNetPlace;
            place.RemoveMarkers (inputArc.Weight);
        }

        foreach (PetriNetArc outputArc in outputs) {
            PetriNetPlace place = outputArc.Output as PetriNetPlace;
            place.AddMarkers (outputArc.Weight);
        }

        foreach (Listener list in listeners) list();

        return true;
    }

}