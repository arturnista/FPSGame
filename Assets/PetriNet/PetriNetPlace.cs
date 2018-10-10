using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetriNetPlace : PetriNetElement {

    protected int markers;

    public int Markers {
        get {
            return markers;
        }
    }

    public PetriNetPlace (string name) : base (name) {
        markers = 0;
    }

    public void RemoveMarker () {
        RemoveMarkers (1);
    }

    public void RemoveMarkers (int amount) {
        AddMarkers (-amount);
    }

    public void AddMarker () {
        AddMarkers (1);
    }

    public void AddMarkers (int amount) {
        markers += amount;
        if (markers < 0) markers = 0;
    }

}