using UnityEngine;
using System.Collections;
using System;

public class Nonce : MonoBehaviour {
	static Guid g;
	
	public static string GetUniqueID(){
       	g = Guid.NewGuid();
		

		return g.ToString();
    }
}
