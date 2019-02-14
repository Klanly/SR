using UnityEngine;
using System.Collections;

public class POISet
{
	public POISet()
	{
		
	}

	public string id
	{
		get;set;
	}
	
	public string type
	{
		get;set;
	}
	
	public string protectedBy
	{
		get;set;
	}
	
	public string nextLevel
	{
		get;set;
	}

	public bool isCleared
	{
		get;set;
	}

	public POISet Clone()
	{
		POISet POISetClone = new POISet();
		
		POISetClone.id=this.id;
		POISetClone.type=this.type;
		POISetClone.protectedBy=this.protectedBy;
		POISetClone.nextLevel=this.nextLevel;
		POISetClone.isCleared=this.isCleared;
	
		return POISetClone;
	}

	public override string ToString ()
	{
		return string.Format ("[POISet: id={0}, type={1}, protectedBy={2}, nextLevel={3}, isCleared={4}]", id, type, protectedBy, nextLevel, isCleared);
	}
	
}