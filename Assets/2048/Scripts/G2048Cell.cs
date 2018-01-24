using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
namespace G2048
{
    public class G2048Cell : MonoBehaviour
    {
        public Image image;
        public Text text;

        private void Start()
        {
            text.color = Color.black;
        }

        public void Updatevalue(int value)
        {
            if (value == -1)
            {
                text.text = "";
                image.color = G2048ColorManager.instance.defaultColor;
            }
            else
            {
                text.text = value.ToString();
                int v = (int)Mathf.Log(value, 2);
                //Debug.Log("v: " + v);
                image.color = G2048ColorManager.instance.colors[v];
            }
        }
    }

    public class G2048ColorManager
    {
        public static G2048ColorManager instance = new G2048ColorManager();

        public List<Color> colors;

        public Color defaultColor = Color.gray;

        public G2048ColorManager()
        {
            colors = new List<Color>();
            for (int i = 0; i < 1000; i++)
            {
                bool acceptColor = false;
                Color c = Color.white;
                while (!acceptColor)
                {
                    c = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f), 1f);
                    acceptColor = true;
                    if (c == Color.gray)
                    {
                        acceptColor = false;
                    }
                    for (int j = 0; j < colors.Count && acceptColor; j++)
                    {
                        if (c == colors[j])
                        {
                            acceptColor = false;
                        }
                    }
                    //Debug.Log("Color " + i + " : " + c.ToString());
                }

                colors.Add(c);
            }
        }
    }
}
