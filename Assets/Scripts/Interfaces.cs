
using UnityEngine;

public interface ISerializable
{
    public string ToJson();
    public void FromJson(string json);
}

public interface IInteractable 
{
    void PrimarAction(GameObject caller);
    void SecondAction(GameObject caller);
}