using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NotificationSystem : MonoBehaviour
{
    public static NotificationSystem Instance;

    [Header("Controle de Notificação")]
    public GameObject notificationsArea;
    public GameObject notificationPrefab;
    public int maxNotifications;

    [SerializeField]
    private List<Notification> notifications;

    void Awake()
    {
        Instance = (Instance == null) ? this : Instance;
    }

    public void CallNotification(InventoryItemData itemData)
    {
        GameObject notificationGO = Instantiate(notificationPrefab, notificationsArea.transform);
        Notification notification = notificationGO.GetComponent<Notification>();
        notifications.Add(notification);
        int index = notifications.Count - 1;
        notification.Notify(index, itemData);
        if (notifications.Count == maxNotifications)
        {
            UpdateNotifications();
        }
    }

    private void UpdateNotifications()
    {
        notifications[0].RemoveNotification();
        notifications.RemoveAt(0);
        foreach (Notification notification in notifications)
        {
            notification.UpdateIndex();
        }
    }

    public void CloseNotification(Notification notification)
    {
        notifications.Remove(notification); 
        foreach (Notification noti in notifications)
        {
            notification.UpdateIndex();
        }
    }
}
