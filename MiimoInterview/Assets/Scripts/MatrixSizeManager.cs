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

    private int[,] Tab;

    private int[,] temp;

    private int[,] maxTemp;

    private int count = 0;

    private int maxCount = 0;

    private int maxNumber;

    public Color findColor;

    public void CreateMatrix()
    {
        if (xInput.text == "" || yInput.text == "")
        {
            Debug.Log("Please Enter Value");
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
                if (transform.GetChild(i).GetChild(j).GetComponent<InputField>().text == "")
                {
                    transform.GetChild(i).GetChild(j).GetComponent<InputField>().text = "0";
                }

                Tab[i, j] = int.Parse(transform.GetChild(i).GetChild(j).GetComponent<InputField>().text);
            }
        }
    }

    public void FindCountElementOfBiggestArea()
    {
        if (xInput.text == "" || yInput.text == "")
        {
            return;
        }

        maxCount = 0;

        GetMatrix();

        int currentNumber = 0;

        //i = row , j = column
        for (int i = 0; i < Tab.GetLength(0); i++)
        {
            for (int j = 0; j < Tab.GetLength(1); j++)
            {
                transform.GetChild(i).GetChild(j).GetComponent<Image>().color = Color.white;

                if (Tab[i, j] != currentNumber && Tab[i, j] != -1)
                {
                    currentNumber = Tab[i, j];

                    temp = new int[Tab.GetLength(0), Tab.GetLength(1)];

                    FindNumberAround (currentNumber, i, j);

                    for (int k = 0; k < temp.GetLength(0); k++)
                    {
                        for (int l = 0; l < temp.GetLength(1); l++)
                        {
                            if (temp[k, l] == 1)
                            {
                                count += 1;
                            }
                        }
                    }

                    if (count > maxCount)
                    {
                        maxCount = count;
                        maxNumber = currentNumber;
                        maxTemp = temp;
                    }

                    count = 0;
                }
            }
        }

        biggestNum.text = "Biggest Number is " + maxNumber.ToString();
        biggestArea.text = "Biggest Area is " + maxCount.ToString();

        for (int i = 0; i < maxTemp.GetLength(0); i++)
        {
            for (int j = 0; j < maxTemp.GetLength(1); j++)
            {
                if(maxTemp[i, j] == 1)
                {
                    transform.GetChild(i).GetChild(j).GetComponent<Image>().color = findColor;
                }
            }
        }
    }

    void FindNumberAround(int currentNumber, int i, int j)
    {
        temp[i, j] = 1;

        if (j + 1 != Tab.GetLength(1))
        {
            if (Tab[i, j + 1] == currentNumber)
            {
                Tab[i, j] = -1;
                FindNumberAround(currentNumber, i, j + 1);
            }
        }

        if (i + 1 != Tab.GetLength(0))
        {
            if (Tab[i + 1, j] == currentNumber)
            {
                Tab[i, j] = -1;
                FindNumberAround(currentNumber, i + 1, j);
            }
        }

        if (j - 1 > -1)
        {
            if (Tab[i, j - 1] == currentNumber)
            {
                Tab[i, j] = -1;
                FindNumberAround(currentNumber, i, j - 1);
            }
        }

        if (i - 1 > -1)
        {
            if (Tab[i - 1, j] == currentNumber)
            {
                Tab[i, j] = -1;
                FindNumberAround(currentNumber, i - 1, j);
            }
        }
    }
}
