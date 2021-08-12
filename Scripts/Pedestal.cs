using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestal : MonoBehaviour
{

	// Use this for initialization
	[SerializeField]
	private GameObject sword;
	private AudioSource audioSource;

	void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	public void removeSword()
	{
		sword.SetActive(false);
		audioSource.Play();
	}


}
