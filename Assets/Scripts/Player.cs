using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
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

            if (targetTransform.name == "Planter")
            {
                return targetTransform;
            }
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
            InteractText.gameObject.SetActive(true);
            if (target.name == "Planter")
            {
                SelectText.gameObject.SetActive(true);
            }
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
            Debug.Log(target);
        }
        DisplayMouseTarget();
    }
}
