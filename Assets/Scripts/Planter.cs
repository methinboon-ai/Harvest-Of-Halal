using System.Collections.Generic;
using UnityEngine;

public class Planter : MonoBehaviour
{
    

    public virtual void Planting(GameObject plantPrefab)
    {
        GameObject plantingPlant = Instantiate(plantPrefab,transform.parent);
        plantingPlant.gameObject.name = plantPrefab.name;
        plantingPlant.transform.position = transform.position;
        Destroy(gameObject);
    }
}
