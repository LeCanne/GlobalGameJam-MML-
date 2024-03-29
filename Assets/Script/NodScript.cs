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
    public float MovementsToNod = 4f; // Nombre de mouvements n�cessaires pour nod
    public float MovementResetTime = 3f; // Temps pour que le nombre de mouvements reset (si aucun n'est effectu�)
    public AudioSource audioSource;
    public AudioClip audioClip;
    public Vector2 PitchRange = new Vector2(0.95f, 1.05f); // Variation du pitch du nod
    public bool IsNodding = false; // Nod
    [SerializeField] private bool isDown = false; // Tete basse
    [SerializeField] private float upY; // Pos Y o� la tete sera haute
    [SerializeField] private float downY; // Pos Y o� la tete sera basse
    [SerializeField] private float movements = 0f; // Augmente � chaque fois que la tete passe de haute � basse et inversement
    [SerializeField] private float lastMoveTime = 0f; // Temps �coul� depuis le dernier mouvement
    [SerializeField] private BlaBlaScript dial;
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
            audioSource.pitch = Random.Range(PitchRange.x, PitchRange.y);
            audioSource.PlayOneShot(audioClip);
            dial.OnExpression(false);
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
