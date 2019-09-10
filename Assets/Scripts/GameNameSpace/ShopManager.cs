using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BuyChips(string amount)
    {
        OrderMessage orderMessage = new OrderMessage()
        {
            email = FB_Handler.instance.SavedEmail,
            name = FB_Handler.instance.SavedUsername,
            amount = amount
        };

        string orderPostData = JsonConvert.SerializeObject(orderMessage);

        WebRequestManager.HttpCreateCoinsOrderID(orderPostData, (OrderResponse orderResponse) =>
        {
            //Debug.Log("Order ID is " + orderResponse.data.id);

            //Debug.Log("Opening WebView...");
            string Url = "http://languagelive.xyz/casino/payment.php?";
            Url += "orderId=" + orderResponse.data.id + "&" + "amount=" + orderResponse.data.amount;

            WebViewHandler.Instance.LoadWebView(Url);
        });
    }
}
