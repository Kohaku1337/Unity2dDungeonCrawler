using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    // Text fields
    public Text levelText, hitpointText, goldText, upgradeCostText, xpText;

    // Logic
    private int currentCharacterSelection = 0;
    public Image characterSelectionSprite;
    public Image weaponSprite;
    public RectTransform xpBar;

    // Character Selection
    public void OnArrowClick(bool right)
    {
         if(right)
         {
             currentCharacterSelection++;

             // If we went too far away
             if (currentCharacterSelection == GameManager.instance.playerSprites.Count)
             currentCharacterSelection = 0;

             OnSelectionChanged();
         }
        else
            
        {
            currentCharacterSelection--;

            // If we went too far away
             if (currentCharacterSelection < 0)
             currentCharacterSelection = GameManager.instance.playerSprites.Count - 1;

             OnSelectionChanged();
        }
         
    }
    private void OnSelectionChanged() 
    {
        characterSelectionSprite.sprite = GameManager.instance.playerSprites[currentCharacterSelection];
        GameManager.instance.player.SwapSprite(currentCharacterSelection);
    }

    // Weapon Upgrade
    public void OnUpgradeClick()
    {
        if(GameManager.instance.TryUpgradeWeapon())
            UpdateMenu();
    }

    // Update the character Information
    public void UpdateMenu()
    {
        // Weapon
        weaponSprite.sprite = GameManager.instance.weaponSprites[GameManager.instance.weapon.weaponLevel];
        if(GameManager.instance.weapon.weaponLevel == GameManager.instance.weaponPrices.Count)
            upgradeCostText.text = "MAX";
        else
            upgradeCostText.text = GameManager.instance.weaponPrices[GameManager.instance.weapon.weaponLevel].ToString();
        // Meta
        levelText.text = GameManager.instance.GetCurrentLevel().ToString();
        hitpointText.text = GameManager.instance.player.hitPoint.ToString();
        goldText.text = GameManager.instance.gold.ToString();
        // xp Bar
        int currLvl = GameManager.instance.GetCurrentLevel();
        if (currLvl == GameManager.instance.xpTable.Count)
        {
            xpText.text = GameManager.instance.xp.ToString() + "total experience points"; // display total exp
            xpBar.localScale = Vector3.one;
        }
        else
        {
            int prevLvlXp = GameManager.instance.GetXpToLevel(currLvl - 1);
            int currLvlXp = GameManager.instance.GetXpToLevel(currLvl);

            int diff = currLvlXp - prevLvlXp;
            int currXpIntoLvl = GameManager.instance.xp - prevLvlXp;

            float completionRatio = (float)currXpIntoLvl / (float)diff;
            xpBar.localScale = new Vector3(completionRatio,1,1);
            xpText.text = currXpIntoLvl.ToString() + "/" + diff;

        }
    }


}

