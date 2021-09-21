using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MatrixSizeManager : MonoBehaviour
{
    [SerializeField]
    InputField xInput;

    [SerializeField]
    InputField yInput;

    [SerializeField]
    GameObject horizontalContent;

    [SerializeField]
    GameObject numberField;

    [SerializeField]
    Text biggestNum;

    [SerializeField]
    Text biggestArea;

    [SerializeField]
    Color findColor;

    private int[,] Tab;
    private int[,] temp;
    private int[,] maxTemp;
    private int area = 0;
    private int maxArea = 0;
    private int maxNumber;

    public void CreateMatrix()
    {
        if (xInput.text == "" || yInput.text == "")
        {
            return;
        }

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        int xLength = int.Parse(xInput.text);
        int yLength = int.Parse(yInput.text);

        Tab = new int[yLength, xLength];

        for (int i = 0; i < yLength; i++)
        {
            var content =
                Instantiate(horizontalContent,
                Vector3.zero,
                Quaternion.identity,
                this.transform);

            for (int j = 0; j < xLength; j++)
            {
                var number =
                    Instantiate(numberField,
                    Vector3.zero,
                    Quaternion.identity,
                    content.transform);
            }
        }
    }

    public void GetMatrix()
    {
        for (int i = 0; i < Tab.GetLength(0); i++)
        {
            for (int j = 0; j < Tab.GetLength(1); j++)
            {
                if (
                    transform
                        .GetChild(i)
                        .GetChild(j)
                        .GetComponent<InputField>()
                        .text ==
                    ""
                )
                {
                    transform
                        .GetChild(i)
                        .GetChild(j)
                        .GetComponent<InputField>()
                        .text = "0";
                }

                Tab[i, j] =
                    int
                        .Parse(transform
                            .GetChild(i)
                            .GetChild(j)
                            .GetComponent<InputField>()
                            .text);
            }
        }
    }

    public void FindCountElementOfBiggestArea()
    {
        if (xInput.text == "" || yInput.text == "")
        {
            return;
        }

        GetMatrix();
        maxArea = 0;

        for (int i = 0; i < Tab.GetLength(0); i++)
        {
            for (int j = 0; j < Tab.GetLength(1); j++)
            {
                CountCurrentArea (i, j);
            }
        }

        SetResult();
    }

    void FindNumberAround(int currentNumber, int i, int j)
    {
        temp[i, j] = 1;
        Tab[i, j] = -1;

        if (j + 1 != Tab.GetLength(1))
        {
            if (Tab[i, j + 1] == currentNumber)
            {
                FindNumberAround(currentNumber, i, j + 1);
            }
        }

        if (i + 1 != Tab.GetLength(0))
        {
            if (Tab[i + 1, j] == currentNumber)
            {
                FindNumberAround(currentNumber, i + 1, j);
            }
        }

        if (j - 1 > -1)
        {
            if (Tab[i, j - 1] == currentNumber)
            {
                FindNumberAround(currentNumber, i, j - 1);
            }
        }

        if (i - 1 > -1)
        {
            if (Tab[i - 1, j] == currentNumber)
            {
                FindNumberAround(currentNumber, i - 1, j);
            }
        }
    }

    void CountCurrentArea(int i, int j)
    {
        if (Tab[i, j] != -1)
        {
            int currentNumber = Tab[i, j];
            temp = new int[Tab.GetLength(0), Tab.GetLength(1)];
            FindNumberAround (currentNumber, i, j);

            area = Area(temp);

            if (area > maxArea)
            {
                maxArea = area;
                maxNumber = currentNumber;
                maxTemp = temp;
            }

            area = 0;
        }
    }

    int Area(int[,] temp)
    {
        int area = 0;
        for (int k = 0; k < temp.GetLength(0); k++)
        {
            for (int l = 0; l < temp.GetLength(1); l++)
            {
                if (temp[k, l] == 1)
                {
                    area += 1;
                }
            }
        }

        return area;
    }

    void SetResult()
    {
        biggestNum.text = "Biggest Number is " + maxNumber.ToString();
        biggestArea.text = "Biggest Area is " + maxArea.ToString();

        for (int i = 0; i < maxTemp.GetLength(0); i++)
        {
            for (int j = 0; j < maxTemp.GetLength(1); j++)
            {
                if (maxTemp[i, j] == 1)
                {
                    transform
                        .GetChild(i)
                        .GetChild(j)
                        .GetComponent<Image>()
                        .color = findColor;
                }
                else
                {
                    transform
                        .GetChild(i)
                        .GetChild(j)
                        .GetComponent<Image>()
                        .color = Color.white;
                }
            }
        }
    }
}
