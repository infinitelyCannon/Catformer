using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GetCharacter : MonoBehaviour
{
    public Sprite[] characterSprites;
    private int currentCharacter;
    public GameObject playerCat;
    public string spriteSheetName;
    public string Hello;

    void Start()
    {
        spriteSheetName = PlayerPrefs.GetString("AnimationSelected");
       currentCharacter = PlayerPrefs.GetInt("CharacterSelected");
        playerCat.gameObject.GetComponent<SpriteRenderer>().sprite = characterSprites[currentCharacter];
    }

    
    void LateUpdate()
    {
        spriteSheetName = PlayerPrefs.GetString("AnimationSelected");
        var subSprites = Resources.LoadAll<Sprite>("Characters/" + spriteSheetName);

        foreach (var renderer in GetComponentsInChildren<SpriteRenderer>())
        {
            string spriteName = renderer.sprite.name;
            var newSprite = Array.Find(subSprites, item => item.name == spriteName);

            if (newSprite)
                renderer.sprite = newSprite;
        }
    }
}
