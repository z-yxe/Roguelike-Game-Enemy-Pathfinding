using Unity.VisualScripting;
using UnityEngine;

public class DungeonExit : MonoBehaviour
{
    private float FinishTimer = 15;

    // Runs at start
    void Start()
    {
        
    }

    // Runs every frame
    void Update()
    {
        
    }

    // Starts finish countdown
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player") 
        {
            FinishTimer--;

            if (FinishTimer == 0)
            { 
                Debug.Log("Finish!");
                FinishTimer = 15;
            }
        }
    }
}
