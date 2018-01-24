using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace G2048
{
    public enum DIR_MOVE
    {
        UP, DOWN, LEFT, RIGHT
    }

    public class G2048
    {


        public int[,] boards;

        public int SIZE_BOARD = 4;

        public int maxValue = -1;

        public G2048Agent agent;

        public G2048(G2048Agent agent)
        {

            this.agent = agent;
            boards = new int[SIZE_BOARD, SIZE_BOARD];
            for (int i = 0; i < SIZE_BOARD; i++)
            {
                for (int j = 0; j < SIZE_BOARD; j++)
                {
                    boards[i, j] = -1;
                }
            }
            EndMove(true);

        }

        public void Move(DIR_MOVE dir)
        {
            if (dir == DIR_MOVE.DOWN)
            {
                MoveDown();
            }
            else if (dir == DIR_MOVE.LEFT)
            {
                MoveLeft();
            }
            else if (dir == DIR_MOVE.RIGHT)
            {
                MoveRight();
            }
            else if (dir == DIR_MOVE.UP)
            {
                MoveUp();
            }
        }

        public void MoveLeft()
        {
            //Debug.Log("MoveRight");
            // PrintBoard();


            // endcanmove
            bool isUpdateMove = false;
            for (int i = 0; i < SIZE_BOARD; i++)
            {

                int[] arr = new int[SIZE_BOARD];
                for (int k = 0; k < SIZE_BOARD; k++)
                {
                    arr[k] = boards[i, k];
                }

                var newCol = UpdateArray(arr);
                for (int j = 0; j < SIZE_BOARD; j++)
                {
                    if (boards[i, j] != newCol[j])
                    {
                        isUpdateMove = true;
                    }
                    boards[i, j] = newCol[j];
                }
            }
            if (isUpdateMove)
            {
                EndMove();
            }
            else
            {
                //agent.AddScore(-0.01f);
            }


        }

        public void MoveRight()
        {
            //Debug.Log("MoveLeft");
            // PrintBoard();
            bool isUpdateMove = false;
            for (int i = 0; i < 4; i++)
            {

                int[] arr = new int[SIZE_BOARD];
                for (int k = 0; k < SIZE_BOARD; k++)
                {
                    arr[k] = boards[i, SIZE_BOARD - 1 - k];
                }

                var newCol = UpdateArray(arr);
                for (int j = SIZE_BOARD - 1; j >= 0; j--)
                {
                    if (boards[i, j] != newCol[SIZE_BOARD - 1 - j])
                    {
                        isUpdateMove = true;
                    }
                    boards[i, j] = newCol[SIZE_BOARD - 1 - j];
                }
            }
            if (isUpdateMove)
            {
                EndMove();
            }
            else
            {
                //agent.AddScore(-0.01f);
            }
        }

        public void MoveUp()
        {
            //Debug.Log("MoveUp");
            //PrintBoard();
            bool isUpdateMove = false;
            for (int j = 0; j < 4; j++)
            {

                int[] arr = new int[SIZE_BOARD];
                for (int k = 0; k < SIZE_BOARD; k++)
                {
                    arr[k] = boards[k, j];
                }

                var newRow = UpdateArray(arr);
                for (int i = 0; i < SIZE_BOARD; i++)
                {
                    if (boards[i, j] != newRow[i])
                        isUpdateMove = true;
                    boards[i, j] = newRow[i];
                }
            }
            if (isUpdateMove)
            {
                EndMove();
            }
            else
            {
                //agent.AddScore(-0.01f);
            }
        }

        public void MoveDown()
        {
            //Debug.Log("MoveDown");
            //PrintBoard();
            bool isUpdateMove = false;
            for (int j = 0; j < 4; j++)
            {

                int[] arr = new int[SIZE_BOARD];
                for (int k = 0; k < SIZE_BOARD; k++)
                {
                    arr[k] = boards[SIZE_BOARD - 1 - k, j];
                }

                var newRow = UpdateArray(arr);
                for (int i = 0; i < SIZE_BOARD; i++)
                {
                    if (boards[i, j] != newRow[SIZE_BOARD - 1 - i])
                        isUpdateMove = true;
                    boards[i, j] = newRow[SIZE_BOARD - 1 - i];
                }
            }
            if (isUpdateMove)
            {
                EndMove();
            }
            else
            {
                //agent.AddScore(-0.01f);
            }
        }

        public void EndMove(bool isNotUpdateVisualBoard = false)
        {
            List<Vector2Int> freeCells = new List<Vector2Int>();
            for (int i = 0; i < SIZE_BOARD; i++)
            {
                for (int j = 0; j < SIZE_BOARD; j++)
                {
                    if (boards[i, j] == -1)
                    {
                        freeCells.Add(new Vector2Int(i, j));
                    }

                }
            }

            if (freeCells.Count > 0)
            {
                int pickCell = UnityEngine.Random.Range(0, freeCells.Count);
                {

                    boards[freeCells[pickCell].x, freeCells[pickCell].y] = UnityEngine.Random.Range(0, 2) == 1 ? 2 : 4;
                }
            }

            for (int i = 0; i < SIZE_BOARD; i++)
            {
                for (int j = 0; j < SIZE_BOARD; j++)
                {

                    if (boards[i, j] > maxValue)
                    {
                        maxValue = boards[i, j];
                    }
                }
            }
           //  PrintBoard();
            if (!isNotUpdateVisualBoard)
                agent.OnUpdateBoard();
        }

        public float GetMaxValue()
        {
            if (maxValue == -1)
            {
                return -1;
            }
            else
                return maxValue;
        }


        public bool CheckEndGame()
        {
            var ret = true;

            for (int i = 0; i < SIZE_BOARD && ret; i++)
            {
                for (int j = 0; j < SIZE_BOARD && ret; j++)
                {
                    if (CheckSuroundCell(i, j))
                    {
                        ret = false;
                    }
                }
            }
            return ret;
        }

        public bool CheckSuroundCell(int i, int j)
        {
            if (i > 0 && (boards[i - 1, j] == -1 || boards[i - 1, j] == boards[i, j]))
            {
                return true;
            }

            if (i < 3 && (boards[i + 1, j] == -1 || boards[i + 1, j] == boards[i, j]))
            {
                return true;
            }

            if (j > 0 && (boards[i, j - 1] == -1 || boards[i, j - 1] == boards[i, j]))
            {
                return true;
            }

            if (j < 3 && (boards[i, j + 1] == -1 || boards[i, j + 1] == boards[i, j]))
            {
                return true;
            }

            return false;
        }



        public List<int> UpdateArray(int[] arr)
        {

            List<int> ret = new List<int>();

            int i = 0;
            for (; i < arr.Length;)
            {
                if (arr[i] == -1)
                {
                    i += 1;
                }
                else if (i == arr.Length - 1)
                {
                    ret.Add(arr[i]);
                    i++;
                }
                else
                {
                    int k = i + 1;
                    for (; k < arr.Length; k++)
                    {
                        if (arr[k] != -1)
                        {
                            break;
                        }
                    }

                    if (k < arr.Length)
                    {


                        if (arr[k] == arr[i])
                        {
                            ret.Add(arr[i] * 2);
                            agent.AddScore(Mathf.Log(maxValue, 2) / 1000);
                            i = k + 1;
                        }
                        else
                        {
                            ret.Add(arr[i]);
                            i = k;
                        }


                    }
                    else
                    {

                        ret.Add(arr[i]);
                        i = k + 1;

                    }
                }
            }
            for (int j = ret.Count; j < SIZE_BOARD; j++)
            {
                ret.Add(-1);
            }
            // Debug.Log("------Transfer------ \n" + PrintArray(arr.ToList()) + "\n" + PrintArray(ret));
            //  PrintArray(arr.ToList());
            //  PrintArray(ret);
            return ret;
        }

        public string PrintArray(List<int> arr)
        {
            string ret = "";
            for (int i = 0; i < arr.Count; i++)
            {
                ret += " " + arr[i];
            }
            return ret;
        }

        public void PrintBoard()
        {
            string ret = "";
            for (int i = 0; i < SIZE_BOARD; i++)
            {
                for (int j = 0; j < SIZE_BOARD; j++)
                {
                    ret += " " + boards[i, j];
                }
                ret += "\n";
            }
            Debug.Log("--------Board----------\n" + ret);

        }

        public List<DIR_MOVE> GetDirectionCanMove()
        {
            List<DIR_MOVE> ret = new List<DIR_MOVE>();
            if (CheckCanMoveDown())
            {
                ret.Add(DIR_MOVE.DOWN);
            }
            if (CheckCanMoveLeft())
            {
                ret.Add(DIR_MOVE.LEFT);
            }
            if (CheckCanMoveUp())
            {
                ret.Add(DIR_MOVE.UP);

            }
            if (CheckCanMoveRight())
            {
                ret.Add(DIR_MOVE.RIGHT);
            }
            return ret;

        }

        public bool CheckCanMoveLeft()
        {
            var ret = false;
            for (int i = 0; i < 4; i++)
            {
                bool canMoveRow = false;
                bool isExitsDiffOneMinus = false;
                for (int j = 3; j > -1; j--)
                {
                    if (boards[i, j] == -1)
                    {
                        if (isExitsDiffOneMinus)
                        {
                            canMoveRow = true;
                            break;
                        }

                    }
                    else
                    {
                        if (!isExitsDiffOneMinus)
                        {
                            isExitsDiffOneMinus = true;
                        }
                        else
                        {
                            if (boards[i, j] == boards[i, j + 1])
                            {
                                canMoveRow = true;
                                break;
                            }
                        }
                    }
                }
                if (canMoveRow)
                {
                    //  Debug.Log("Can move left at row: " + i);
                    ret = true;
                    break;
                }
            }
            return ret;
        }

        public bool CheckCanMoveRight()
        {
            var ret = false;
            for (int i = 0; i < 4; i++)
            {
                bool canMoveRow = false;
                bool isExitsDiffOneMinus = false;
                for (int j = 0; j < 4; j++)
                {
                    if (boards[i, j] == -1)
                    {
                        if (isExitsDiffOneMinus)
                        {
                            canMoveRow = true;
                            break;
                        }

                    }
                    else
                    {
                        if (!isExitsDiffOneMinus)
                        {
                            isExitsDiffOneMinus = true;
                        }
                        else
                        {
                            if (boards[i, j] == boards[i, j - 1])
                            {
                                canMoveRow = true;
                                break;
                            }
                        }
                    }
                }
                if (canMoveRow)
                {
                    //  Debug.Log("Can move right at row: " + i);
                    ret = true;
                    break;
                }
            }
            return ret;
        }

        public bool CheckCanMoveUp()
        {
            var ret = false;
            for (int j = 0; j < 4; j++)
            {
                bool canMoveRow = false;
                bool isExitsDiffOneMinus = false;
                for (int i = 3; i > 0; i--)
                {
                    if (boards[i, j] == -1)
                    {
                        if (isExitsDiffOneMinus)
                        {
                            canMoveRow = true;
                            break;
                        }

                    }
                    else
                    {
                        if (!isExitsDiffOneMinus)
                        {
                            isExitsDiffOneMinus = true;
                        }
                        else
                        {
                            if (boards[i, j] == boards[i + 1, j])
                            {
                                canMoveRow = true;
                                break;
                            }
                        }
                    }
                }
                if (canMoveRow)
                {
                    // Debug.Log("Can move up at col: " + j);
                    ret = true;
                    break;
                }
            }
            return ret;
        }

        public bool CheckCanMoveDown()
        {
            var ret = false;
            for (int j = 0; j < 4; j++)
            {
                bool canMoveRow = false;
                bool isExitsDiffOneMinus = false;
                for (int i = 0; i < 4; i++)
                {
                    if (boards[i, j] == -1)
                    {
                        if (isExitsDiffOneMinus)
                        {
                            canMoveRow = true;
                            break;
                        }

                    }
                    else
                    {
                        if (!isExitsDiffOneMinus)
                        {
                            isExitsDiffOneMinus = true;
                        }
                        else
                        {
                            if (boards[i, j] == boards[i - 1, j])
                            {
                                canMoveRow = true;
                                break;
                            }
                        }
                    }
                }
                if (canMoveRow)
                {
                    //  Debug.Log("Can move down at col: " + j);
                    ret = true;
                    break;
                }
            }
            return ret;
        }



    }

}
