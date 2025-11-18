using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public interface IInteractable
{
    void Planting(Planter planter, GameObject plantPrefab);
    void Harvesting(Plant plant);
    void ItemPickUp(Item item);
}

public class InventoryItem
{
    public string ItemName;
    public int Amount;

    public bool isUnavailable()
    {
        if (Amount <= 0)
        {
            return false;
        }
        return true;
    }
}

public class Player : MonoBehaviour, IInteractable
{
    [SerializeField] List<GameObject> PlantPrefabs;
    [SerializeField] Canvas PlayerUI;
    TMP_Text InteractText;
    TMP_Text SelectText;
    InventoryItem selectingItem;

    // Inventory System
    List<InventoryItem> Inventorylist = new List<InventoryItem>();
    public void AddItem(string itemName, int amountToAdd)
    {
        // 1. ค้นหาไอเท็มใน List ด้วยชื่อ
        InventoryItem existingItem = Inventorylist
            .FirstOrDefault(item => item.ItemName == itemName);

        // 2. ตรวจสอบผลการค้นหา
        if (existingItem != null)
        {
            // ถ้าเจอไอเท็มเดิม: ให้เพิ่มจำนวน (Amount) เข้าไป
            existingItem.Amount += amountToAdd;
            Debug.Log($"Added {amountToAdd} x {itemName}. New total: {existingItem.Amount}");
        }
        else
        {
            // ถ้าไม่เจอไอเท็มเดิม: ให้สร้าง InventoryItem ใหม่แล้วเพิ่มเข้า List
            InventoryItem newItem = new InventoryItem
            {
                ItemName = itemName,
                Amount = amountToAdd
            };
            Inventorylist.Add(newItem);
            Debug.Log($"New item added: {amountToAdd} x {itemName}");
        }
    }
    void DisplayItem()
    {
        SelectText.gameObject.SetActive( true );
        SelectText.text = $"(Q/E) {selectingItem.ItemName} x{selectingItem.Amount}";
    }
    void SwitchItem(string switchType)
    {
        if (Inventorylist.Count <= 0) return;
        if (selectingItem == null || switchType == null)
        {
            selectingItem = Inventorylist.First();
        }
        if (switchType == "Left")
        {
            int currentIndexItem = Inventorylist.IndexOf(selectingItem);
            if (currentIndexItem - 1 <= -1) {
                selectingItem = Inventorylist[Inventorylist.Count - 1];
            }
        }
        if (switchType == "Right")
        {
            int currentIndexItem = Inventorylist.IndexOf(selectingItem);
            if (currentIndexItem + 1 >= Inventorylist.Count)
            {
                selectingItem = Inventorylist[0];
            }
        }
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
                InteractText.gameObject.SetActive(true);
                if (plant.currentStage >= plant.MaxStageUnit)
                {
                    InteractText.text = "(F) Harvest";
                    return;
                }
            }
            if (planter != null)
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
        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            Transform target = FindMouseTarget();
            Planter planter = target.GetComponent<Planter>();
            Plant plant = target.GetComponent<Plant>();
            Item item = target.GetComponent <Item>();
            if (plant != null)
            {
                
                if (plant.currentStage >= plant.MaxStageUnit)
                {
                    // Harvest
                    Harvesting(plant);
                    return;
                }
            }
            if (planter != null)
            {
                // Plant
                //Planting(planter);
                return;
            }
        }
        // Switch Inventory Trigger
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchItem("Left");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchItem("Right");
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Transform target = FindMouseTarget();
            Item item = target.GetComponent<Item>();
            if (item != null)
            {
                ItemPickUp(item);
            }
        }
        DisplayMouseTarget();
    }

    public void Planting(Planter planter, GameObject plantPrefab)
    {
        planter.Planting(plantPrefab);
    }

    public void Harvesting(Plant plant)
    {
        plant.Harvesting(this);
    }

    public void ItemPickUp(Item item)
    {
        AddItem(item.ItemName, 1);
        item.PickUp(this);
        if (selectingItem == null)
        {
            SwitchItem(null);
        }
    }
}
