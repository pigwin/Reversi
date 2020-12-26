using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Net;
using System.Net.Sockets;

public class Manager : MonoBehaviour
{
    public enum STATUS
    {
        Black = 0,
        White = 1,
        Empty = 2,
        Draw = 3,
    }
    public enum GAMETYPE
    {
        TITLE,
        INITIAL,
        GAME,
        RESULT,
    }
    public enum PLAYTYPE
    {
        UsingAI,
        UsingHand,
        Empty,
    }
    public static STATUS result;
    public static GAMETYPE gameMenu = GAMETYPE.TITLE;
    public static PLAYTYPE[] player = { PLAYTYPE.Empty, PLAYTYPE.Empty };
    public static Sprite[] koma;
    public static Socket[] handler;
    public static Socket listener;
    private void Start()
    {
        SceneManager.LoadSceneAsync("Title");
    }
}
