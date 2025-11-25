using UnityEngine;
public class BroccoliSeed_Item : Item
{
    BroccoliSeed_Class _item = new BroccoliSeed_Class();
    public override InventoryItem InventoryItem => _item;
    [SerializeField] GameObject PlantPrefab;
    [SerializeField] string ItemName;
    private void Start()
    {
        _item.PlantPrefab = PlantPrefab;
        _item.SetName(ItemName);
    }
}
