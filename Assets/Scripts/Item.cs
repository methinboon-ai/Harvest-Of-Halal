using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public abstract InventoryItem InventoryItem { get; }    
    public void PickUp(Player _player)
    {
        Destroy(gameObject);
    }
}
