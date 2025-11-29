using UnityEngine;
public class CarrotSeed_Item : Item
{
    Seed_Class _item = new Seed_Class();
    public override InventoryItem InventoryItem => _item;
    [SerializeField] GameObject PlantPrefab;
    [SerializeField] string ItemName;
    private void Start()
    {
        _item.PlantPrefab = PlantPrefab;
        _item.SetName(ItemName);
    }
}
