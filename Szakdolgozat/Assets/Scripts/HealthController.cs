using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public GameObject heartContainer;
    private float fillValue;

    void Update()
    {
        fillValue = (float)GameController.Health;
        fillValue = fillValue / GameController.MaxHealth;
        heartContainer.GetComponent<Image>().fillAmount = fillValue;
    }
}
