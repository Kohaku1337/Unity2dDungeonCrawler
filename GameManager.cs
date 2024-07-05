using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   public static GameManager instance;

   private void Awake()
   {
       if (GameManager.instance != null)
       {
           Destroy(gameObject);
           Destroy(player.gameObject);
           Destroy(floatingTextManager.gameObject);
           Destroy(hud.gameObject);
           Destroy(menu.gameObject);
           return;
       }
      instance = this; ;
      SceneManager.sceneLoaded += LoadState;
      SceneManager.sceneLoaded += OnSceneLoaded;
   }
   //resources
   public List<Sprite> playerSprites;
   public List<Sprite> weaponSprites;
   public List<int> weaponPrices;
   public List<int> xpTable;
   //references
   public Player player;
   public Weapon weapon;
   public FloatingTextManager floatingTextManager;
   public RectTransform hitpointBar;
   public Animator deathMenuAnim;
   public GameObject hud;
   public GameObject menu;

   //logic
   public int gold;
   public int xp;
   //Floating Text
   public void ShowText(string msg, int fontSize, Color color,Vector3 position, Vector3 motion, float duration)
   {
       floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
   }
   
   //Upgrade Weapon
   public bool TryUpgradeWeapon()
   {
       // is the weapon max lvl?
       if (weaponPrices.Count <= weapon.weaponLevel)
            return false;

       if (gold >= weaponPrices[weapon.weaponLevel])
       {
           gold -= weaponPrices[weapon.weaponLevel];
           weapon.UpgradeWeapon();
           return true;
       }

       return false;
   }
    //healthbar
   public void OnHitpointChange()
   {
       float ratio = (float)player.hitPoint / (float)player.maxHitpoint;
       hitpointBar.localScale = new Vector3(1,ratio, 1);
   }
   
   //Exp
   public int GetCurrentLevel()
   {
       int r = 0;
       int add = 0;

       while (xp >= add) 
        {
            add += xpTable[r];
            r++;

            if (r == xpTable.Count) // if max lvl
                return r;
        }

        return r;
       
   }
   public int GetXpToLevel(int level)
   {
       int r = 0;
       int xp = 0;
       while (r < level)
       {
           xp += xpTable[r];
           r++;
       }
       return xp;

   }
   public void GrantXP(int exp)
   {
       int currLevel = GetCurrentLevel();
       xp += exp;
       if (currLevel < GetCurrentLevel())
            OnLevelUp();
   }
   public void OnLevelUp()
   {
       ShowText("Level up!", 25, Color.magenta, transform.position, transform.position + new Vector3(0,0.16f,0), 1.5f);
       player.OnLevelUp();
       OnHitpointChange();
   }
   //scene load
   public void OnSceneLoaded(Scene s,LoadSceneMode mode)
   {
       player.transform.position = GameObject.Find("SpawnPoint").transform.position;
   }
   //death menu and respawn
   public void Respawn()
   {
       deathMenuAnim.SetTrigger("Hide");
       UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
       player.Respawn();
   }

   //save state
   public void SaveState()
   {
       string s = "";

       s += "0" + "|";
       s += gold.ToString() + "|";
       s += xp.ToString() + "|";
       s += weapon.weaponLevel.ToString();
           
       PlayerPrefs.SetString("SaveState", s);
   }

   public void LoadState(Scene s,LoadSceneMode mode)
   {
       SceneManager.sceneLoaded -= LoadState;

       if(!PlayerPrefs.HasKey("SaveState"))
           return;
       string[] data = PlayerPrefs.GetString("SaveState").Split('|');
       //change player skin
       gold = int.Parse(data[1]);
       //xp
       xp = int.Parse(data[2]);
       if (GetCurrentLevel() != 1)
            player.SetLevel(GetCurrentLevel());
       //change weapon lvl
       weapon.SetWeaponLevel(int.Parse(data[3]));

       player.transform.position = GameObject.Find("SpawnPoint").transform.position;
   }
}
