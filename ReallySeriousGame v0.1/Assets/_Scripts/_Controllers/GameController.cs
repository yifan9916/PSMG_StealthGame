﻿using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameController : MonoBehaviour 
{
	public static GameController instance;
	
	SoundManager sound;
	
	//private string dataFileName = "/gameprogress.dat";
	
	GameObject hitObject, selectedObject;
	GameObject currentSuspect, lastSuspect;
	GameObject currentInteractable;
	GameObject currentClue;
	
	int currentLevel = -1;
	public float resetSuspectStateTimer = 7f;
	private const int CASE_CLOSED = 6;
	bool caseClosed = false;
	
	void Awake() 
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
		
		sound = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
	}
	
	void Update () 
	{	
		Debug.Log("current interactable: " + GetCurrentInteractable());
		CheckControls();
		CheckGameState();
		ControllBGVolume();
		CheckCaseClosedState();
	}
	
	/// <summary>
	/// Checks GameState and sets Background sound accordingly.
	/// </summary>
	void CheckGameState()
	{
		if(!(currentLevel == Application.loadedLevel))
		{
			switch(Application.loadedLevelName)
			{
			case "MainMenu":
				sound.StopAmbientSound();
				sound.PlayBGMusic(Application.loadedLevelName);
				GameState.ChangeState(GameState.States.MainMenu);
				break;
			case "BarScene":
			case "Scene_2":
				sound.StopBGMusic();
				sound.PlayAmbientSound(Application.loadedLevelName);
				GameState.ChangeState(GameState.States.InGame);
				break;
			default:
				break;
			}
			currentLevel = Application.loadedLevel;
		}
	}
	
	void CheckCaseClosedState()
	{
		if(DialogManager.instance.GetCorrectAccusationsInOrder() == CASE_CLOSED)
		{
			StartCoroutine("WaitForDialogToFinish");
		}
	}
	
	void CaseClosed()
	{
		if(!caseClosed)
		{
			sound.PlayVoice("Sounds/Case_Closed/" + UnityEngine.Random.Range(0,2));
			caseClosed = true;
		}
	}
	
	IEnumerator WaitForDialogToFinish()
	{
		while(GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>().isPlaying || GameObject.FindGameObjectWithTag("Suspect").GetComponent<AudioSource>().isPlaying)
		{
			yield return null;
		}
		CaseClosed();
	}
	
	void CheckControls()
	{
		if(!(Application.loadedLevel == 0))
		{
			if(ControlsOptions.IsKeyboardControls)
			{
				InputController.instance.KeyboardControls();
			}
			else if(ControlsOptions.IsGamepadControls)
			{
				InputController.instance.GamepadControls();
			}
		}
	}
	
	void ControllBGVolume()
	{
		if(GameState.IsInteracting)
		{
			sound.LowerBGVolume();
		}
		else
		{
			sound.DefaultBGVolume();
		}
	}
	
	#region set object references
	public GameObject GetSelectedObject()
	{
		return selectedObject;
	}
	
	//Set selection for mouse interaction
	public void SetSelectedMouseObject()
	{
		/*if(!GameState.IsInteracting)
		{*/
			selectedObject = Mouse.rayTarget().collider.gameObject;
			
			if(selectedObject.tag == "Suspect")
			{
				currentSuspect = selectedObject;
				
				if(GameState.IsState(GameState.States.InGame))
				{
					GameState.ChangeState(GameState.States.Interrogating);
				}
			}
			else if(selectedObject.tag == "Clue")
			{
				currentClue = selectedObject;
			}
			else if(selectedObject.tag == "Interactable")
			{
				currentInteractable = selectedObject;
				
				if(GameState.IsState(GameState.States.InGame))
				{
					GameState.ChangeState(GameState.States.Inspecting);
				}
			}
		//}
	}
	
	//Set selection for gaze interaction
	public void SetSelectedGazeObject(GameObject gazedObject)
	{
		selectedObject = gazedObject;
			
		Debug.Log("selected gaze object: " + selectedObject);
			
		if(selectedObject.tag == "Suspect")
		{
			currentSuspect = selectedObject;
				
			if(GameState.IsState(GameState.States.InGame))
			{
				GameState.ChangeState(GameState.States.Interrogating);
			}
		}
		else if(selectedObject.tag == "Clue")
		{
			currentClue = selectedObject;
		}
		else if(selectedObject.tag == "Interactable")
		{
			currentInteractable = selectedObject;
				
			if(GameState.IsState(GameState.States.InGame))
			{
				GameState.ChangeState(GameState.States.Inspecting);
			}
		}
	}
	
	public void ClearSelections()
	{
		GameState.ChangeState(GameState.States.InGame);
		if(currentSuspect != null)
		{
			lastSuspect = currentSuspect;
		}
		if(lastSuspect != null)
		{
			if(lastSuspect.GetComponent<Suspect>().GetSuspectState() == Suspect.SuspectState.Nervous)
			{
				StartCoroutine("ResetSuspectState", lastSuspect);
			}
		}
		selectedObject = null;
		currentSuspect = null;
		currentClue = null;
		currentInteractable = null;
	}
	
	public void SetCurrentSuspect(GameObject suspect)
	{
		currentSuspect = suspect;
	}
	
	public GameObject GetCurrentSuspect()
	{
		return currentSuspect;
	}
	
	public void SetCurrentClue(GameObject clue)
	{
		currentClue = clue;
	}
	
	public GameObject GetCurrentClue()
	{
		return currentClue;
	}
	
	public void SetCurrentInteractable(GameObject interactable)
	{
		currentInteractable = interactable;
	}
	
	public GameObject GetCurrentInteractable()
	{
		return currentInteractable;
	}
	#endregion
	
	IEnumerator ResetSuspectState(GameObject suspect)
	{
		float timer = 0f;
		while(GameState.IsState(GameState.States.InGame))
		{
			if(timer == resetSuspectStateTimer)
			{
				suspect.GetComponent<Suspect>().SetNeutralState();
			}
			timer++;
			yield return new WaitForSeconds(1f);
		}
	}
	
	/*
	#region save/load player data
	public void Save() 
	{
		BinaryFormatter formatter = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + dataFileName, FileMode.Open);
		
		PlayerData data = new PlayerData();
		data.foundClues = foundClues;
		
		formatter.Serialize(file, data);
		file.Close();
	}
	
	public void Load() 
	{
		if(File.Exists(Application.persistentDataPath + dataFileName))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + dataFileName", FileMode.Open);
			PlayerData data = (PlayerData)formatter.Deserialize(file);
			file.Close();
			
			foundClues = data.foundClues;
		}
    }
    #endregion
    */
}

/*
[Serializable]
class PlayerData 
{
	public int foundClues;
}
*/
