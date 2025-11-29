using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;



public class Player : MonoBehaviour
{
    [SerializeField] List<GameObject> PlantPrefabs;
    [SerializeField] Canvas PlayerUI;
    TMP_Text InteractText;
    TMP_Text SelectText;
    InventoryItem selectingItem;

    // Inventory System
    List<InventoryItem> Inventorylist = new List<InventoryItem>();
    void AddItem(InventoryItem _newitem, int amountToAdd)
    {
        // 1. ค้นหาไอเท็มใน List ด้วยชื่อ
        InventoryItem existingItem = Inventorylist
            .FirstOrDefault(item => item.ItemName == _newitem.ItemName);

        // 2. ตรวจสอบผลการค้นหา
        if (existingItem != null)
        {
            // ถ้าเจอไอเท็มเดิม: ให้เพิ่มจำนวน (Amount) เข้าไป
            existingItem.Add(amountToAdd);
            Debug.Log($"Added {amountToAdd} x {existingItem.ItemName}. New total: {existingItem.Amount}");
        }
        else
        {
            // ถ้าไม่เจอไอเท็มเดิม: ให้สร้าง InventoryItem ใหม่แล้วเพิ่มเข้า List
            InventoryItem newItem = _newitem;
            Inventorylist.Add(newItem);
            newItem.Add(amountToAdd);
            Debug.Log($"New item added: {amountToAdd} x {newItem.ItemName}");
        }
    }
    void RemoveItem(InventoryItem _item, int amountToRemove) {
        InventoryItem existingItem = Inventorylist
            .FirstOrDefault(item => item == _item);
        if (existingItem != null)
        {
            existingItem.Remove(amountToRemove);
            if (existingItem.Amount <= 0)
            {
                Inventorylist.Remove(existingItem);
                selectingItem = null;
                SwitchItem(null);
            }
            else
            {
                DisplayItem();
            }
        }
    }
    void DisplayItem()
    {
        if (Inventorylist.Count <= 0)
        {
            SelectText.gameObject.SetActive(false);
            return;
        }
        SelectText.gameObject.SetActive( true );
        SelectText.text = $"(Q/E) {selectingItem.ItemName} x{selectingItem.Amount}";
    }
    void SwitchItem(string switchType)
    {
        // 1. ตรวจสอบลิสต์ว่างเปล่า
        if (Inventorylist.Count <= 0)
        {
            DisplayItem();
            return;
        }

        // 2. ตรวจสอบ/กำหนดค่าเริ่มต้น
        if (selectingItem == null || switchType == null)
        {
            selectingItem = Inventorylist.First();
            DisplayItem(); // แสดงผลไอเท็มแรกทันที
            return; // ออกจากฟังก์ชันเมื่อกำหนดค่าเริ่มต้นแล้ว
        }

        // 3. หาสถานะปัจจุบัน
        int currentIndexItem = Inventorylist.IndexOf(selectingItem);

        if (switchType == "Left")
        {
            // คำนวณ Index ใหม่สำหรับ "Left" (ก่อนหน้า)
            // ใช้ Modulo (%) เพื่อวนกลับ: (Index - 1 + Count) % Count
            int newIndexItem = (currentIndexItem - 1 + Inventorylist.Count) % Inventorylist.Count;

            // กำหนดไอเท็มใหม่
            selectingItem = Inventorylist[newIndexItem];
        }
        else if (switchType == "Right") // ใช้ else if เพื่อประหยัดเวลาประมวลผล
        {
            // คำนวณ Index ใหม่สำหรับ "Right" (ถัดไป)
            // ใช้ Modulo (%) เพื่อวนกลับ: (Index + 1) % Count
            int newIndexItem = (currentIndexItem + 1) % Inventorylist.Count;

            // กำหนดไอเท็มใหม่
            selectingItem = Inventorylist[newIndexItem];
        }

        // 4. แสดงผลไอเท็มที่ถูกเลือกใหม่
        DisplayItem();
    }

    void Start()
    {
        InteractText = PlayerUI.transform.Find("InteractText").GetComponent<TMP_Text>();
        SelectText = PlayerUI.transform.Find("SelectText").GetComponent<TMP_Text>();
        InteractText.gameObject.SetActive(false);
        SelectText.gameObject.SetActive(false);
    }

    Transform FindMouseTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit))
        {
            // If the ray hits an object, get its Transform
            Transform targetTransform = hit.transform;

            if (targetTransform != null) return targetTransform;
                
            return null;
        }
        else
        {
            return null;
        }
    }

    void DisplayMouseTarget()
    {
        Transform target = FindMouseTarget();
        if (target != null) {
            Planter planter = target.GetComponent<Planter>();
            Plant plant = target.GetComponent<Plant>();
            Item item = target.GetComponent<Item>();
            //Debug.Log(target);
            if (plant != null)
            {
                //Debug.Log($"{plant.name} Harvestable : {plant.Harvestable}");
                if (plant.Harvestable == true)
                {
                    InteractText.gameObject.SetActive(true);
                    InteractText.text = "(F) Harvest";
                    return;
                }
            }
            if (planter != null && selectingItem is IPlantable plantable)
            {
                InteractText.gameObject.SetActive(true);
                InteractText.text = "(F) Plant";
                return;
            }
            if (item != null) {
                InteractText.gameObject.SetActive (true);
                InteractText.text = "(F) Pick up";
                return;
            }
            InteractText.gameObject.SetActive(false);
        }
        else
        {
            InteractText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        DisplayMouseTarget(); // Display Target On Screen
        // Switch Inventory Trigger
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchItem("Left");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchItem("Right");
        }
        // Interact
        if (Input.GetKeyDown(KeyCode.F))
        {
            Transform target = FindMouseTarget();
            Item item = target.GetComponent<Item>();
            if (item != null) ItemPickUp(item);
            Planter planter = target.GetComponent<Planter>();
            if (planter != null) Planting(planter);
            Plant plant = target.GetComponent<Plant>();
            if (plant != null && plant.Harvestable == true) Harvesting(plant);
        }
    }
    // Functions
    public void Planting(Planter planter)
    {
        if (selectingItem is IPlantable plantable)
        {
            if (selectingItem.Amount <= 0)
            {
                return;
            }
            RemoveItem(selectingItem, 1);
            planter.Planting(plantable.GetPlantPrefab());
        }
        
    }

    public void Harvesting(Plant plant)
    {
        plant.Harvesting(this);
    }

    public void ItemPickUp(Item item)
    {
        InventoryItem _item = item.InventoryItem;
        Debug.Log(_item.ItemName);
        AddItem(_item, 1);
        item.PickUp(this);
        if (selectingItem == null)
        {
            SwitchItem(null);
        }
        DisplayItem();
    }
    public void ItemPickUp(InventoryItem _inventoryItem)
    {
        AddItem(_inventoryItem, 1);
        if (selectingItem == null)
        {
            SwitchItem(null);
        }
        DisplayItem();
    }
}
