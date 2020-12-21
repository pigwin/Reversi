using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviour
{
    void GameResult()
    {
        switch (Manager.result)
        {
            case Manager.STATUS.Black:
                Debug.Log("Black win");
                break;
            case Manager.STATUS.White:
                Debug.Log("White win");
                break;
            case Manager.STATUS.Draw:
                Debug.Log("Draw");
                break;
        }
        Manager.gameMenu = Manager.GAMETYPE.TITLE;
    }
    // Update is called once per frame
    void Update()
    {
        GameResult();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Title");
        }
    }
}
