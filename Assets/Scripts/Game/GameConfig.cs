using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfig", order = 1)]
public class GameConfig : ScriptableObject
{

    public List<string> words;
    [Header("For game 3")]
    public List<char> wordLetters;
    public List<char> gameLetters; 

}
