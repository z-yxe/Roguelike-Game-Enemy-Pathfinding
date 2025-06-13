using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    public GameObject gunUI;
    public GameObject swordUI;
    
    // Shows weapon UI
    public void DisplayWeapon(string currentWeapon)
    {
        currentWeapon += "UI";
        Debug.Log(currentWeapon + " Equiped");
        
        if (currentWeapon == swordUI.name) swordUI.SetActive(true);
        if (currentWeapon == gunUI.name) gunUI.SetActive(true);
    }

    // Hides weapon UI
    public void ClearDisplay()
    {
        swordUI.SetActive(false);
        gunUI.SetActive(false);
    }
}
