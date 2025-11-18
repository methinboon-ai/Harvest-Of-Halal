using System.Collections.Generic;
using TMPro;
using UnityEngine;

public interface IInteractable
{
    void Planting(Planter planter, GameObject plantPrefab);
    void Harvesting(Plant plant);
}

public class Player : MonoBehaviour, IInteractable
{
    [SerializeField] List<GameObject> PlantPrefabs;
    [SerializeField] Canvas PlayerUI;
    TMP_Text InteractText;
    TMP_Text SelectText;

    

    void Start()
    {
        InteractText = PlayerUI.transform.Find("InteractText").GetComponent<TMP_Text>();
        SelectText = PlayerUI.transform.Find("SelectText").GetComponent<TMP_Text>();
        InteractText.gameObject.SetActive(false);
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
}
