using MultiplayerARPG;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class UICharacterCreateExtension : UICharacterCreate
{
    public Text HairList;
    public Text FaceList;
    public Text BeardList;
    public Text EyebrowList;

    protected int selectedHair = 1;
    protected int selectedFace = 1;
    protected int selectedBeard = 1;
    protected int selectedEyebrow = 1;

    public override void Show()
    {
       
        EquipHair();
        //EquipBeard();
        //EquipFace();
        //EquipEyebrow();
        LoadHair();
        LoadBeard();
        LoadFace();
        LoadEyebrow();



        base.Show();
    }

    protected virtual List<Item> GetSelectableHair()
    {
        List<BaseItem> items = GameInstance.Items.Values.ToList();
        List<Item> Hairs = new List<Item>();

        foreach (BaseItem item in items)
        {
            if (item as Item)
            {
                Item armoritem = (Item)item;
                if (armoritem.ArmorType.name.ToString().Equals("Hair"))
                    Hairs.Add(armoritem);
            }
               
        }

        return Hairs;
    }

    protected virtual List<Item> GetSelectableFaces()
    {
        List<BaseItem> items = GameInstance.Items.Values.ToList();
        List<Item> Faces = new List<Item>();

        foreach (BaseItem item in items)
        {
            if (item as Item)
            {
                Item armoritem = (Item)item;
                if (armoritem.ArmorType.name.ToString().Equals("Face"))
                    Faces.Add(armoritem);
            }
        }

        return Faces;
    }

    protected virtual List<Item> GetSelectableBeard()
    {
        List<BaseItem> items = GameInstance.Items.Values.ToList();
        List<Item> Beard = new List<Item>();

        foreach (BaseItem item in items)
        {
            if (item as Item)
            {
                Item armoritem = (Item)item;
                if (armoritem.ArmorType.name.ToString().Equals("Beard"))
                    Beard.Add(armoritem);
            }
        }

        return Beard;
    }

    protected virtual List<Item> GetSelectableEyebrow()
    {
        List<BaseItem> items = GameInstance.Items.Values.ToList();
        List<Item> Eyebrow = new List<Item>();

        foreach (BaseItem item in items)
        {
            if (item as Item)
            {
                Item armoritem = (Item)item;
                if (armoritem.ArmorType.name.ToString().Equals("Eyebrow"))
                    Eyebrow.Add(armoritem);
            }
        }

        return Eyebrow;
    }

    protected virtual void LoadHair()
    {
        HairList.text = selectedHair + " / " + GetSelectableHair().Count.ToString();
        Debug.Log(GetSelectableHair().Count.ToString());
    }
    protected virtual void LoadFace()
    {
        FaceList.text = selectedFace + " / " + GetSelectableFaces().Count.ToString();
        Debug.Log(GetSelectableFaces().Count.ToString());
    }
    protected virtual void LoadBeard()
    {
        BeardList.text = selectedBeard + " / " + GetSelectableBeard().Count.ToString();
        Debug.Log(GetSelectableFaces().Count.ToString());
    }
    protected virtual void LoadEyebrow()
    {
        EyebrowList.text = selectedEyebrow + " / " + GetSelectableEyebrow().Count.ToString();
        Debug.Log(GetSelectableEyebrow().Count.ToString());
    }

    public void EquipHair()
    {
        PlayerCharacterEntity player = null;
        foreach (Transform child in characterModelContainer)
            if (child.gameObject.activeInHierarchy == true)
                player = child.gameObject.GetComponent<PlayerCharacterEntity>();
        if (player != null)
            foreach (BasePlayerCharacterEntity BplayerCharacter in GameInstance.PlayerCharacterEntities.Values.ToList())
                foreach (PlayerCharacter playerCharacter in BplayerCharacter.CharacterDatabases)
                    playerCharacter.ArmorItems[0] = GetSelectableHair()[selectedHair - 1];
    }
    public void EquipBeard()
    {
        PlayerCharacterEntity player = null;
        foreach (Transform child in characterModelContainer)
            if (child.gameObject.activeInHierarchy == true)
                player = child.gameObject.GetComponent<PlayerCharacterEntity>();
        if (player != null)
            foreach (BasePlayerCharacterEntity BplayerCharacter in GameInstance.PlayerCharacterEntities.Values.ToList())
                foreach (PlayerCharacter playerCharacter in BplayerCharacter.CharacterDatabases)
                    playerCharacter.ArmorItems[1] = GetSelectableBeard()[selectedBeard - 1];
    }
    public void EquipFace()
    {
        PlayerCharacterEntity player = null;
        foreach (Transform child in characterModelContainer)
            if (child.gameObject.activeInHierarchy == true)
                player = child.gameObject.GetComponent<PlayerCharacterEntity>();
        if (player != null)
            foreach (BasePlayerCharacterEntity BplayerCharacter in GameInstance.PlayerCharacterEntities.Values.ToList())
                foreach (PlayerCharacter playerCharacter in BplayerCharacter.CharacterDatabases)
                    playerCharacter.ArmorItems[2] = GetSelectableFaces()[selectedFace - 1];
    }
    public void EquipEyebrow()
    {
        PlayerCharacterEntity player = null;
        foreach (Transform child in characterModelContainer)
            if (child.gameObject.activeInHierarchy == true)
                player = child.gameObject.GetComponent<PlayerCharacterEntity>();
        if (player != null)
            foreach (BasePlayerCharacterEntity BplayerCharacter in GameInstance.PlayerCharacterEntities.Values.ToList())
                foreach (PlayerCharacter playerCharacter in BplayerCharacter.CharacterDatabases)
                    playerCharacter.ArmorItems[3] = GetSelectableEyebrow()[selectedEyebrow - 1];
    }

    public void UpdateCharacter()
    {
        // Prepare equip items
        List<CharacterItem> equipItems = new List<CharacterItem>();
        foreach (BaseItem armorItem in SelectedPlayerCharacter.ArmorItems)
        {
            if (armorItem == null)
                continue;
            equipItems.Add(CharacterItem.Create(armorItem));
        }
        // Set model equip items
        SelectedModel.SetEquipItems(equipItems);
        // Prepare equip weapons
        EquipWeapons equipWeapons = new EquipWeapons();
        if (SelectedPlayerCharacter.RightHandEquipItem != null)
            equipWeapons.rightHand = CharacterItem.Create(SelectedPlayerCharacter.RightHandEquipItem);
        if (SelectedPlayerCharacter.LeftHandEquipItem != null)
            equipWeapons.leftHand = CharacterItem.Create(SelectedPlayerCharacter.LeftHandEquipItem);
        // Set model equip weapons
        SelectedModel.SetEquipWeapons(equipWeapons);
    }

    public void NextHair()
    {
        if (selectedHair < GetSelectableHair().Count)
        {
            selectedHair++;
            EquipHair();
            LoadHair();
            UpdateCharacter();
        }
    }

    public void PreviousHair()
    {
        if (selectedHair - 1 > 0)
        {
            selectedHair--;
            EquipHair();
            LoadHair();
            UpdateCharacter();
        }
    }

    public void NextBeard()
    {
        if (selectedBeard < GetSelectableBeard().Count)
        {
            selectedBeard++;
            EquipBeard();
            LoadBeard();
            UpdateCharacter();
        }
    }

    public void PreviousBeard()
    {
        if (selectedBeard - 1 > 0)
        {
            selectedBeard--;
            EquipBeard();
            LoadBeard();
            UpdateCharacter();
        }
    }

    public void NextFace()
    {
        if (selectedFace < GetSelectableFaces().Count)
        {
            selectedFace++;
            EquipFace();
            LoadFace();
            UpdateCharacter();
        }

    }

    public void PreviousFace()
    {
        if (selectedFace - 1 > 0)
        {
            selectedFace--;
            EquipFace();
            LoadFace();
            UpdateCharacter();
        }
    }

    public void NextEyebrowm()
    {
        if (selectedEyebrow < GetSelectableEyebrow().Count)
        {
            selectedEyebrow++;
            EquipEyebrow();
            LoadEyebrow();
            UpdateCharacter();
        }

    }

    public void PreviousEyebrowm()
    {
        if (selectedEyebrow - 1 > 0)
        {
            selectedEyebrow--;
            EquipEyebrow();
            LoadEyebrow();
            UpdateCharacter();
        }
    }
}
