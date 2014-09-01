﻿using UnityEngine;
using System.Collections;

public class VerbalResponse : MonoBehaviour 
{
	private string dir = "Sounds/Dialog/";
	
	private string newVOClipPath;
	
	private AudioClip newVOClip;
	
	public void AccusationVO(string subject)
	{
		Debug.Log("Wasn't me! ");
	}
	
	public void NotLookingVO()
	{
		Debug.Log("Hey I'm here.");
	}
	
	public void RandomVO()
	{
		newVOClipPath = dir + gameObject.name + "_Default_" + Suspect.state + "_" + Random.Range(0,1);
		PlayVO();
	}
	
	public void FixatedVO()
	{
		newVOClipPath = dir + gameObject.name + "_" + Suspect.state + "_" + Random.Range(0,1);
		PlayVO();
	}
	
	public void FixatedOnClueVO(string clueID)
	{
		newVOClipPath = dir + gameObject.name + "_" + Suspect.state + "_" + clueID;
		PlayVO();
	}
	
	public void RandomOnClueVO(string clueID)
	{
		newVOClipPath = dir + gameObject.name + "_" + Suspect.state + "_" + clueID;
		PlayVO();
	}
	
	public void VoiceOverForInteractable(string interactableName)
	{
		newVOClipPath = dir + gameObject.name + "_Positive_" + interactableName;
		PlayVO();
	}
	
	public bool IsSpeaking
	{
		get
		{
			return audio.isPlaying;
		}
	}
	
	void PlayVO()
	{
		if(audio.isPlaying)
		{
			return;
		}
		
		audio.Stop();
		audio.clip = Resources.Load(newVOClipPath, typeof(AudioClip)) as AudioClip;
		audio.Play();
	}
}
