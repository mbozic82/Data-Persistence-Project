using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Brick : MonoBehaviour
{
    public UnityEvent<int> onDestroyed;
    
    public int PointValue;
    private int points;

    void Start()
    {
        var renderer = GetComponentInChildren<Renderer>();

        MaterialPropertyBlock block = new MaterialPropertyBlock();
        switch (PointValue)
        {
            case 1 :
                block.SetColor("_BaseColor", GameManager.Instance.Point1Color);
                break;
            case 2:
                block.SetColor("_BaseColor", GameManager.Instance.Point2Color);
                break;
            case 3:
                block.SetColor("_BaseColor", GameManager.Instance.Point3Color);
                break;
            default:
                block.SetColor("_BaseColor", GameManager.Instance.Point1Color);
                break;
        }
        renderer.SetPropertyBlock(block);

        if (GameManager.Instance.PlayerManager.Difficulty == 0)
        {
            points = 1;
        }
        else
        {
            points = PointValue;
            PointValue *= 2;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        points--;
        if (points == 0)
        {
            onDestroyed.Invoke(PointValue);
            //slight delay to be sure the ball have time to bounce
            Destroy(gameObject, 0.2f);
        }
    }
}
