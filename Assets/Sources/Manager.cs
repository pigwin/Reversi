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
    public async static void SocketServer()
    {
        byte[] bytes = new byte[1024];
        IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress ipAddress = ipHostInfo.AddressList[3];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);
        Debug.Log(ipAddress);
        listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        listener.Bind(localEndPoint);
        listener.Listen(2);
        handler = new Socket[2];
        handler[0] = await listener.AcceptAsync();
        handler[1] = await listener.AcceptAsync();
    }
    private void Start()
    {
        SocketServer();
        SceneManager.LoadScene("Title");
    }
}
