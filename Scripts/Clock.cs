using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{

	//This script is assigned to the clock prefab.
	//-Requirement 1: The clock prefab must have the "Clock" tag assigned to it.
	//-Requirement 2: The clock prefab needs a Collider in trigger mode, a sphere is recommended.
	//-Attributes that appear in the inspector:
	//string playerTag for the character's tag.
	//-Attributes that do not appear in the inspector:
	//real lifeTime value for the life time of the clock.
	//GameObject labyrinthPiece with read/write access to save the piece of the labyrinth in which the clock is located.
	//GameControl to save the reference of the GameControl component located in the object Control of the hierarchy whose tag is "GameController". 

	//Este script se asigna al prefab del reloj.
	//-Requisito 1: El prefab del reloj debe tener asignado el tag "Clock".
	//-Requisito 2: El prefab del reloj necesita un Collider en modo trigger, se recomienda una esfera.
	//-Atributos que aparecen en el inspector:
	//string playerTag para el tag del personaje.
	//-Atributos que no aparecen en el inspector:
	//valor real lifeTime para el tiempo de vida del reloj.
	//GameObject labyrinthPiece con acceso de lectura/escritura para guardar la pieza del laberinto en la que el reloj se encuentra
	//GameControl para guardar la referencia de la componente GameControl que se encuentra en el objeto Control de la jerarquía cuyo tag es "GameController" 

	[SerializeField]
	private string playerTag;

	private float lifetime;

	public GameObject labyrinthPiece { get; set; }

	private GameControl gameControl;

	void Start()
	{
		//In a single step we find the object Control that has the tag "GameController", we get the reference of its component GameControl and assign it to gameControl.
		//We invoke the selfDestruction method with lifeTime plus a random number between 0 and 1 to prevent all clocks from disappearing in the same frame.

		//En un solo paso encontramos el objeto Control que tiene el tag "GameController", obtenemos la referencia de su componente GameContorl y lo asignamos a gameControl.
		//Invocamos el método selfDestruction con el tiempo lifeTime más un número aleatorio entre 0 y 1 para evitar que todos los relojes desaparezcan en el mismo frame.

		gameControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
		Invoke("selfDestruction", lifetime + Random.value);
	}

	public void setLifetime(float t)
	{
		//Public method to assign the lifetime variable.

		//Método público para asignar el tiempo de vida

		lifetime = t;
	}

	private void selfDestruction()
	{
		//Method to self-destruct the clock.
		//Execute the clockDestroyed method defined in the GameControl script.
		//We destroy this object.

		//Método para autodestruir el reloj.
		//Ejecutamos el método clockDestroyed definido en el script GameControl.
		//Destruimos este objeto.

		gameControl.clockDestroyed(labyrinthPiece);
		Destroy(gameObject);
	}

	public void collect()
	{
		//This method is executed when the character comes into contact with the collider of the clock.
		//We cancel pending invocations.
		//Execute the clockCollected method defined in the GameControl Script.
		//Execute the selfDestruction method.

		//Este método se ejecuta cuando el personaje entra en contacto on el collider del reloj.
		//Cancelamos invocaciones pendientes.
		//Ejecutamos el método clockCollected definido en el Script GameControl.
		//Ejecutamos el método selfDestruction.

		CancelInvoke();
		gameControl.clockCollected();
		selfDestruction();
	}

	void OnTriggerEnter(Collider collider)
	{
		//This method belongs to MonoBehaviour and is executed when a Collider comes into contact with the 
		//Collider in trigger mode assigned to this object (if it has one).
		//Verify if the collider tag is equal to the character tag.
		//If it's true we execute the collect method.

		//Este método pertenece a MonoBehaviour y se ejecuta cuando un Collider entra en contacto con el Collider en modo trigger que tiene asignado
		//este objeto (si es que lo tiene).
		//Verificamos si el tag del collider es el tag del personaje.
		//Si es verdad ejecutamos el método collect.

		if (collider.tag.Equals(playerTag))
		{
			collect();
		}
	}


}
