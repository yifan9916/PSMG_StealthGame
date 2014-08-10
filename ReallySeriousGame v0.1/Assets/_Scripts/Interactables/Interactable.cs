﻿using UnityEngine;
using System.Collections;
using iViewX;

public class Interactable : MonoBehaviourWithGazeComponent 
{

	private Color initialColor;
	public Color highlightColor = new Color (0.75f, 1f, 0.75f, 1f);
	public static bool isHighlighted = false;
	
	void OnMouseEnter()
	{
		if(!GameState.IsInteracting && !isHighlighted)
		{
			Highlight();
		}
	}
	
	void OnMouseOver()
	{
		//Unhighlight when interacting with highlighted object
		if(GameState.IsInteracting && isHighlighted)
		{
			UnHighlight();
		}
	}
	
	void OnMouseExit()
	{
		if(isHighlighted)
		{
			UnHighlight();
		}
	}
	
	public override void OnGazeEnter(RaycastHit hit)
	{
	
	}
	
	public override void OnGazeStay(RaycastHit hit)
	{
	
	}
	
	public override void OnGazeExit()
	{
	
	}
	
	void Highlight() 
	{
		initialColor = renderer.material.color;
		renderer.material.color = highlightColor;
		
		isHighlighted = true;
	}
	
	void UnHighlight()
	{
		renderer.material.color = initialColor;
		isHighlighted = false;
	}
}