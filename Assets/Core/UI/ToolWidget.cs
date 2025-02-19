﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class ToolWidget : MonoBehaviour {

	public Sprite ToolIcon;

	public Sprite iconLeftControllerLeft;
	public Sprite iconLeftControllerRight;
	public Sprite iconLeftControllerUp;
	public Sprite iconLeftControllerDown;
	public Sprite iconRightControllerLeft;
	public Sprite iconRightControllerRight;
	public Sprite iconRightControllerUp;
	public Sprite iconRightControllerDown;

	/*! Lets you handle when a tool should be available */
	public enum ToolDisplayTime {
		WhenPatientIsLoaded,
		WhenNoPatientIsLoaded,
		Always,
		Never
	};

	public ToolDisplayTime displayTime = ToolDisplayTime.WhenPatientIsLoaded;

	private bool isDisabled = false;

	// Use this for initialization
	void Start () {

		// Set material for all texts:
		Material mat = new Material (Shader.Find ("Custom/TextShader"));
		mat.renderQueue += 2;	// overlay!
		Component[] texts;
		texts = GetComponentsInChildren (typeof(Text), true);

		if (texts != null) {
			foreach (Text t in texts)
				t.material = mat;
		}

		Material material = new Material (Shader.Find ("Custom/UIObject"));
		material.renderQueue += 2;	// overlay!
		Component[] images;
		images = GetComponentsInChildren (typeof(Image), true);

		if (images != null) {
			foreach (Image i in images) {
				if (i.material.name == "Default UI Material")
					i.material = material;
			}
		}
	}

	public void OnEnable()
	{
		// Move the object to the current anchor (to "helmet" or to controller)
		Invoke ("MoveToUIAnchor",0.0001f);

		InputDeviceManager.instance.setLeftControllerTouchpadIcons (
			iconLeftControllerLeft, iconLeftControllerRight, iconLeftControllerUp, iconLeftControllerDown);

		InputDeviceManager.instance.setRightControllerTouchpadIcons (
			iconRightControllerLeft, iconRightControllerRight, iconRightControllerUp, iconRightControllerDown);
	}

	public void OnDisable()
	{
		// Move the object back to the toolControl:
		Invoke ("MoveBackToToolControl",0.0001f);
	}

	private void MoveToUIAnchor()
	{
		if( ToolUIAnchor.instance != null )
			transform.SetParent (ToolUIAnchor.instance.transform, false);
	}
	private void MoveBackToToolControl()
	{
		transform.SetParent (ToolControl.instance.transform, false);
	}

	public void MoveToBackground()
	{
		if (isDisabled)
			return;
		CanvasGroup canvasGroup = transform.GetComponent<CanvasGroup> ();
		canvasGroup.alpha = 0.2f;
		canvasGroup.interactable = false;
		transform.Translate (0f, -0.04f, 0f);
		isDisabled = true;
	}

	public void MoveToForeground()
	{
		if (!isDisabled)
			return;
		CanvasGroup canvasGroup = transform.GetComponent<CanvasGroup> ();
		canvasGroup.alpha = 1f;
		canvasGroup.interactable = true;
		transform.Translate (0f, 0.04f, 0f);
		isDisabled = false;
	}
}
