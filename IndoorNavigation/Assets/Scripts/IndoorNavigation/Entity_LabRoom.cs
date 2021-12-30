using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_LabRoom : ScriptableObject
{
    public List<Sheet> sheets = new List<Sheet> ();

	[System.SerializableAttribute]
	public class Sheet
	{
		public string name = string.Empty;
		public List<Param> list = new List<Param>();
	}

	[System.SerializableAttribute]
	public class Param
	{
		
		public string ID;
		public string Name;
		public float X;
		public float Y;
	}
}
