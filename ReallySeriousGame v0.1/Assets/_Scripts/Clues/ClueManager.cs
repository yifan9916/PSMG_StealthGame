﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClueManager : MonoBehaviour 
{
	public static ClueManager instance;
	
	static List<string> foundClues = new List<string>();
	Clue selectedClue;
	bool cluesActivated = false;
	
	void Awake ()
	{
		#region singleton
		if(instance == null) 
		{
			DontDestroyOnLoad(gameObject);
			instance = this;
		} 
		else if(instance != this) 
		{
			Destroy(gameObject);
		}
		#endregion
	}
	
	/// <summary>
	/// Activate all Clue objects on selected GameObject.
	/// </summary>
	/// <param name="selectedObject">Selected GameObject.</param>
	public void ActivateCluesOn(GameObject selectedObject)
	{
		if(!cluesActivated)
		{
			foreach (Transform clue in selectedObject.transform)
			{
				if(clue.tag == "Clue")
				{
					string childClueName = clue.GetComponent<Clue>().clueName;
					if(!foundClues.Contains(childClueName))
						clue.gameObject.SetActive(true);
				}
			}
			cluesActivated = true;
		}
		else
		{
			return;
		}
	}
	
	/// <summary>
	/// Deactivate all Clue objects on selected GameObject.
	/// </summary>
	/// <param name="selectedObject">Selected GameObject.</param>
	public void DeactivateCluesOn(GameObject selectedObject)
	{
		if(cluesActivated)
		{
			foreach (Transform child in selectedObject.transform)
			{
				if(child.tag == "Clue")
					child.gameObject.SetActive(false);
			}
			cluesActivated = false;
		}
		else
		{
			return;
		}
	}
	
	/// <summary>
	/// Sets the selected Clue to discovered and adds it to the found clues list.
	/// </summary>
	/// <param name="newClue">Selected Clue.</param>
	public void FoundClue(GameObject newClue)
	{
		if(newClue.tag == "Clue")
		{
			selectedClue = newClue.GetComponent<Clue>();
			selectedClue.SetDiscovered();
			selectedClue.gameObject.SetActive(false);
			
			if(!foundClues.Contains(selectedClue.clueName))
				foundClues.Add(selectedClue.clueName);
		} 
		else if(newClue.tag == "Interactable")
		{
			foundClues.Add("Barmann Aussage: " + newClue.name);
		}
	}
	
	/// <summary>
	/// Returns all found clues as a List.
	/// </summary>
	/// <returns>List of type Clue.</returns>
	public List<string> GetFoundClues()
	{
		return foundClues;
	}
}