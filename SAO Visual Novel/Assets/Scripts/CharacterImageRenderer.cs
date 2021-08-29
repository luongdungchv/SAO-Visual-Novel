using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterImageRenderer : MonoBehaviour
{
    public Character character;
    public void SetProperty(string name, int bIndex, int eIndex)
    {
        character.name = name;
        character.bodyIndex = bIndex;
        character.emotionIndex = eIndex;
    }
    public void SetProperty(Character input)
    {
        SetProperty(input.name, input.bodyIndex, input.emotionIndex);
    }
}
