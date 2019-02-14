using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UICenterOnChild))]
public class GridScrollHelper : MonoBehaviour {

	private UICenterOnChild _centerOnChild;

	public void Start()
	{
		_centerOnChild = gameObject.GetComponent<UICenterOnChild>();
		_centerOnChild.Recenter();
	}

	private void MoveToNextChild(GridNavigationChild obj, bool moveRight)
	{
		if(moveRight)
		{
			if(obj.nextTransform != null)
				_centerOnChild.CenterOn(obj.nextTransform);
		}
		else
		{
			if(obj.previousTransform != null)
				_centerOnChild.CenterOn(obj.previousTransform);
		}
	}

	//Leave @param : currentTransform null if moveLeft, else it will moveRight
	public void MoveToNext(Transform currentTransform)
	{
		GridNavigationChild child = null;

		if(_centerOnChild.centeredObject != null)
			child = _centerOnChild.centeredObject.GetComponent<GridNavigationChild>();

		if(child == null)
		{
			Debug.Log("GridNavigationChild NOT found! Can't navigate!");
			return;
		}

		if(currentTransform == null)
			MoveToNextChild(child, false);
		else 
			MoveToNextChild(child, true);
		
	}
}
