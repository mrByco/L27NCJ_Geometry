using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HardnessText : MonoBehaviour
{

    public TextMeshProUGUI text;
    public MapGenerator map;

    void Start()
    {
        this.text = this.GetComponent<TextMeshProUGUI>();
        this.map = GameObject.FindObjectsByType(typeof(MapGenerator), FindObjectsSortMode.None).FirstOrDefault() as MapGenerator;
    }

    void Update()
    {
        this.text.text = this.map.GetHardness().ToString();
    }
}
