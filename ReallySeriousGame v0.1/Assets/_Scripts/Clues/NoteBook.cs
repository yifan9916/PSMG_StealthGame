﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NoteBook : MonoBehaviour 
{
	ClueManager clue;
	
	public GUIStyle notebookStyle;
	public Texture notebookTexture;
	
	private Rect notebook;
	private float notebookX;
	private float notebookY;
	private float notebookWidth		= Screen.width / 3;
	private float notebookHeight 	= Screen.height * 0.7f;
	private float offset			= 10;
	
	private Rect note;
	private float noteHeight;
	private float noteWidth;
	
	private List<string> notes;
	
	private const int NOTEBOOK_ID = 0;
	private string notebookHeader = "NOTES";
	private bool isToggled = false;
	
	void Awake()
	{
		clue = GameObject.FindGameObjectWithTag("ClueManager").GetComponent<ClueManager>();
		
		notebookX 	= Screen.width - notebookWidth - offset;
		notebookY 	= Screen.height - notebookHeight;
		notebook 	= new Rect(notebookX, notebookY, notebookWidth, notebookHeight);
		
		noteHeight 	= notebookHeight * 0.1f;
		noteWidth 	= notebookWidth - (offset * 2);
	}
	
	void OnGUI()
	{
		if(isToggled)
		{
			notes = clue.GetFoundClues();
			notebook = GUI.Window(NOTEBOOK_ID, notebook, DisplayFoundClues, notebookHeader);
		}
	}
	
	void DisplayFoundClues(int ID)
	{
		for(int i = 0; i < notes.Count; i++)
		{
			if(GUI.Button(new Rect(offset, (offset * 2) + (noteHeight * i), noteWidth, noteHeight), notes[i]))
				Debug.Log("stuff");
		}
	}
	
	public void ToggleNotebook()
	{
		isToggled = !isToggled;
	}
}
