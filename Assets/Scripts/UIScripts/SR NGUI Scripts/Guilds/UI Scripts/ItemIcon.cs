using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game.UI
{
	public abstract class ItemIcon : MonoBehaviour
	{
		public object item;

		public virtual object Item
		{
			get
			{
				return item;
			}
			set
			{
                if(item != value)
                {
    				item = value;
                    //Debug.Log("VALUE SET"+value);
    				Refresh();
                    gameObject.SetActive(true);
                }
			}
		}

		public abstract void Refresh();
	}
}