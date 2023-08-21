using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
public class LifeGauge : MonoBehaviour
{
    public Image fillImage;
    Transform unitTransform;
    private RectTransform _parentRectTransform;
    Unit targetUnit;
    private Camera _camera;
        // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Initialize(Unit unit, RectTransform parentRectTransform, Camera camera)
    {
        _parentRectTransform = parentRectTransform;
        _camera = camera;
        targetUnit = unit;
    }
    // Update is called once per frame
    void Update()
    {
        
        fillImage.fillAmount = ((float)targetUnit.hitPoint)/(float)targetUnit.maxHP;

        var screenPoint = _camera.WorldToScreenPoint(targetUnit.transform.position);
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _parentRectTransform, screenPoint, null, out localPoint
        );
        transform.localPosition = localPoint + new Vector2(0, 120);
    }
}
