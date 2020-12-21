using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public enum STATUS{
        Black = 0,
        White = 1,
        Empty = 2,
        Draw = 3,
    }
    STATUS IntToStatus(int num)
    {
        switch (num)
        {
            case 0:
                return STATUS.Black;
            case 1:
                return STATUS.White;
            case 2:
                return STATUS.Empty;
        }
        return STATUS.Empty;
    }
    Sprite[] koma;
    [SerializeField]public Text turn_text;
    [SerializeField]public GameObject Original_field;
    SpriteRenderer[][] field = new SpriteRenderer[8][];
    public STATUS[][] field_status = new STATUS[8][];
    // Start is called before the first frame update
    void Start()
    {
        koma = Resources.LoadAll<Sprite>("koma");
        for(int i = 0; i < 8; i++)
        {
            field[i] = new SpriteRenderer[8];
            field_status[i] = new STATUS[8];
            for(int j = 0; j < 8; j++)
            {
                field[i][j] = Instantiate<SpriteRenderer>(Original_field.GetComponent<SpriteRenderer>(), new Vector2(i * 1.2f-4.8f, (7-j) * 1.2f-4f),new Quaternion());
                field[i][j].sprite = koma[2];
                field_status[i][j] = STATUS.Empty;
            }
        }
        field_status[3][4] = STATUS.Black;
        field_status[4][3] = STATUS.Black;
        field_status[3][3] = STATUS.White;
        field_status[4][4] = STATUS.White;
    }
    GameObject clickedGameObject;
    int turn = 0;
    (int x,int y) point = (0, 0);
    (int x,int y) TupleAdd((int x,int y)a,(int x,int y) b)
    {
        return (a.x + b.x, a.y + b.y);
    }
    bool CanPut(int turn,(int x,int y)point)
    {
        if (field_status[point.x][point.y] != STATUS.Empty) return false;
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
    STATUS Winner()
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
                    case STATUS.Black:
                        black++;
                        break;
                    case STATUS.White:
                        white++;
                        break;
                }
            }
        }
        if(!HavePutspace(0) && !HavePutspace(1))
        {
            if (black > white) return STATUS.Black;
            if (black < white) return STATUS.White;
            if (black == white) return STATUS.Draw;
        }
        return STATUS.Empty;
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
            if (field_status[point.x][point.y] == STATUS.Empty) return false;
            if (field_status[point.x][point.y] != IntToStatus(turn)) canput = true;
            if (field_status[point.x][point.y] == IntToStatus(turn)) return canput;
            point = TupleAdd(point, direction);
        }
        return false;
    }
    STATUS result;
    // Update is called once per frame
    void Update()
    {
        if (!HavePutspace(turn))
        {
            turn = (turn + 1) % 2;
        }
        if (IntToStatus(turn) == STATUS.Black)
        {
            turn_text.text = "BLACK";
        }
        else
        {
            turn_text.text = "WHITE";
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(point.y != 0)
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
            if(point.x != 0)
            {
                point.x -= 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CanPut(turn, point))
            {
                field[point.x][point.y].sprite = koma[turn];
                field_status[point.x][point.y] = IntToStatus(turn);
                Flip(turn, point);
                turn = (turn + 1) % 2;
            }
        }
        if((result = Winner()) != STATUS.Empty)
        {
            Debug.Log("winner is "+result);
        }
        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                field[i][j].color = new Color(1, 1, 1);
                if ((i, j) == point)
                {
                    field[i][j].color = new Color(0, 0, 0);
                }
                field[i][j].sprite = koma[(int)field_status[i][j]];
            }
        }
    }
}
