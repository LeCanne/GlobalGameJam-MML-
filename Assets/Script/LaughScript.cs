using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LaughScript : MonoBehaviour
{
    DragAndDropUI dragAndDropUI;
    RectTransform rectTransform;
    public float ClosedPercent = 0.1f; // La bouche se fermera si elle est ouverte � 10% (valeur modifiable)
    public float OpenPercent = 0.9f; // La bouche s'ouvrira si elle est ouverte � 90% (valeur modifiable)
    public float MovementsToLaugh = 6f; // Nombre de mouvements n�cessaires pour rire
    public float MovementResetTime = 3f; // Temps pour que le nombre de mouvements reset (si aucun n'est effectu�)
    public AudioSource audioSource;
    public AudioClip audioClip;
    public Vector2 PitchRange = new Vector2(0.95f, 1.05f); // Variation du pitch du rire
    public bool IsLaughing = false; // Rire
    [SerializeField] private bool isOpen = false; // Bouche ouverte
    [SerializeField] private float closedY; // Pos Y o� la bouche se fermera
    [SerializeField] private float openY; // Pos Y o� la bouche s'ouvrira
    [SerializeField] private float movements = 0f; // Augmente � chaque fois que la bouche passe d'ouverte � ferm�e et inversement
    [SerializeField] private float lastMoveTime = 0f; // Temps �coul� depuis le dernier mouvement
    [SerializeField] private BlaBlaScript dialScript;

    void Start()
    {
        dragAndDropUI = GetComponent<DragAndDropUI>();
        rectTransform = GetComponent<RectTransform>();
        closedY = dragAndDropUI.InitialYPos - dragAndDropUI.Amplitude * ClosedPercent;
        openY = dragAndDropUI.InitialYPos - dragAndDropUI.Amplitude * OpenPercent;
    }

    void Update()
    {
        if (!isOpen && rectTransform.anchoredPosition.y <= openY) // Ouvrir
        {
            movements += 1f;
            isOpen = true;
            lastMoveTime = Time.time;
            audioSource.pitch = Random.Range(PitchRange.x, PitchRange.y);
            audioSource.PlayOneShot(audioClip);
        }
        if (isOpen && rectTransform.anchoredPosition.y >= closedY) // Fermer
        {
            isOpen = false;
            lastMoveTime = Time.time;
        }
        if (movements >= MovementsToLaugh) // Rire
        {
            IsLaughing = true;
        }
        if (Time.time - lastMoveTime > MovementResetTime) // Reset mouvements
        {
            movements = 0f;
            IsLaughing = false;
        }
        if (IsLaughing)
        {
            dialScript.OnExpression(true);
            movements = 0f;
            IsLaughing = false;
        }
    }
}
