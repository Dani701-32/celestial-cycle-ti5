using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Notification : MonoBehaviour
{
    [SerializeField]
    private int currentIndex = 0;
    [SerializeField]
    private float timeDisplay = 15f;
    public TextMeshProUGUI textNotification;
    InventoryItemData itemData;

    public void Notify(int currentIndex, InventoryItemData itemData)
    {
        Debug.Log(itemData.displayName);
        this.currentIndex = currentIndex;
        this.itemData = itemData;
        textNotification.text = $"Coletou: {itemData.displayName} - {currentIndex}";
    }
    private void Update() {
        timeDisplay-= Time.deltaTime; 
        if(timeDisplay <= 0){
            NotificationSystem.Instance.CloseNotification(this);
            RemoveNotification();
        }
    }

    public void UpdateIndex()
    {
        currentIndex--;
    }

    public void RemoveNotification()
    {
        Destroy(this.gameObject);
    }
}
