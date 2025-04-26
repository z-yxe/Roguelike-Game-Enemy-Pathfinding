using Unity.VisualScripting;
using UnityEngine;

public class DungeonExit : MonoBehaviour
{
    private float FinishTimer = 15;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
