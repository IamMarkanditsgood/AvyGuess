using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game3Config", menuName = "ScriptableObjects/Game3Config", order = 1)]
public class Game3Config : ScriptableObject
{
    [SerializeField] public List<Wrod> words;

    public void SetChars()
    {
        foreach (var word in words)
        {
            word.wordLetters = new List<char>(word.word.ToCharArray());
            word.gameLetters = new List<char>(word.letters.ToCharArray());
        }
    }

    [Serializable]
    public class Wrod
    {
        public string word;
        public string letters; 
        public Sprite closedImage;
        public Sprite openedImage;
        public List<char> wordLetters;
        public List<char> gameLetters;
    }
}
