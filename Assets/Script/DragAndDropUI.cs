using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragAndDropUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public float Amplitude = 50f; // Distance max sur laquelle la bouche peut être déplacée
    [HideInInspector] public float InitialYPos;
    private float mouseYOffset; // Diff entre positions y de la souris et de la bouche (évite que la bouche se centre sur la souris)
    private bool dragging = false;
    public void OnBeginDrag(PointerEventData eventData)
    {
        mouseYOffset = Input.mousePosition.y - transform.position.y;
        dragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = new Vector2(transform.position.x, Mathf.Clamp(Input.mousePosition.y - mouseYOffset,InitialYPos - Amplitude,InitialYPos));
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
    }

    void Start()
    {
        InitialYPos = transform.position.y;
    }

    void Update()
    {
        if (!dragging && transform.position.y != InitialYPos) // La bouche reprend sa position d'origine lorsqu'elle n'est pas tirée
        {
            transform.position = new Vector2(transform.position.x, Mathf.Clamp(transform.position.y + 2, InitialYPos - Amplitude, InitialYPos));
        }
    }
}
