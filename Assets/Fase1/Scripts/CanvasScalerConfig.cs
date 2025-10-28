using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class CanvasScalerConfig : MonoBehaviour
{
    [Header("Resolução base de referência (a que você configurou o layout)")]
    public Vector2 referenceResolution = new Vector2(1280, 720);

    [Header("Modo de correspondência (0 = Largura, 1 = Altura, 0.5 = Automático)")]
    [Range(0f, 1f)] public float matchWidthOrHeight = 0.5f;

    private void Awake()
    {
        CanvasScaler scaler = GetComponent<CanvasScaler>();

        // Garante que o modo de escala está configurado corretamente
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = referenceResolution;
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = matchWidthOrHeight;

        Debug.Log($"CanvasScaler configurado: {referenceResolution.x}x{referenceResolution.y}, match {matchWidthOrHeight}");
    }
}
