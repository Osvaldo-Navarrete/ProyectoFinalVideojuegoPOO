using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthPiece : MonoBehaviour
{

	[SerializeField]
	private GameObject pointA1;
	[SerializeField]
	private GameObject pointA2;
	[SerializeField]
	private GameObject pointB1;
	[SerializeField]
	private GameObject pointB2;

	public Vector3 getRandomPosition()
	{
		//This method returns a random position of the inner area of the labyrinth piece, 
		//every labyrinth piece has its own particular region defined by A1, A2, B1 and B2
		//Este método devuelve una posición aleatoria de la región interna de una pieza del laberinto, 
		//cada pieza del laberinto tiene su propia región particular definida por los puntos A1, A2, B1 y B2.


		Vector3 position;
		if (Random.value < 0.5f)
		{
			//Linear interpolation between A1 amd A2 positions.
			position = Vector3.Lerp(pointA1.transform.position, pointA2.transform.position, Random.Range(0f, 1f));
		}
		else
		{
			//Linear interpolation between B1 amd B2 positions.
			position = Vector3.Lerp(pointB1.transform.position, pointB2.transform.position, Random.Range(0f, 1f));
		}
		return position;
	}
}
