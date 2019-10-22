using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
	public static T _instance;

	public static T Ins
	{
		get
		{
			return _instance;
		}
	}
	public virtual void Awake()
	{
		_instance = (T)this;
	}
	public virtual void OnDestroy()
	{
		_instance = null;
	}
}
