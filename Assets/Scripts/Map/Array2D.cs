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

    public bool TryGet(Vector2Int coords, out T value)
    {
        return TryGet(coords.x, coords.y, out value);
    }

    public bool TryGet(int x, int y, out T value)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            value = default(T);
            return false;
        }

        value = list[x * height + y];
        return true;
    }

    public bool TrySet(Vector2Int coords, T value)
    {
        return TrySet(coords.x, coords.y, value);
    }

    public bool TrySet(int x, int y, T value)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
            return false;

        list[x * height + y] = value;
        return true;
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
