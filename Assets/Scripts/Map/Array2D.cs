using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Array2D<T> where T : new()
{
    int width;
    int height;
    List<T> list;

    public void Initialize(int width, int height)
    {
        this.width = width;
        this.height = height;
        list = new List<T>(width * height);
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                list.Add(new T());
            }
        }
    }

    public void Clear()
    {
        list.Clear();
        width = 0;
        height = 0;
    }

    public T Get(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
            return new T();

        return list[x * height + y];
    }

    public void Set(int x, int y, T value)
    {
        list[x * height + y] = value;
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public List<T> GetAsList()
    {
        return list;
    }

    public void CreateFromList(List<T> newList, int width, int height)
    {
        list = newList;
        this.width = width;
        this.height = height;
    }
}
