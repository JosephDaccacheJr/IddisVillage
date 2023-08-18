using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_InteractSpot : MonoBehaviour
{
    public Building_Plot plot;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            switch(plot.buildingType)
            {
                case Building_Plot.type.empty:
                    GameManager.instance.panelBuilding.SetActive(true);
                    GameManager.instance.setCurrentPlot(plot);
                    break;
            }

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            switch (plot.buildingType)
            {
                case Building_Plot.type.apothecary:
                    plot.takePotion();
                    break;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.instance.panelBuilding.SetActive(false);
            GameManager.instance.setCurrentPlot(null);
        }
    }
}
