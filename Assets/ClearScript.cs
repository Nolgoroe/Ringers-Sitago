using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClearScript : MonoBehaviour
{
    public UnityEvent interactEvent;

    // Start is called before the first frame update
    void Start()
    {
        interactEvent.AddListener(() => UIManager.instance.RestartCurrentLevel());
    }

    private void OnMouseDown()
    {
        interactEvent.Invoke();
    }
}
