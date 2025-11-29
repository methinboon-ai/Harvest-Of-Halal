using UnityEngine;

public interface IPlantable
{
    GameObject GetPlantPrefab();
}

public abstract class InventoryItem
{
    public string ItemName { get; private set; }
    public int Amount { get; private set; }

    public void Add(int newAmount) => Amount += newAmount;
    public void Remove(int removingAmount) => Amount = Mathf.Max(0,Amount - removingAmount);
    public void SetName (string Name) => ItemName = Name ?? string.Empty;
}

public class Seed_Class : InventoryItem, IPlantable
{
    public GameObject PlantPrefab; // Put string of Plant Prefab

    public GameObject GetPlantPrefab()
    {
        return PlantPrefab;
    }
}

public class Crop_Class : InventoryItem { }
