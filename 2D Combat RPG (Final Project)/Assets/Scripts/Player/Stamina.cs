using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : Singleton<Stamina>
{
    public int CurrentStamina { get; private set; }

    [SerializeField] Sprite fullStaminaImage, EmptyStaminaImage;
    [SerializeField] int timeBetweenStaminaRefesh = 3;

    Transform staminaContainer;
    int startingStamina = 3;
    int maxStamina;
    const string STAMINA_CONTAINER_TEXT = "Stamina Container";

    protected override void Awake()
    {
        base.Awake();

        maxStamina = startingStamina;
        CurrentStamina = startingStamina;
    }

    void Start()
    {
        staminaContainer = GameObject.Find(STAMINA_CONTAINER_TEXT).transform;
    }

    // When called by our dash function in the player controller
    // this method uses up our current stamina
    public void UseStamina()
    {
        CurrentStamina--;
        UpdateStaminaImages();
        StopAllCoroutines();
        StartCoroutine(RefreshStaminaRoutine());
    }

    // When called, this method restores stamina
    public void RefreshStamina()
    {
        if (CurrentStamina < maxStamina && !PlayerHealth.Instance.IsDead)
        {
            CurrentStamina++;
        }
        UpdateStaminaImages();
    }

    public void ReplenishStaminaOnDeath()
    {
        CurrentStamina = maxStamina;
        UpdateStaminaImages();
    }

    // This coroutine will refresh our stamina automatically 
    // at a regular interval
    IEnumerator RefreshStaminaRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenStaminaRefesh);
            RefreshStamina();
        }
    }

    // Using a for loop, this method updates the stamina images in the UI
    void UpdateStaminaImages()
    {
        for (int i = 0; i < maxStamina; i++)
        {
            Transform child = staminaContainer.GetChild(i);
            Image image = child?.GetComponent<Image>();

            if (i <= CurrentStamina - 1)
            {
                image.sprite = fullStaminaImage;
            }
            else
            {
                image.sprite = EmptyStaminaImage;
            }
        }
    }
}
