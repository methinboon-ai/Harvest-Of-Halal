using UnityEngine;

public class Player : MonoBehaviour
{
    

    void Start()
    {
        
    }

    void Update()
    {
        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Create a ray from the camera, through the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Perform the raycast
            if (Physics.Raycast(ray, out hit))
            {
                // If the ray hits an object, get its Transform
                Transform targetTransform = hit.transform;
                Debug.Log("Mouse clicked on object: " + targetTransform.name);

                // You can now use targetTransform for various actions,
                // such as moving towards it, interacting with it, etc.
            }
            else
            {
                Debug.Log("Mouse clicked on empty space.");
            }
        }
    }
}
