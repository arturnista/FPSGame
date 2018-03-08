using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayloadGyratoryActivate : Interactive {

	public PayloadGyratory giratory;
	
	public override void Use() {
		base.Use();
		if(giratory) giratory.Activate();
	}
}
