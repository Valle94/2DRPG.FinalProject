using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveInventory : MonoBehaviour
{
    int activeSlotIndexNum = 0;

    PlayerControls playerControls;

    void Awake()
    {
        playerControls = new PlayerControls();
    }

    void Start()
    {
        // Once again we're subscribing to an action defined in our player controls action map. 
        // However, because in this case we have a different value for each keypress, we're actually
        // passing in a value 'ctx' when we call 'ToggleActiveSlot'
        playerControls.Inventory.Keyboard.performed += ctx => ToggleActiveSlot((int)ctx.ReadValue<float>());

        // Instantiate item slot 0 (Sword) as our default weapon on game start
        ToggleActiveHighlight(0);
    }

    void OnEnable()
    {
        playerControls.Enable();
    }

    // This method toggles the active slot in our inventory.
    void ToggleActiveSlot(int numValue)
    {
        // Passing in numvalue-1 because our inventory indexes are 0-2, not 1-3 like our keyboard buttons
        ToggleActiveHighlight(numValue - 1);
    }

    // We have a separate method for turning on/off the inventory highlight because here we 
    // just need to handle the visual for the active slot, not the actual item in the inventory.
    void ToggleActiveHighlight(int indexNum)
    {
        activeSlotIndexNum = indexNum;

        // For each of our inventory slots we first turn off all highlights
        foreach (Transform inventorySlot in this.transform)
        {
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }

        // Then we turn on only the highlight of the selected inventory slot.
        this.transform.GetChild(indexNum).GetChild(0).gameObject.SetActive(true);

        // Calling our method to actually change weapons
        ChangeActiveWeapon();
    }

    // While the previous methods handle changing the active slot index and 
    // highlighting the active slot, this method changes the actual weapon 
    // we have equipped in logic and instantiates it on our player. 
    void ChangeActiveWeapon()
    {
        // If we have a currently active weapon, destroy it
        if (ActiveWeapon.Instance.CurrentActiveWeapon != null)
        {
            Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
        }

        // This chain of getter methods is just searching down levels in our hierarchy
        // until we find the weapon prefab defined by the active slot index.
        Transform childTransform = transform.GetChild(activeSlotIndexNum);
        InventorySlot inventorySlot = childTransform.GetComponentInChildren<InventorySlot>();
        WeaponInfo weaponInfo = inventorySlot.GetWeaponInfo();
        GameObject weaponToSpawn = weaponInfo.weaponPrefab;

        // Instantiate the weapon we've chosen on our active weapon position
        GameObject newWeapon = Instantiate(weaponToSpawn, ActiveWeapon.Instance.transform.position, Quaternion.identity);

        // Reset the rotation of the instantiated object
        ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, 0);
        
        // Set the instantiated object as a child of the active weapon instance in the hierarchy
        newWeapon.transform.parent = ActiveWeapon.Instance.transform;

        // Finally, we call our 'NewWeapon' method which our selected weapon as the current active weapon. 
        ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
    }
}
