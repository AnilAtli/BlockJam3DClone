using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoxCharacterPair
{
    public Transform box; 

    public GameObject character; 
}

public class BoxManager : MonoBehaviour
{
    public List<BoxCharacterPair> boxCharacterPairs = new List<BoxCharacterPair>(); 
    public List<bool> boxStatus; 
 

    private void Awake()
    {
        boxStatus = new List<bool>();

        foreach (BoxCharacterPair pair in boxCharacterPairs)
        {
            boxStatus.Add(false); 

            pair.character = null; 
        }
    }

    public Transform GetNextAvailableBox()
    {
        for (int i = 0; i < boxStatus.Count; i++)
        {
            if (!boxStatus[i]) 
            {
                boxStatus[i] = true; 
                return boxCharacterPairs[i].box;
            }
        }

        return null; 
    }

    public void AssignCharacterToBox(Transform box, GameObject character)
    {
        for (int i = 0; i < boxCharacterPairs.Count; i++)
        {
            if (boxCharacterPairs[i].box == box)
            {
                boxCharacterPairs[i].character = character; 
                boxStatus[i] = true; 
                break;
            }
        }
    }

    public void FreeBox(Transform box)
    {
        for (int i = 0; i < boxCharacterPairs.Count; i++)
        {
            if (boxCharacterPairs[i].box == box)
            {
                boxCharacterPairs[i].character = null; 
                boxStatus[i] = false; 
                break;
            }
        }
    }

    public GameObject GetCharacterInBox(Transform box)
    {
        foreach (BoxCharacterPair pair in boxCharacterPairs)
        {
            if (pair.box == box)
            {
                return pair.character;
            }
        }

        return null; 
    }

    public System.Type GetCharacterTypeInBox(Transform box)
    {
        GameObject character = GetCharacterInBox(box);
        if (character != null)
        {
            ICharacter characterScript = character.GetComponent<ICharacter>();
            if (characterScript != null)
            {
                return characterScript.GetType(); 
            }
        }

        return null; 
    }

    public void RemoveConsecutiveSameTypeCharacters()
    {
        int count = 1;
        System.Type previousType = null;

        for (int i = 0; i < boxCharacterPairs.Count; i++)
        {
            BoxCharacterPair pair = boxCharacterPairs[i];
            if (pair.character != null)
            {
                ICharacter characterScript = pair.character.GetComponent<ICharacter>();
                if (characterScript != null)
                {
                    System.Type currentType = characterScript.GetType();

                    if (currentType == previousType)
                    {
                        count++;
                        if (count == 3) 
                        {
                         
                            
                            for (int j = i; j > i - 3; j--)
                            {
                                
                                Destroy(boxCharacterPairs[j].character);
                                boxCharacterPairs[j].character = null;
                                boxStatus[j] = false;
                            }

                           
                            
                            count = 1;
                            previousType = null;
                            continue;
                        }
                    }
                    else
                    {
                        count = 1;
                        previousType = currentType;
                    }
                }
                else
                {
                    count = 1;
                    previousType = null;
                }
            }
            else
            {
                count = 1;
                previousType = null;
            }
        }
    }


    public Transform GetClosestAvailableBoxBeforeIndex(int currentIndex)
    {
        
        for (int i = currentIndex - 1; i >= 0; i--)
        {
            if (!boxStatus[i])
            {
                boxStatus[i] = true; 
                return boxCharacterPairs[i].box;
            }
        }
        return null; 
    }

    


    private void OnDrawGizmos()
    {
        if (boxCharacterPairs == null || boxCharacterPairs.Count == 0)
            return;

        for (int i = 0; i < boxCharacterPairs.Count; i++)
        {
            if (boxCharacterPairs[i].box != null)
            {
              
                Gizmos.color = boxStatus != null && boxStatus.Count > i && boxStatus[i]
                    ? Color.red 
                    : Color.green; 

               
                Gizmos.DrawCube(boxCharacterPairs[i].box.position, Vector3.one * 0.5f);

                
                if (boxCharacterPairs[i].character != null)
                {
                    UnityEditor.Handles.Label(
                        boxCharacterPairs[i].box.position + Vector3.up * 0.5f,
                        boxCharacterPairs[i].character.name
                    );
                }
            }
        }
    }


}

