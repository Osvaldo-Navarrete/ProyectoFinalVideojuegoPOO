using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameControl : MonoBehaviour
{

	//Text variable to find the spawn points.
	[SerializeField]
	private string tag;

	//This will save the PREFAB of the character.
	[SerializeField]
	private GameObject playerPrefab;

	//This will save the INSTANCE of the character we place in the hierarchy.
	private GameObject player;

	//In this array will be all the spawn points that we place in the scene.
	[SerializeField]
	private GameObject[] spawnPoints;

	//This will save one of the vector's spawn points.
	[SerializeField]
	private GameObject selectedSpawnPoint;

	//Here we will place the reference of the camera that will be initially active
	[SerializeField]
	private GameObject menuCamera;

	//This will save the reference of the empty GameObject that contains the menuUI elements
	[SerializeField]
	private GameObject menuUI;

	//This will save the reference of the empty GameObject that contains the gameUI elements.
	[SerializeField]
	private GameObject gameUI;

	//This will save the reference of the instance of the Timer script.
	private Timer timer;

	//Video 9

	//The tag of the pieces that can contain the object to find.
	[SerializeField]
	private string spawnPieceTag;

	//The minimum distance between the object to be found and the character.
	[SerializeField]
	private float minDistance;

	//The prefab of the object to find.
	[SerializeField]
	private GameObject objectToFind;

	//The reference of the pieces of the labyrinth that can contain the object to find.
	private GameObject[] labyrinthPieces;

	//The reference of the instance of the object to find.
	private GameObject objectToFindInstance;

	//Video 10

	[SerializeField]
	private GameObject clockPrefab;

	//Number of clocks that will be in the scenario.
	[SerializeField]
	private int nClocks;

	[SerializeField]
	private float clockLifetime;

	[SerializeField]
	private int timePerClock;

	private List<GameObject> piecesWithClocks;



	//Video 12

	[SerializeField]
	private string noSwordMessage;
	[SerializeField]
	private string interactMessage;
	[SerializeField]
	private float minInteractionDistance;

	private Ray ray;
	private UIManager uiManager;
	private bool swordFound;
	private Vector3 screenCenter;

	//Video 13

	private bool scoreScreenActive;

	void Start()
	{
		//*Find the reference of the Timer Script
		//*Enable menuCamera.
		//*Show menuUI.
		//*Hide gameUI.
		//*Find the reference of the UIManager Script
		//*Define a vector (0.5f,0.5f,0.5f)

		timer = gameObject.GetComponent<Timer>();
		menuCamera.SetActive(true);

		uiManager = GetComponent<UIManager>();
		uiManager.showMainMenuUI();

		screenCenter = new Vector3(0.5f, 0.5f, 0f);

	}

	// Update is called once per frame
	void Update()
	{

		//if endScreen=true press any key to end the game.
		//Check Raycast



		if (Input.GetKeyDown(KeyCode.F1))
		{
			endGame();
		}

		if (player != null)
		{
			checkRaycast();
		}

	}

	private void checkRaycast()
	{
		//*Define a Ray from the center of the camera forward.

		ray = Camera.main.ViewportPointToRay(screenCenter);

		//*Defina a RaycastHit.
		RaycastHit hit;

		//*Check Physics.Raycast.
		if (Physics.Raycast(ray, out hit))
		{
			//*If the hit distance is less than the minimum interaction distance

			if (hit.distance < minInteractionDistance)
			{
				//**Try to obtain the Gate and Pedestal components of the GameObject associated with hit.

				Gate gate = hit.transform.gameObject.GetComponent<Gate>();
				Pedestal pedestal = hit.transform.gameObject.GetComponent<Pedestal>();

				if (gate != null)
				{
					//Can interact with gate
					if (swordFound)
					{
						uiManager.showMessage(interactMessage);
					}
					else
					{
						uiManager.showMessage(noSwordMessage);
					}

					if (Input.GetKeyDown(KeyCode.E))
					{
						if (swordFound)
						{
							gameFinished(true);
						}
					}

				}

				if (pedestal != null)
				{
					//Can take the sword
					if (!swordFound)
					{
						uiManager.showMessage(interactMessage);
						if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
						{
							swordFound = true;
							pedestal.removeSword();
						}
					}

				}


			}
		}

	}

	public void startGame()
	{
		//Execute the method startTimer which is defined in the Timer Script.
		timer.startTimer();

		//Disable menuCamera.
		menuCamera.SetActive(false);

		uiManager.showGameUI();

		//We call the placePlayerRandomly method.
		placePlayerRandomly();

		labyrinthPieces = GameObject.FindGameObjectsWithTag(spawnPieceTag);

		//We execute a method that places the object to be found in a random position.
		placeObjectToFind();

		piecesWithClocks = new List<GameObject>();

		placeAllTheClocks();


		swordFound = false;
		scoreScreenActive = false;
	}

	public void gameFinished(bool success)
	{
		//Unlock the cursor
		Cursor.lockState = CursorLockMode.None;
		//Make the cursor visible
		Cursor.visible = true;

		menuCamera.SetActive(true);
		destroyAll();
		timer.stopTimer();
		uiManager.gameFinished(success, timer.getGameTime());

	}


	public void endGame()
	{
		//Execute the method stopTimer which is defined in the Timer Script.
		timer.stopTimer();
		//Enable menuCamera.
		menuCamera.SetActive(true);
		uiManager.showMainMenuUI();



		player.GetComponent<CharacterController>().enabled = true;
		destroyAll();


		//Unlock the cursor
		Cursor.lockState = CursorLockMode.None;
		//Make the cursor visible
		Cursor.visible = true;

	}



	private void placePlayerRandomly()
	{
		//Find all the objects in the hierarchy that have the specified tag and assign it to the spawnPoints vector.
		spawnPoints = GameObject.FindGameObjectsWithTag(tag);
		//Define a random number that can be between 0 and the size of spawnPoints minus 1.
		int rand = Random.Range(0, spawnPoints.Length);
		//Assign the random spawn point to the selectedSpawnPoint GameObject.
		selectedSpawnPoint = spawnPoints[rand];
		//Instantiate the GameObject playerPrefab and keep it in the GameObject player.
		player = Instantiate(playerPrefab, selectedSpawnPoint.transform.position, selectedSpawnPoint.transform.rotation);

	}

	//The following method instantiates the object to be found in one of the pieces that is at least a distance "minDistance" away from the character.
	private void placeObjectToFind()
	{
		//Boolean variable to exit the loop
		bool validSelection = false;
		//Reference of a LabryrinthPiece type object.
		LabyrinthPiece labyrinthPiece = null;
		while (!validSelection)
		{
			//Random number between 0 and LabyrinthPiece.Length -1.
			int i = Random.Range(0, labyrinthPieces.Length);
			//Choose a random object of the array.
			GameObject piece = labyrinthPieces[i];
			if (Vector3.Distance(player.transform.position, piece.transform.position) > minDistance)
			{
				//If the distance between the player and the chosen piece is greater than minDistance then:

				//Get the reference of the LabyrinthPiece object present in piece.
				labyrinthPiece = piece.GetComponent<LabyrinthPiece>();
				//Make true the boolean variable to exit the loop.
				validSelection = true;
			}
		}
		//Then we create an instance of the object to find, using the position that the chosen piece gives us.
		objectToFindInstance = Instantiate(objectToFind, labyrinthPiece.getRandomPosition(), Quaternion.identity);
	}

	//Video 10

	private void placeAllTheClocks()
	{
		//In this script we first make sure that the number of clocks does not exceed the number of labyrinth pieces.
		//Using a for loop we place all the clocks in the labyrinth.

		int nPieces = labyrinthPieces.Length;
		if (nClocks >= nPieces)
		{
			nClocks = nPieces - 1;
		}
		for (int i = 0; i < nClocks; i++)
		{
			placeAClock(nPieces);
		}
	}

	private void placeAClock(int nPieces)
	{
		//It is necessary to use the while loop because we don't know if the randomly chosen piece already contains a clock.
		//Inside the loop we select a piece of the labyrinth at random and then we verify that it is not in the list.
		//If the piece is not in the list we add it to it, then we get the labyrinthPiece reference and in the same step we get a random position.
		//We modify the component of that position with the height defined in clockPrefab
		//We instantiate the clock
		//Obtain the Clock component of the newly created clock.
		//We set to the new clock the piece of the labyrinth in which it is.
		//We set the new clock the life time.
		//We make the loop output condition true.

		bool validSelection = false;
		while (!validSelection)
		{
			int r = Random.Range(0, nPieces);
			GameObject piece = labyrinthPieces[r];
			if (!piecesWithClocks.Contains(piece))
			{
				piecesWithClocks.Add(piece);
				Vector3 rP = piece.GetComponent<LabyrinthPiece>().getRandomPosition();
				rP.y = clockPrefab.transform.position.y;
				GameObject clock = Instantiate(clockPrefab, rP, clockPrefab.transform.rotation);
				Clock clk = clock.GetComponent<Clock>();
				clk.labyrinthPiece = piece;
				clk.setLifetime(clockLifetime);
				validSelection = true;
			}
		}
	}

	public void clockDestroyed(GameObject labPiece)
	{
		//This method is executed when a clock disappears from the stage.
		//We remove the labyrinth piece from the list
		//We place a new clock in the labyrinth.

		piecesWithClocks.Remove(labPiece);
		int nPieces = labyrinthPieces.Length;
		placeAClock(nPieces);
	}

	public void clockCollected()
	{
		//This method is executed when the character takes a clock.
		//We add seconds to the timer.

		timer.addSeconds(timePerClock);
	}

	private void destroyAll()
	{
		//This method is executed at the end of the game and removes everything that needs to be removed from the hierarchy.
		//The character is destroyed.
		//The object to be found is destroyed.
		//All the clocks present on the stage are found.
		//Using a foreach loop, all the clocks on the stage are destroyed.

		Destroy(player);
		Destroy(objectToFindInstance);
		GameObject[] clocks = GameObject.FindGameObjectsWithTag("Clock");

		foreach (GameObject g in clocks)
		{
			Destroy(g);
		}

	}
}