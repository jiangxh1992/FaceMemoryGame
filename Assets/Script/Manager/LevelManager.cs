using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEditor;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    public int stepMax = 0;
    public int col = 0;
    public int row = 0;
    public ItemBase[] Items;

    public bool allExit = false;
    public Text stepLabel;
    public GameObject WinPanel;


    private List<int> Players = new List<int>();
    private List<int> EmptyExits = new List<int>();
    private int BoardSize = 0;
    private int step = 0;
    private int matchCount = 0;
    private bool isGameOver = false;





    // Use this for initialization
    void Start () {
        GameInit();
	}

    void GameInit()
    {
        isGameOver = false;
        Players.Clear();
        matchCount = 0;
        step = 0;
        stepLabel.text = string.Format("Step:{0}", stepMax);

        InitGameBoard();
    }
	
	// Update is called once per frame
	void Update () {
        if (isGameOver) return;

		if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            Up();
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Left();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Right();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Down();
        }

    }

    public void Left()
    {
        Players.Sort((x, y) => x.CompareTo(y));
        foreach (int player in Players)
        {
            if (player == -1) continue;
            int leftIndex = player - 1;
            if (!isSameRow(leftIndex, player)) continue;
            MoveToPos(player, -1);
        }
        CheckWin();
    }
    public void Right()
    {
        Players.Sort((x, y) => -x.CompareTo(y));
        foreach (int player in Players)
        {
            if (player == -1) continue;
            int rightIndex = player + 1;
            if (!isSameRow(rightIndex, player)) continue;
            MoveToPos(player, 1);
        }
        CheckWin();
    }
    public void Up()
    {
        Players.Sort((x, y) => x.CompareTo(y));
        foreach (int player in Players)
        {
            if (player == -1) continue;
            int upIndex = player - col;
            if (isOverBoard(upIndex)) continue; // 超出边界
            MoveToPos(player, -col);
        }
        CheckWin();
    }
    public void Down()
    {
        Players.Sort((x, y) => -x.CompareTo(y)); // 降序排列players
        foreach (int player in Players)
        {
            if (player == -1) continue;
            int downIndex = player + col;
            if (isOverBoard(downIndex)) continue;
            MoveToPos(player, col);
        }
        CheckWin();
    }

    void CheckWin()
    {
        ++step;
        if (allExit)
        {
            if (matchCount >= Players.Count)
            {
                GameWin();
                foreach (int player in Players)
                {
                    RemovePlayer(player);
                }
            }
            else
            {
                matchCount = 0;
            }
        }
        else
        {
            if (isWin())
            {
                GameWin();
            }
        }

        RefreshGameBoard();
        stepLabel.text = string.Format("Step:{0}", step);

        if (step < 0)
        {
            GameOver();
        }
    }

    [ContextMenu("InitGameBoard")]
    void InitGameBoard()
    {
        BoardSize = transform.childCount;
        Items = new ItemBase[BoardSize];
        for (int i = 0; i < BoardSize; ++i)
        {
            ItemBase item = transform.GetChild(i).GetComponent<ItemBase>();
            Items[i] = item;

            if (item.itemType == JXH.ItemType.Item_Player)
            {
                Players.Add(i);
            }
            else if(item.itemType == JXH.ItemType.Item_EmptyExit)
            {
                EmptyExits.Add(i);
            }
        }
    }

    [ContextMenu("RefreshGameBoard")]
    void RefreshGameBoard()
    {
        for (int i = 0; i < Items.Length; ++i)
        {
            Items[i].SetItemType();
        }
    }

    void MoveToPos(int player, int next)
    {
        if (isOverBoard(player + next)) return;
        // 当前在空出口
        if(isEmptyExit(player))
        {
            SwitchItem(player, player + next);
            Items[player].itemType = JXH.ItemType.Item_EmptyExit;
            return;
        }
        // next是空的
        if (Items[player + next].itemType == JXH.ItemType.Item_Empty)
        {
            SwitchItem(player, player + next);
        }
        // next是墙
        if (Items[player + next].itemType == JXH.ItemType.Item_Wall || Items[player + next].itemType == JXH.ItemType.Item_Player)
        {
        }
        // 漩涡
        if (Items[player + next].itemType == JXH.ItemType.Item_Trap)
        {
            RemovePlayer(player);
            GameOver();
        }
        // 出口
        if (Items[player + next].itemType == JXH.ItemType.Item_Exit || Items[player + next].itemType == JXH.ItemType.Item_EmptyExit)
        {
            if (!allExit)
            {
                RemovePlayer(player);
                Players[GetPlayerIndex(player)] = -1;
            }
            else
            {
                matchCount++; 
                // 移动到空出口
                if(Items[player + next].itemType == JXH.ItemType.Item_EmptyExit)
                {
                    SwitchItem(player, player + next);
                    Items[player].itemType = JXH.ItemType.Item_Empty;
                }
            }
        }
        // 冰面
        if (Items[player + next].itemType == JXH.ItemType.Item_Ice)
        {
            int finalIndex = player + 2 * next;

            bool isValid = true;
            bool isLeftRight = next == -1 || next == 1;
            if (isOverBoard(finalIndex)) isValid = false;
            if (isLeftRight && !isSameRow(finalIndex, player)) isValid = false;
            if(isValid)
            {
                if (Items[finalIndex].itemType == JXH.ItemType.Item_Empty)
                {
                    SwitchItem(player, finalIndex);
                }
            }
        }
    }

    bool isSameRow(int first, int second)
    {
        return (first / col) == (second / col);
    }
    bool isOverBoard(int index)
    {
        return index < 0 || index >= BoardSize; 
    }
    int GetPlayerIndex(int player)
    {
        for(int i=0; i<Players.Count ;++i)
        {
            if(Players[i] == player)
            {
                return i; 
            } 
        }
        return -1;
    }

    void SwitchItem(int first, int second)
    {
        int playerIndex = GetPlayerIndex(first);
        if (playerIndex == -1)
        {
            Debug.Log("出错！玩家不存在！");
            return;
        }

        Players[playerIndex] = second;

        JXH.ItemType firstType = Items[first].itemType;
        JXH.ItemType secondType = Items[second].itemType;

        Items[first].itemType = secondType;
        //Items[first].SetItemType();

        Items[second].itemType = firstType;
        //Items[second].SetItemType();

    }
    bool isWin()
    {
        foreach (int player in Players)
        {
            if (player != -1)
            {
                return false;
            }
        }
        return true;
    }

    bool isAllTaged()
    {
        foreach (int player in Players)
        {
            if (player != -1)
                return false;
        }
        return true;
    }
    void RemovePlayer(int player)
    {
        Items[player].itemType = JXH.ItemType.Item_Empty;
        Items[player].SetItemType();
    }

    bool isEmptyExit(int player)
    {
        return EmptyExits.Contains(player); 
    }

    void GameOver()
    {
        isGameOver = false;
        WinPanel.SetActive(true);
        WinPanel.transform.Find("Bg/Text").GetComponent<Text>().text = "GAME OVER!";
        Debug.Log("game over");
    }

    void GameWin()
    {
        isGameOver = false;
        WinPanel.SetActive(true);
        WinPanel.transform.Find("Bg/Text").GetComponent<Text>().text = "YOU WIN!";
        Debug.Log("game win!");
    }

    public void BackToMenu()
    {
        SceneManager.LoadSceneAsync("levelmap");
    }
}
