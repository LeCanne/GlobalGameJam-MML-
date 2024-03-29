using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragAndDropUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Canvas canvas;
    RectTransform rectTransform;
    public float Amplitude = 50f; // Distance max sur laquelle la bouche peut �tre d�plac�e*
    public float Force = 0.1f;
    [HideInInspector] public float InitialYPos;
    private float mouseYOffset; // Diff entre positions y de la souris et de la bouche (�vite que la bouche se centre sur la souris)
    private bool dragging = false;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        InitialYPos = rectTransform.anchoredPosition.y;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        mouseYOffset = Input.mousePosition.y / canvas.scaleFactor - rectTransform.anchoredPosition.y;
        dragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, Mathf.Clamp(((Input.mousePosition.y / canvas.scaleFactor) - mouseYOffset) * Force,InitialYPos - Amplitude,InitialYPos));
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
    }

    void Update()
    {
        if (!dragging && transform.position.y != InitialYPos) // La bouche reprend sa position d'origine lorsqu'elle n'est pas tir�e
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, Mathf.Clamp(rectTransform.anchoredPosition.y + 0.1f, InitialYPos - Amplitude, InitialYPos));
        }
    }
}
