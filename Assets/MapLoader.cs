using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic; 

public class MapLoader
{
    //[MenuItem("Tools/Read file")]
    public static float[,] ReadMap(Dictionary<char, float> leyend)
    {
        string path = "Assets/map.txt";

        int height, width;
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path); 

        string line = reader.ReadLine();

        width = int.Parse(line.Split(' ')[0].Split('=')[1]);
        height = int.Parse(line.Split(' ')[1].Split('=')[1]);

        float[,] matrix = new float[height, width];

        while(!reader.EndOfStream)
        {
            line = reader.ReadLine();
            string[] index = line.Split('|')[0].Split(' ');
            matrix[int.Parse(index[0]),int.Parse(index[1])] = leyend[char.Parse(line.Split('|')[1])];
        }

        /*
        for (int i = 0; i < height; i++)
        {
            for (int ii = 0; ii < width; ii++)
            {
                Debug.Log(matrix[i,ii]);
            }
        }
        */
        reader.Close();

        return matrix;
    }

    public static float[,] ExpandMap(float[,] map, int expansionSize)
    {
        int height = map.GetLength(0);
        int width = map.GetLength(1);

        float[,] res = new float[height * expansionSize, width * expansionSize];
        for (int i = 0; i < height; i++)
        {
            for (int ii = 0; ii < width; ii++)
            {
                for (int x = 0; x < expansionSize; x++)
                {
                    for (int y = 0; y < expansionSize; y++)
                    {
                        res[expansionSize*i + x,expansionSize*ii + y] = map[i,ii];
                    }
                }
            }
        }

        return res;
    }
}
