using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeGaugeContainer : MonoBehaviour
{
    public static LifeGaugeContainer Instance
    {
        get { return _instance; }
    }

    private static LifeGaugeContainer _instance;
    [SerializeField]private Camera mainCamera;
    public LifeGauge lifeGaugePrefab;
    private RectTransform rectTransform;
    // Start is called before the first frame update
    void Awake()
    {
        if(null != _instance) throw new Exception("LifeBarContainer instance already exists");
        _instance = this;
        rectTransform = GetComponent<RectTransform>();
    }
    public void Add(Unit unit)
    {
        var lifeGauge = Instantiate(lifeGaugePrefab, transform);
        lifeGauge.Initialize(unit, rectTransform, mainCamera);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
