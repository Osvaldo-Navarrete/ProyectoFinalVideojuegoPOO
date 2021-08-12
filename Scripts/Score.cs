using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
	[SerializeField]
	private string bestScoreName;
	[SerializeField]
	private int bestScoreValue;

	private Timer timer;
	private UIManager uiManager;

	void Start(){
		timer = GetComponent<Timer>();
		bestScoreName = PlayerPrefs.GetString("bestScoreName");
		bestScoreValue = PlayerPrefs.GetInt("bestScoreValue");
	}

	public string getBestScoreName()
	{
		return bestScoreName;
	}
	public int getBestScoreValue()
	{
		return bestScoreValue;
	}

	public void updateBestScore(string bSName)
	{
		bestScoreName = bSName;
		bestScoreValue = timer.getGameTime();
		PlayerPrefs.SetString("bestScoreName",bestScoreName);
		PlayerPrefs.SetInt("bestScoreValue",bestScoreValue);

	}

	public bool isBestScore(int val)
	{
		return (bestScoreValue == 0) || (val < bestScoreValue);
	}

	public void reset()
	{
		bestScoreName = "";
		bestScoreValue = 0;
		PlayerPrefs.SetString("bestScoreName",bestScoreName);
		PlayerPrefs.SetInt("bestScoreValue",bestScoreValue);
	}

}
