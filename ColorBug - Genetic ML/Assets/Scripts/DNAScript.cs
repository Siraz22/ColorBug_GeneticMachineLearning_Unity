using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DNAScript : MonoBehaviour
{
    
    //Colors
    public float Red;
    public float Green;
    public float Blue;

    public float SizeMultiplier;

    private float timeOfDeath = 0;

    public float GetDeathTime()
    {
        return timeOfDeath;
    }

    SpriteRenderer bugSprite;
    BoxCollider2D bugCollider;

    private void Start()
    {
        bugSprite = gameObject.GetComponent<SpriteRenderer>();
        bugSprite.color = new Color(Red, Green, Blue);
        transform.localScale *= SizeMultiplier;

        bugCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    private void OnMouseDown()
    {
        bugCollider.enabled = false;
        bugSprite.enabled = false;

        timeOfDeath = BreederManager.BreederInstance.ReturnTime();
    }
}
