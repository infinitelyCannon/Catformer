using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetCharacter : MonoBehaviour
{
    public Sprite[] characterSprites;
    private int currentCharacter;
    public GameObject playerCat;
    void Start()
    {
       currentCharacter = PlayerPrefs.GetInt("CharacterSelected");
        playerCat.gameObject.GetComponent<SpriteRenderer>().sprite = characterSprites[currentCharacter];
    }

    
    void Update()
    {
        
    }
}
