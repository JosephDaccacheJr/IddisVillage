using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Plot : MonoBehaviour
{
    public enum type { empty,house,blacksmith, apothecary }
    public bool isBuilt { get; private set; }

    public type buildingType;

    [Header("References")]
    public GameObject buildingHouse;
    public GameObject buildingBlacksmith;
    public GameObject buildingApothecary;
    public GameObject healingPotion;

    [Header("Aporapothecary Controls")]
    public float setPotionTimer;
    public float potionTimer;

    private void Update()
    {
        if (buildingType == type.apothecary)
        {
            potionTimer -= Time.deltaTime;
            if (potionTimer <= 0 && !healingPotion.activeInHierarchy)
            {
                healingPotion.SetActive(true);
            }
        }
    }

    public void selectHouse()
    {
        buildingType = type.house;
        buildOnPlot(buildingHouse);
    }

    public void selectBlacksmith()
    {
        buildingType = type.blacksmith;
        buildOnPlot(buildingBlacksmith);
    }
    public void selectApothecary()
    {
        buildingType = type.apothecary;
        buildOnPlot(buildingApothecary);
    }


    void buildOnPlot(GameObject building)
    {
        if (isBuilt) return;
        building.SetActive(true);
        isBuilt = true;
        GameManager.instance.addBuilding(gameObject);
        switch (buildingType)
        {
            case type.house:
                GameManager.instance.changeVillageRating(100);
                break;
            case type.blacksmith:
                GameManager.instance.playerDamage++;
                GameManager.instance.changeVillageRating(10);
                break;
            case type.apothecary:
                GameManager.instance.changeVillageRating(10);
                break;
        }
    }
    
    public void takePotion()
    {
        if (potionTimer <= 0)
        {
            potionTimer = setPotionTimer;
            healingPotion.SetActive(false);
            GameManager.instance.setHealth(GameManager.instance.playerMaxHealth);
        }
    }
}
