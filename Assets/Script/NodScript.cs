using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class NodScript : MonoBehaviour
{
    DragAndDropUI dragAndDropUI;
    RectTransform rectTransform;
    public float UpPercent = 0.1f;
    public float DownPercent = 0.9f;
    public float MovementsToNod = 4f; // Nombre de mouvements nécessaires pour nod
    public float MovementResetTime = 3f; // Temps pour que le nombre de mouvements reset (si aucun n'est effectué)
    public bool IsNodding = false; // Nod
    [SerializeField] private bool isDown = false; // Tete basse
    [SerializeField] private float upY; // Pos Y où la tete sera haute
    [SerializeField] private float downY; // Pos Y où la tete sera basse
    [SerializeField] private float movements = 0f; // Augmente à chaque fois que la tete passe de haute à basse et inversement
    [SerializeField] private float lastMoveTime = 0f; // Temps écoulé depuis le dernier mouvement
    void Start()
    {
        dragAndDropUI = GetComponent<DragAndDropUI>();
        rectTransform = GetComponent<RectTransform>();
        upY = dragAndDropUI.InitialYPos - dragAndDropUI.Amplitude * UpPercent;
        downY = dragAndDropUI.InitialYPos - dragAndDropUI.Amplitude * DownPercent;
    }

    void Update()
    {
        if (!isDown && rectTransform.anchoredPosition.y <= downY) // Ouvrir
        {
            movements += 1f;
            isDown = true;
            lastMoveTime = Time.time;
        }
        if (isDown && rectTransform.anchoredPosition.y >= upY) // Fermer
        {
            movements += 1f;
            isDown = false;
            lastMoveTime = Time.time;
        }
        if (movements >= MovementsToNod) // Rire
        {
            IsNodding = true;
        }
        if (Time.time - lastMoveTime > MovementResetTime) // Reset mouvements
        {
            movements = 0f;
            IsNodding = false;
        }
    }
}
