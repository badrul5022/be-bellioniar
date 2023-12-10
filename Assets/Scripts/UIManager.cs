using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject propertyCanvas;
    public TextMeshProUGUI propertyName;
    public TextMeshProUGUI propertyRent;
    public Property activeProperty;

    void Start()
    {
        instance = this;
        propertyCanvas.SetActive(false);
    }

    public void showPropertyInfo(Property property)
    {
        if(NetworkGameManager.instance.localPlayer.cash < property.propertyPrice)
        {
            NetworkGameManager.instance.localPlayer.SetBankRuppted();
            return;
        }
        propertyCanvas.SetActive(true);
        float floor_price = property.propertyPrice;
        float rent = (floor_price / 100) * 20;
        int _rent = Mathf.FloorToInt(rent);
        this.propertyRent.text = $"{_rent}$";
        this.propertyName.text = property.propertyTitle;
        activeProperty = property;
        NetworkGameManager.instance.localPlayer.stopped = true;
    }
    public void PurchaseProperty()
    {
        Waypoints.waypoint[NetworkGameManager.instance.localPlayer.currentRequestedProperty].PurchaseProperty(NetworkGameManager.instance.localPlayer.id);
        propertyCanvas.SetActive(false);
        DiceController.instance.Transfer();
        NetworkGameManager.instance.localPlayer.stopped = false;
        NetworkGameManager.instance.localPlayer.DeductCash(activeProperty.propertyPrice);
    }
}
