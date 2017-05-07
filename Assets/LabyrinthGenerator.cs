using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 迷路を作成するクラス
public class LabyrinthGenerator : MonoBehaviour
{

    // SerializeField
    [SerializeField]
    private GameObject ground;
    [SerializeField]
    private GameObject wall;

    // private.
    bool[,] board;
    int boardWidth;
    int boardHeight;
    List<Vector2> dir = new List<Vector2> { Vector2.up, Vector2.down, Vector2.right, Vector2.left };

    private void Start()
    {
        CreateLabyrinth(51, 45);
    }

    /// <summary>
    /// 迷路を生成する
    /// ※生成する迷路の高さ幅は必ず奇数にする
    /// </summary>
    /// <param name="width">迷路の幅</param>
    /// <param name="height">迷路の高さ</param>
    public void CreateLabyrinth(int width, int height)
    {
        boardWidth = width;
        boardHeight = height;

        InitializeBoardAndCamera();

        CreateLabyrinthData();

        CreateWall();
    }

    /// <summary>
    /// 迷路の盤面の位置とサイズ、カメラの位置を初期化する
    /// </summary>
    private void InitializeBoardAndCamera()
    {
        float boardPosX = boardWidth / 2f - 0.5f;
        float boardPosZ = boardHeight / 2f - 0.5f;

        ground.transform.localScale = new Vector3(boardWidth, 1, boardHeight);
        ground.transform.position = new Vector3(boardPosX, 0, boardPosZ);

        Camera.main.transform.position = new Vector3(boardPosX, (boardWidth + boardHeight) / 2, boardPosZ);

        board = new bool[boardWidth, boardHeight];

        for (int i = 0; i<boardWidth; i++)
        {
            for (int j = 0; j<boardHeight; j++)
            {
                board[i, j] = true;
            }
        }
    }

    /// <summary>
    /// 迷路データを初期化する
    /// </summary>
    private void CreateLabyrinthData()
    {
        var digCandidate = new List<Vector2>();
        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                if (i != 0 && j != 0 && i % 2 != 0 && j % 2 != 0 && board[i, j])
                {
                    digCandidate.Add(new Vector2(i, j));
                }
            }
        }

        foreach (var vec in digCandidate.OrderBy(v => Guid.NewGuid()))
        {
            if (board[(int)vec.x, (int)vec.y])
            {
                Dig(vec);
            }
        }
    }

    /// <summary>
    /// 迷路を掘る
    /// </summary>
    /// <param name="pos">掘る場所</param>
    private void Dig(Vector2 pos)
    {
        board[(int)pos.x, (int)pos.y] = false;

        var searchDir = dir.OrderBy(i => Guid.NewGuid()).ToList();

        foreach (var d in searchDir)
        {
            var checkPos = pos + d * 2;
            if (IsInBoard(checkPos) && board[(int)checkPos.x, (int)checkPos.y])
            {
                var c = pos + d;
                board[(int)c.x, (int)c.y] = false;
                Dig(checkPos);
            }
        }
    }

    /// <summary>
    /// ある位置が盤面上にあるかどうかを判定して返す
    /// </summary>
    /// <returns><c>true</c> 盤面上にある <c>false</c> 盤面上にない</returns>
    /// <param name="pos">チェックする場所</param>
    private bool IsInBoard(Vector2 pos)
    {
        return pos.x >= 0 && pos.y >= 0 && pos.x < boardWidth && pos.y < boardHeight;
    }

    /// <summary>
    /// 生成された迷路データをもとにオブジェクトを作成する
    /// </summary>
    private void CreateWall()
    {
        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                if (board[i, j])
                {
                    Instantiate(wall, new Vector3(i, 0.5f, j), Quaternion.identity);
                }
            }
        }
    }
}
