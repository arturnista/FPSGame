using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PetriNetController : MonoBehaviour {
	
	public static PetriNetController main;
	public PetriNet petriNet;

	private HUDController m_HUDController;

	void Awake () {
		m_HUDController = GameObject.FindObjectOfType<HUDController>();
		main = this;

		petriNet = new PetriNet();

		petriNet.CreatePlace("big_door_01");
		petriNet.CreatePlace("big_door_01_is_open");
		petriNet.CreateTransition("open_big_door_01");
		petriNet.CreateArc("big_door_01", "open_big_door_01");
		petriNet.CreateArc("open_big_door_01", "big_door_01_is_open");

		petriNet.CreatePlace("big_door_02");
		petriNet.CreatePlace("big_door_02_is_open");
		petriNet.CreateTransition("open_big_door_02");
		petriNet.CreateArc("big_door_01_is_open", "open_big_door_02");
		petriNet.CreateArc("big_door_02", "open_big_door_02");
		petriNet.CreateArc("open_big_door_02", "big_door_02_is_open");

		petriNet.CreatePlace("enemies");
		petriNet.CreateTransition("heli_available");
		petriNet.CreateArc("big_door_02_is_open", "heli_available");
		petriNet.CreateArc("enemies", "heli_available", 3);
		petriNet.CreatePlace("boss");

		petriNet.CreateArc("heli_available", "boss");
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.I)) SaveFile();
		else if(Input.GetKeyDown(KeyCode.O)) LoadFile();
	}

	void SaveFile() {
        string path = "./petri_net.txt";

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine(petriNet);
        writer.Close();

		m_HUDController.ShowText("Petri net SAVED with success!");
	}

	void LoadFile() {
        string path = "./petri_net.txt";

        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path); 
		petriNet = new PetriNet(reader.ReadToEnd());
        reader.Close();

		m_HUDController.ShowText("Petri net LOADED with success!");
	}
}
