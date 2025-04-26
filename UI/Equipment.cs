using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    public GameObject gunUI;
    public GameObject swordUI;

    // ( RAPI )

    // Display Item by nama weapon ( !!! Keep Update kalo ada weapon baru !!! )
    public void DisplayWeapon(string currentWeapon)
    {
        currentWeapon += "UI";
        Debug.Log(currentWeapon + " Equiped");
        
        // Aktifin weapon dalam equipmentUI
        if (currentWeapon == swordUI.name) swordUI.SetActive(true); // sword
        if (currentWeapon == gunUI.name) gunUI.SetActive(true);     // gun

    }

    // Disable semua item di equipmentUI
    public void ClearDisplay()
    {
        swordUI.SetActive(false);
        gunUI.SetActive(false);
    }
}
