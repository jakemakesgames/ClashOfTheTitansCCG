using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour 
{
	[Header("Object Text Information")]
	public string objectName;
	[TextArea]
	public string objectInfo;

	[Header("Display Info")]
	public GameObject toolTipWindow;
	public Text displayName;
	public Text displayInfo;

	private Vector3 pos;

	void Update()
	{
		pos = Input.mousePosition;
		pos.z = 45f;
		pos = Camera.main.ScreenToWorldPoint (pos);
	
	}

	void OnMouseEnter()
	{
		toolTipWindow.SetActive (true);
		transform.position = pos;

		if (toolTipWindow != null) 
		{
			displayName.text = objectName;
			displayInfo.text = objectInfo;
		}
	}

	void OnMouseExit()
	{
		toolTipWindow.SetActive (false);
	}
}
