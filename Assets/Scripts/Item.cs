using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] public string ItemName;
    public virtual void PickUp(Player _player)
    {
        Destroy(gameObject);
    }
}
