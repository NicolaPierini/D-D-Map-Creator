using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.QrCode;
using UnityEngine.UI;

public class QRGenerator : MonoBehaviour
{
    public RawImage qrContainer;
    public Image qrPanelContainer;
    void Start()
    {
        SetQRVisibility(false);
    }
    private Color32[] Encode(string qrText, int width, int height)
    {
        BarcodeWriter writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };

        return writer.Write(qrText);
    }

    public void EncodeTextToQR(string text)
    {
        Texture2D encodedTexture = new Texture2D(256, 256);

        Color32[] encoded = Encode(text, encodedTexture.width, encodedTexture.height);
        encodedTexture.SetPixels32(encoded);
        encodedTexture.Apply();

        qrContainer.texture = encodedTexture;
    }

    public void SetQRVisibility(bool visible)
    {
        qrContainer.enabled = visible;
        qrPanelContainer.enabled = visible;
    }
}
