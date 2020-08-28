using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositionUI : MonoBehaviour
{
    // Start is called before the first frame update
    public TMPro.TextMeshProUGUI pos;
    public TMPro.TextMeshProUGUI driver;
    public Image color;
    public Image bg;

    public TMPro.TMP_FontAsset blackFont;
    public TMPro.TMP_FontAsset whiteFont;

    private const string bgDarkColorHex = "#33333399";
    private const string bgLightColorHex = "#77777799";

    public void Initialize(int carPos, string driverName, Color carColor) 
    {
        Color dark;
        ColorUtility.TryParseHtmlString(bgDarkColorHex, out dark);

        Color light;
        ColorUtility.TryParseHtmlString(bgLightColorHex, out light);

        pos.SetText(carPos.ToString("00"));
        bool isOdd = carPos % 2 == 0;
        bg.color = isOdd ? dark : light;
        driver.font = isOdd ? whiteFont : blackFont;
        setNewDriverInfo(driverName, carColor);
    }

    public void setNewDriverInfo(string driverName, Color carColor)
    {
        driver.SetText(driverName);
        color.color = carColor;
    }
}
