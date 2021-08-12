using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

    [SerializeField]
    private int minutes;

    [SerializeField]
    private int seconds;

    private int m, s;


    private GameControl gameControl;

    private UIManager uiManager;

    //timer de partida 

    private int gameTime; 

    // Start is called before the first frame update
    void Start() {
        gameControl = GetComponent<GameControl> ();
        uiManager = GetComponent<UIManager>(); 
    }

    public void startTimer() {

        m = minutes;
        s = seconds;
        gameTime = 0; 


        uiManager.writeTimer(m, s);
        Invoke("updateTimer", 1f);

    }

    public void stopTimer() {
        CancelInvoke ();
    }

    private void updateTimer() {

        s--;
        gameTime++;
        if (s < 0)
        {

            if (m == 0)
            {
                gameControl.gameFinished(false);
                return;

            } else {

                m--;
                s = 59;
            }
        }

       uiManager.writeTimer(m, s);
        Invoke("updateTimer", 1f);
    }

    public void addSeconds(int s){
        this.s += s; 
        if (this.s > 59)
        {
            m++;
            this.s -= 60; 
        }
        uiManager.writeTimer(m, this.s);

    }

    public int getGameTime()
    {
        return gameTime; 
    }
}