using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Net.Sockets;
using System.Text;

public class Main : MonoBehaviour
{
    [SerializeField]public Text turn_text;
    [SerializeField]public GameObject Original_field;
    SpriteRenderer pointer;
    SpriteRenderer[][] field = new SpriteRenderer[8][];
    Manager.STATUS[][] field_status = new Manager.STATUS[8][];
    int turn = 0;
    (int x,int y) point = (0, 0);
    // Start is called before the first frame update

    (int x,int y) TupleAdd((int x,int y)a,(int x,int y) b)
    {
        return (a.x + b.x, a.y + b.y);
    }
    bool CanPut(int turn,(int x,int y)point)
    {
        if (field_status[point.x][point.y] != Manager.STATUS.Empty) return false;
        if (CheckDirection(turn, point, (-1, -1))) return true;
        if (CheckDirection(turn, point, (0, -1))) return true;
        if (CheckDirection(turn, point, (1, -1))) return true;
        if (CheckDirection(turn, point, (-1, 0))) return true;
        if (CheckDirection(turn, point, (1, 0))) return true;
        if (CheckDirection(turn, point, (-1, 1))) return true;
        if (CheckDirection(turn, point, (0, 1))) return true;
        if (CheckDirection(turn, point, (1, 1))) return true;

        return false;
    }
    bool HavePutspace(int turn)
    {
        int x = field_status.Length;
        int y = field_status[0].Length;
        for(int i = 0; i < x; i++)
        {
            for(int j = 0; j < y; j++)
            {
                if (CanPut(turn, (i, j))) return true;
            }
        }
        return false;
    }
    Manager.STATUS IntToStatus(int num)
    {
        switch (num)
        {
            case 0:
                return Manager.STATUS.Black;
            case 1:
                return Manager.STATUS.White;
            case 2:
                return Manager.STATUS.Empty;
        }
        return Manager.STATUS.Empty;
    }
    Manager.STATUS Winner()
    {
        int x = field_status.Length;
        int y = field_status[0].Length;
        int white = 0, black = 0;
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                switch (field_status[i][j])
                {
                    case Manager.STATUS.Black:
                        black++;
                        break;
                    case Manager.STATUS.White:
                        white++;
                        break;
                }
            }
        }
        if(!HavePutspace(0) && !HavePutspace(1))
        {
            if (black > white) return Manager.STATUS.Black;
            if (black < white) return Manager.STATUS.White;
            if (black == white) return Manager.STATUS.Draw;
        }
        return Manager.STATUS.Empty;
    }
    void Flip(int turn,(int x,int y) point)
    {
        if (CheckDirection(turn, point, (-1, -1)))
        {
            FlipDirection(turn, point, (-1, -1));
        }
        if (CheckDirection(turn, point, (0, -1)))
        {
            FlipDirection(turn, point, (0, -1));
        }
        if (CheckDirection(turn, point, (1, -1)))
        {
            FlipDirection(turn, point, (1, -1));
        }
        if (CheckDirection(turn, point, (-1, 0)))
        {
            FlipDirection(turn, point, (-1, 0));
        }
        if (CheckDirection(turn, point, (1, 0)))
        {
            FlipDirection(turn, point, (1, 0));
        }
        if (CheckDirection(turn, point, (-1, 1)))
        {
            FlipDirection(turn, point, (-1, 1));
        }
        if (CheckDirection(turn, point, (0, 1)))
        {
            FlipDirection(turn, point, (0, 1));
        }
        if (CheckDirection(turn, point, (1, 1)))
        {
            FlipDirection(turn, point, (1, 1));
        }

    }
    void FlipDirection(int turn,(int x,int y) point,(int x,int y) direction)
    {
        int x = field_status.Length;
        int y = field_status[0].Length;
        point = TupleAdd(point, direction);
        while (0 <= point.x && point.x < x && 0 <= point.y && point.y < y)
        {
            if (field_status[point.x][point.y] != IntToStatus(turn))
            {
                field_status[point.x][point.y] = IntToStatus(turn);
            }else if (field_status[point.x][point.y] == IntToStatus(turn)) break;
            point = TupleAdd(point, direction);
        }
    }
    bool CheckDirection(int turn,(int x,int y)point,(int x,int y) direction)
    {
        int x = field_status.Length;
        int y = field_status[0].Length;
        bool canput = false;
        point = TupleAdd(point, direction);
        while(0<=point.x && point.x <x && 0<= point.y && point.y < y)
        {
            if (field_status[point.x][point.y] == Manager.STATUS.Empty) return false;
            if (field_status[point.x][point.y] != IntToStatus(turn)) canput = true;
            if (field_status[point.x][point.y] == IntToStatus(turn)) return canput;
            point = TupleAdd(point, direction);
        }
        return false;
    }
    void LocalControl()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (point.y != 0)
            {
                point.y -= 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (point.y != 7)
            {
                point.y += 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (point.x != 7)
            {
                point.x += 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (point.x != 0)
            {
                point.x -= 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CanPut(turn, point))
            {
                field[point.x][point.y].sprite = Manager.koma[turn];
                field_status[point.x][point.y] = IntToStatus(turn);
                Flip(turn, point);
                turn = (turn + 1) % 2;
                if (Manager.handler[turn] != null)
                {
                    Manager.handler[turn].Send(Encoding.UTF8.GetBytes("put"));
                    Manager.handler[turn].Send(Encoding.UTF8.GetBytes($"{point.x},{point.y}"));
                }
            }
        }
    }
    public void AIControl()
    {
        Manager.handler[turn].Send(Encoding.UTF8.GetBytes("your turn"));
        byte[] bytes = new byte[1024];
        int bytesRec = Manager.handler[turn].Receive(bytes);
        string data = Encoding.UTF8.GetString(bytes, 0, bytesRec);

        Debug.Log(data);
        string[] pointStr = data.Split(',');
        point = (int.Parse(pointStr[0]), int.Parse(pointStr[1]));
        if (CanPut(turn, point))
        {
            field[point.x][point.y].sprite = Manager.koma[turn];
            field_status[point.x][point.y] = IntToStatus(turn);
            Flip(turn, point);
            turn = (turn + 1) % 2;
            if (Manager.handler[turn] != null)
            {
                Manager.handler[turn].Send(Encoding.UTF8.GetBytes("put"));
                Manager.handler[turn].Send(Encoding.UTF8.GetBytes($"{point.x},{point.y}"));
            }
        }
        else
        {
            Manager.handler[turn].Send(Encoding.UTF8.GetBytes("cant"));
        }

    }
    void DrawField()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                field[i][j].color = new Color(1, 1, 1);
                if ((i, j) == point)
                {
                    pointer.transform.position = field[i][j].transform.position;
                }
                field[i][j].sprite = Manager.koma[(int)field_status[i][j]];
            }
        }
        if (IntToStatus(turn) == Manager.STATUS.Black)
        {
            turn_text.text = "BLACK";
        }
        else
        {
            turn_text.text = "WHITE";
        }
    }
    private void Awake()
    {
        Manager.koma = Resources.LoadAll<Sprite>("koma");
        Initial();
        DrawField();
    }
    void GameMain()
    {
        if (!HavePutspace(turn))
        {
            turn = (turn + 1) % 2;
        }
        switch (Manager.player[turn])
        {
            case Manager.PLAYTYPE.UsingHand:
                LocalControl();
                break;
            case Manager.PLAYTYPE.UsingAI:
                AIControl();
                break;
        }
        DrawField();
        if ((Manager.result = Winner()) != Manager.STATUS.Empty)
        {
            Manager.player[0] = Manager.PLAYTYPE.Empty;
            Manager.player[1] = Manager.PLAYTYPE.Empty;
            SceneManager.LoadScene("Result");
        }
    }
    void Initial()
    {
        turn = 0;
        point = (0, 0);
        field = new SpriteRenderer[8][];
        field_status = new Manager.STATUS[8][];
        pointer = new SpriteRenderer();
        pointer = Instantiate(Original_field.GetComponent<SpriteRenderer>(), new Vector2(-4.8f,7*1.2f -4f), new Quaternion());
        pointer.sprite = Manager.koma[2];
        pointer.color = new Color(0.0f, 0.0f, 0.0f, 0.5f);
        pointer.sortingOrder = 2;
        turn = 0;
        for (int i = 0; i < 8; i++)
        {
            field[i] = new SpriteRenderer[8];
            field_status[i] = new Manager.STATUS[8];
            for (int j = 0; j < 8; j++)
            {
                field[i][j] = Instantiate<SpriteRenderer>(Original_field.GetComponent<SpriteRenderer>(), new Vector2(i * 1.2f - 4.8f, (7 - j) * 1.2f - 4f), new Quaternion());
                field[i][j].sprite = Manager.koma[2];
                field_status[i][j] = Manager.STATUS.Empty;
            }
        }
        field_status[3][4] = Manager.STATUS.Black;
        field_status[4][3] = Manager.STATUS.Black;
        field_status[3][3] = Manager.STATUS.White;
        field_status[4][4] = Manager.STATUS.White;
    }
    int waitNum = 0;
    // Update is called once per frame
    void Update()
    {
        if(waitNum == 0)
        {
            waitNum = 1;
        }
        else
        {
            GameMain();
        }
    }
}
