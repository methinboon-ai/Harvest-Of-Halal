using System.Collections.Generic;
using UnityEngine;

public class Planter : MonoBehaviour
{
    

    public virtual void Planting(GameObject plantPrefab)
    {
        GameObject plantingPlant = Instantiate(plantPrefab,transform.parent);
        plantingPlant.transform.position = transform.position;
        Destroy(gameObject);
    }
}
