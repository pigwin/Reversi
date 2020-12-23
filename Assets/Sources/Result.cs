using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text;

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
    private void Awake()
    {
        GameResult();
        if(Manager.handler[0] != null)
        {
            switch (Manager.result)
            {
                case Manager.STATUS.Black:
                    Manager.handler[0].Send(Encoding.UTF8.GetBytes("YouWin"));
                    break;
                case Manager.STATUS.White:
                    Manager.handler[0].Send(Encoding.UTF8.GetBytes("YouLose"));
                    break;
                case Manager.STATUS.Draw:
                    Manager.handler[0].Send(Encoding.UTF8.GetBytes("Draw"));
                    break;
            }
            Manager.handler[0].Close();
        }
        if (Manager.handler[1] != null)
        {
            switch (Manager.result)
            {
                case Manager.STATUS.White:
                    Manager.handler[1].Send(Encoding.UTF8.GetBytes("YouWin"));
                    break;
                case Manager.STATUS.Black:
                    Manager.handler[1].Send(Encoding.UTF8.GetBytes("YouLose"));
                    break;
                case Manager.STATUS.Draw:
                    Manager.handler[1].Send(Encoding.UTF8.GetBytes("Draw"));
                    break;
            }
            Manager.handler[1].Close();
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Manager.player[0] = Manager.PLAYTYPE.Empty;
            Manager.player[1] = Manager.PLAYTYPE.Empty;
            SceneManager.LoadScene("Title");
        }
    }
}
