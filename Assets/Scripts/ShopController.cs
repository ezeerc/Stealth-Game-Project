using System.Collections;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [SerializeField] private ConfirmationPanel confirmationPanel;
    [SerializeField] private ShopBuilder shopBuilder;
    [SerializeField] private float time;


    private void Start()
    {
        ConnectToShop();
    }

    private void HandleItemClickedBuy(ItemDTO itemToBuy, ItemUI itemUI)
    {
        confirmationPanel.Show(
            () => ConfirmPurchase(itemToBuy, itemUI)
        );
    }

    private void ConfirmPurchase(ItemDTO itemToBuy, ItemUI itemUI)
    {
        shopBuilder.OnItemBought(itemToBuy, itemUI);
    }

    public void ConnectToShop()
    {
        StartCoroutine(StartWaitCoroutine());
    }

    IEnumerator StartWaitCoroutine()
    {
        yield return new WaitForSeconds(time);
        foreach (Transform child in shopBuilder.shopParent)
        {
            var itemUI = child.GetComponent<ItemUI>();
            if (itemUI != null)
            {
                itemUI.onItemClickedBuy += HandleItemClickedBuy;
            }
        }
    }
}