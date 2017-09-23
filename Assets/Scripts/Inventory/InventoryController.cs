using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;


public class InventoryController : GameController
{
    public Transform SlotsParentContent; //인벤토리 부모 팬넬
    public Transform InventorySlot; //각 슬롯
    private int slotAmount = 25;
    public List<GameObject> slots = new List<GameObject>();
    public GameObject ItemInfoBackPanel;

    private int pssItemID;
    
    void Start () {
        PlayerPssItemLoadALL(); //전체 소유 아이템 로드

    }
    #region [플레이어 소유 아이템 전체 로드]
    protected void PlayerPssItemLoadALL()
    {
        for (int i = 0; i < slotAmount; i++)
        {
            GameObject SlotInfo = Resources.Load("Prefabs/InventorySlot") as GameObject;  //프리팹으로 등록된 정보 불러옴
            GameObject objslot = Instantiate(SlotInfo, SlotsParentContent);   //자식 오브젝트
            slots.Add(objslot);

            RectTransform rt = objslot.GetComponent<RectTransform>(); //SlotInfo
            rt.anchoredPosition = new Vector2(0f, 0f);       // 자식 오브젝트를 위치를 잡고 그린다
        }

        List<PssItem> passitems = DataController.Instance.GetPssItemInfo().PssItemList; //소유 아이템
        List<StuffInfo> stuffList = DataController.Instance.GetStuffInfo().StuffList;
        List<WeaponInfo> weaponinfos = DataController.Instance.GetWeaponInfo().WeaponList;

        for (int i = 0; i < passitems.Count; i++)
        {
             //무기로딩
            foreach (WeaponInfo weapon in weaponinfos)
            {
                if (passitems[i].GameItem_Type == "Weapon" && weapon.Wpn_ID == passitems[i].Item_ID)
                {
                    Color color = slots[i].transform.GetChild(0).GetComponent<Image>().color;
                    color.a = 1f;
                    slots[i].transform.GetChild(0).GetComponent<Image>().color = color;
                    slots[i].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/Weapon/" + weapon.Wpn_ImgName);
                    slots[i].transform.GetChild(1).GetComponent<Text>().text = passitems[i].Amount.ToString();
                    break;
                }
            }
            //물약로딩
            foreach (WeaponInfo weapon in weaponinfos)
            {
                if (passitems[i].GameItem_Type == "Hpotion")
                {
                    Color color = slots[i].transform.GetChild(0).GetComponent<Image>().color;
                    color.a = 1f;
                    slots[i].transform.GetChild(0).GetComponent<Image>().color = color;
                    slots[i].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/Hp");
                    slots[i].transform.GetChild(1).GetComponent<Text>().text = passitems[i].Amount.ToString();
                    break;
                }
            }
            //재료로딩
            foreach (StuffInfo stuff in stuffList)
            {
                if(passitems[i].GameItem_Type == "Stuff" && stuff.Stuff_ID == passitems[i].Item_ID)
                {
                    Color color = slots[i].transform.GetChild(0).GetComponent<Image>().color;
                    color.a = 1f;
                    slots[i].transform.GetChild(0).GetComponent<Image>().color = color;
                    slots[i].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/Stuff/" + stuff.Stuff_ImgName);
                    slots[i].transform.GetChild(1).GetComponent<Text>().text = passitems[i].Amount.ToString();
                    break;
                }
            }

        }


    }
    #endregion

    public void ShowItemInfoPanel()
    {
        ItemInfoBackPanel.gameObject.SetActive(true);
    }

    public void CloseItemInfoPanel()
    {
        ItemInfoBackPanel.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoMain();
        }
    }
}
