using UnityEngine;

public class MoveController : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public float rotationSpeed = 10f; 
    private Transform targetBox; 
    private BoxManager boxManager;
    private bool isMoved = false;
    private Animator animator;
    private int idleAnim = Animator.StringToHash("idle");
    private int runAnim = Animator.StringToHash("Run");
    private Transform previousBox;
    private void Start()
    {
        animator = GetComponent<Animator>();
        boxManager = FindObjectOfType<BoxManager>();
    }

    public void MoveToBox(Transform box)
    {
        if (targetBox == box) return; 

     
        if (previousBox != null)
        {
            boxManager.FreeBox(previousBox);
        }

        previousBox = targetBox; 
        targetBox = box;         
    }

    private void Update()
    {
        if (isMoved) return;

        if (targetBox != null)
        {
          
            transform.position = Vector3.MoveTowards(transform.position, targetBox.position, moveSpeed * Time.deltaTime);

           
            Vector3 direction = (targetBox.position - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            
            animator.Play(runAnim);

           
            if (Vector3.Distance(transform.position, targetBox.position) < 0.1f)
            {
               
                boxManager.AssignCharacterToBox(targetBox, gameObject);

                
                if (previousBox != null)
                {
                    boxManager.FreeBox(previousBox);
                }

               
                int currentIndex = boxManager.boxCharacterPairs.FindIndex(pair => pair.box == targetBox);
                Transform closestBox = boxManager.GetClosestAvailableBoxBeforeIndex(currentIndex);

                if (closestBox != null)
                {
                    MoveToBox(closestBox); 
                    return;
                }

               
                boxManager.RemoveConsecutiveSameTypeCharacters();
                animator.Play(idleAnim);
                
                isMoved = true;
            }

        }
    }


}
