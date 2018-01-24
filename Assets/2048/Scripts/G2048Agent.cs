using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace G2048
{

    public class G2048Agent : Agent
    {

        private G2048 g2048;

        public G2048Cell[] cells;

        public override void InitializeAgent()
        {


        }
        public string PrintArray(List<float> arr)
        {
            string ret = "";
            for (int i = 0; i < arr.Count; i++)
            {
                ret += " " + arr[i];
            }
            return ret;
        }

        public override List<float> CollectState()
        {
            List<float> state = new List<float>();
            state.Add(g2048.SIZE_BOARD);
            for (int i = 0; i < g2048.SIZE_BOARD; i++)
            {
                for (int j = 0; j < g2048.SIZE_BOARD; j++)
                {

                    if (g2048.boards[i, j] == -1)
                    {
                        state.Add(0);
                    }
                    else
                    {
                        state.Add(Mathf.Log(g2048.boards[i, j], 2));
                    }

                }
            }
            //Debug.Log(PrintArray(state));
            return state;
        }

        public override void AgentStep(float[] act)
        {
            if (brain.brainType == BrainType.Player)
            {
                if (act[0] > 0 && act[0] < 5)
                {

                    g2048.Move((DIR_MOVE)(act[0] - 1));
                    if (g2048.CheckEndGame())
                    {
                        Debug.Log("MaxValue: " + g2048.GetMaxValue());
                        done = true;

                    }
                    else
                    {

                    }
                }
            }
            else
            {
                var t = ((int)act[0]);

                if (t >= 0 && t < 4)
                {
                    g2048.Move((DIR_MOVE)t);
                    if (g2048.CheckEndGame())
                    {
                        Debug.Log("MaxValue: " + g2048.GetMaxValue());
                        done = true;

                    }
                    else
                    {

                    }
                }


            }

        }

        public override void AgentReset()
        {
            //Debug.Log("Agent reset");

            g2048 = new G2048(this);
            OnUpdateBoard();
        }


        public void OnUpdateBoard()
        {

            for (int i = 0; i < g2048.SIZE_BOARD; i++)
            {
                for (int j = 0; j < g2048.SIZE_BOARD; j++)
                {
                    cells[i * g2048.SIZE_BOARD + j].Updatevalue(g2048.boards[i, j]);
                }
            }
        }

        public override void AgentOnDone()
        {

        }

        public void Move(int action)
        {
            g2048.Move((DIR_MOVE)action);
            if (g2048.CheckEndGame())
            {
                Debug.Log("MaxValue: " + g2048.GetMaxValue());
                done = true;

            }
            else
            {

            }
        }

        public void AddScore(float bonus)
        {
            reward += bonus;
        }

        public void SetReward(float reward)
        {
            this.reward = reward;
        }

    }

}