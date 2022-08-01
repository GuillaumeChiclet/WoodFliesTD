using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(menuName = "MapEditor/TMP_Editor_InputValidator")]
public class Editor_InputValidator : TMP_InputValidator
{
    public override char Validate(ref string text, ref int pos, char ch)
    {
        if (ch >= '0' && ch <= '9' || ch == ',')
        {
            text += ch;
            pos += 1;
            return ch;
        }
        if (ch == '.')
        {
            text += ',';
            pos += 1;
            return ch;
        }
        return (char)0;
    }
}
