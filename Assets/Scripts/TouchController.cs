using UnityEngine;

public class TouchController : MonoBehaviour
{
    private BoxManager boxManager;

    private void Start()
    {
        boxManager = FindObjectOfType<BoxManager>();
    }

    private void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
           
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                
                ICharacter character = hit.collider.GetComponent<ICharacter>();
                if (character != null)
                {
                    MoveController moveController = hit.collider.GetComponent<MoveController>();
                    if (moveController != null)
                    {
                        
                        Transform nextBox = boxManager.GetNextAvailableBox();
                        if (nextBox != null)
                        {
                            moveController.MoveToBox(nextBox);
                        }
                    }
                }
            }
        }
    }
}
