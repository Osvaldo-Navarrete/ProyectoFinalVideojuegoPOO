using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

	[Header("Interfaces")]
	[Space(10)]

	[SerializeField]
	private GameObject mainMenuUI;
	[SerializeField]
	private GameObject gameUI;
	[SerializeField]
	private GameObject scoreUI;

	[Header("Game UI")]
	[Space(10)]

	//Here we will save a reference of the text element of the Canvas.
	[SerializeField]
	private Text timerText;

	[SerializeField]
	private GameObject messageGameObject;


	//Aqui guardaremos una referencia del elemento texto del Canvas.
	[SerializeField]
	private Text messageText;

	//Video 13

	[Header("ScoreUI")]
	[Space(10)]

	[SerializeField]
	private Text textResult;
	[SerializeField]
	private string stringCongratulations;
	[SerializeField]
	private string stringTryAgain;
	[SerializeField]
	private GameObject objectYourTime;
	[SerializeField]
	private Text textYourTimeValue;
	[SerializeField]
	private GameObject objectNewBestScore;
	[SerializeField]
	private InputField inputName;
	[SerializeField]
	private Text textNewBestScoreValue;
	[SerializeField]
	private GameObject objectCurrentBestScore;
	[SerializeField]
	private Text textCurrentBestScoreName;
	[SerializeField]
	private Text textCurrentBestScoreValue;

	private Score score;


	void Start()
	{

		messageGameObject.SetActive(false);

		score = GetComponent<Score>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		clearMessage();
	}

	public void showMessage(string message)
	{
		messageText.text = message;
		messageGameObject.SetActive(true);
	}

	private void clearMessage()
	{
		messageGameObject.SetActive(false);
	}


	//The next method formats the time values and writes them to the Canvas element.
	//Unlike the methods we have written before, this one will receive two parameters: the integers m and s,
	//which will be used within the method of doing operations.
	//Defining the method in this way is very interesting because it raises the following question:
	//When we use m and s within the writeTimer() method. Are we using the m and s defined in the class itself or are we using the parameters we received at the time of the call?
	//When we study programming we're going to come back to this.

	//El siguiente método se encarga de dar formato a los valores de tiempo y escribirlos en el elemento del Canvas.
	//A diferencia de los métodos que hemos escrito antes, este recibirá dos parámetros: los enteros m y s,
	//los cuales se utilizarán dentro del método para hacer operaciones.
	//Definir el método de esta forma es muy interesante porque nos plantea la siguiente interrogante:
	//Cuando utilizo m y s dentro del método writeTimer(). ¿Estoy usando el m y s definido en la propia clase o estoy usando los parámetros que recibi al momento de la llamada?
	//Cuando estudiemos programación vamos a volver sobre esto.

	public void writeTimer(int m, int s)
	{
		//If this is true it means that the second variable has a single digit.
		//In this case we must concatenate a 0 to the left of the seconds to keep the format,
		//otherwise we could visualize the time for example in this way: 1:6 (indicating 1 minute 6 seconds).

		//Si esto se cumple significa que la variable segundos tiene un solo dígito.
		//En este caso debemos concatenar un 0 a la izquierda de los segundos para conservar el formato,
		//de lo contrario podríamos visualizar el tiempo por ejemplo de esta manera: 1:6 (indicando 1 minuto 6 segundos).

		if (s < 10)
		{
			timerText.text = m.ToString() + ":0" + s.ToString();
		}
		else
		{
			//In this case the seconds variable has two digits, therefore we don't concatenate the 0 to the left.

			//En este caso la variable segundos tiene 2 dígitos, por lo tanto no se concatena un 0 a la izquierda.

			timerText.text = m.ToString() + ":" + s.ToString();
		}


	}

	public void showMainMenuUI()
	{
		clearUI();
		mainMenuUI.SetActive(true);
	}

	public void showGameUI()
	{
		clearUI();
		gameUI.SetActive(true);
	}

	public void showScoreUI()
	{
		clearUI();
		scoreUI.SetActive(true);
	}

	private void clearUI()
	{
		mainMenuUI.SetActive(false);
		gameUI.SetActive(false);
		scoreUI.SetActive(false);
	}

	private void clearScoreUI()
	{
		objectNewBestScore.SetActive(false);
		objectCurrentBestScore.SetActive(false);
		objectYourTime.SetActive(false);
	}

	public void gameFinished(bool success, int time)
	{
		clearScoreUI();
		if (success)
		{
			gameWon(time);
		}
		else
		{
			gameLost();
		}

		int bestScore = score.getBestScoreValue();
		if (bestScore != 0)
		{
			objectCurrentBestScore.SetActive(true);

			textCurrentBestScoreName.text = score.getBestScoreName();
			textCurrentBestScoreValue.text = bestScore.ToString();
		}
		showScoreUI();

	}

	private void gameWon(int time)
	{
		textResult.text = stringCongratulations;
		objectYourTime.SetActive(true);
		textYourTimeValue.text = time.ToString();
		if (score.isBestScore(time))
		{
			inputName.enabled = true;
			inputName.text = "";
			textNewBestScoreValue.text = time.ToString();
			objectNewBestScore.SetActive(true);
		}

	}

	private void gameLost()
	{
		textResult.text = stringTryAgain;
	}

	public void updateScore()
	{
		objectYourTime.SetActive(false);
		score.updateBestScore(inputName.text);
		inputName.enabled = false;
		inputName.text = "";
		refreshBestScoreText();

	}

	public void refreshBestScoreText()
	{
		int bestScore = score.getBestScoreValue();
		if (bestScore != 0)
		{
			objectCurrentBestScore.SetActive(true);
			textCurrentBestScoreName.text = score.getBestScoreName();
			textCurrentBestScoreValue.text = bestScore.ToString();
		}
		else
		{
			objectCurrentBestScore.SetActive(false);
		}
		objectNewBestScore.SetActive(false);

	}

	public void resetBestScore()
	{
		objectCurrentBestScore.SetActive(false);
		score.reset();

	}




}
