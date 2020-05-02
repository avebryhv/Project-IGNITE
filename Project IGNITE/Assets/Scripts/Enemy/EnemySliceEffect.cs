using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySliceEffect : MonoBehaviour
{
    Sprite sourceSprite;
    Bounds sourceBounds;
    public float cutAngle;
    Vector2 direction;
    public float spacingMagnitude;

    Sprite part1Sprite;
    Sprite part2Sprite;

    public bool moving;
    public float moveSpeed;

    public float fadeTime;
    float fadeStep;

    [Header("Object Links")]
    public SpriteMask part1Mask;
    public SpriteRenderer part1Renderer;
    public SpriteMask part2Mask;
    public SpriteRenderer part2Renderer;

    // Start is called before the first frame update
    void Start()
    {
        SetSprites(GetComponent<SpriteRenderer>());
        SetSpriteBounds(GetComponent<SpriteRenderer>());
        SetMaskPosition();
        moving = true;
        fadeStep = 1.0f / (fadeTime * (1.0f / 60.0f));
        cutAngle = Random.Range(90, 150);
        CreateSliceEffect(GetComponent<SpriteRenderer>(), cutAngle, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (moving) //Move the two segments away from each other
        {
            part1Renderer.transform.localPosition += new Vector3(-direction.x, -direction.y) * moveSpeed;
            part2Renderer.transform.localPosition += new Vector3(direction.x, direction.y) * moveSpeed;
        }

        Color currentCol = part1Renderer.color;
        //Color nextCol = new Color(currentCol.r, currentCol.b, currentCol.g, currentCol.a - fadeStep);
        //part1Renderer.color = nextCol;
        //part2Renderer.color = nextCol;

        float curAlpha = part1Renderer.color.a;
        Color newCol = new Color(currentCol.r, currentCol.g, currentCol.b, curAlpha - (1.0f / (60.0f * fadeTime)));
        part1Renderer.color = newCol;
        part2Renderer.color = newCol;
        if (curAlpha <= 0)
        {
            Destroy(gameObject);
        }

    }

    public void CreateSliceEffect(SpriteRenderer spr, float angle, float spacing)
    {
        SetSprites(spr);
        SetSpriteBounds(spr);
        cutAngle = angle;
        spacingMagnitude = spacing;
        RotateMasks();
        moving = true;
    }

    //Sets sprites of each component
    public void SetSprites(SpriteRenderer spr)
    {
        sourceSprite = spr.sprite;
        part1Sprite = spr.sprite;
        part2Sprite = spr.sprite;
        part1Renderer.sprite = part1Sprite;
        part2Renderer.sprite = part2Sprite;
        //part1Mask.sprite = part1Sprite;
        //part2Mask.sprite = part2Sprite;
    }

    //Scales each part to the origin sprite
    void SetSpriteBounds(SpriteRenderer spr)
    {
        sourceBounds = spr.bounds;
        part1Mask.transform.localScale = new Vector3(sourceBounds.size.y * 2, sourceBounds.size.y * 2);
        part2Mask.transform.localScale = new Vector3(sourceBounds.size.y * 2, sourceBounds.size.y * 2);
    }

    //Positions sprite masks
    void SetMaskPosition()
    {
        //part1Mask.transform.position += new Vector3(sourceBounds.size.x / 2, 0, 0);
        part1Mask.transform.position += new Vector3(part1Mask.transform.localScale.x / 2, 0, 0);
        part2Mask.transform.position -= new Vector3(part2Mask.transform.localScale.x / 2, 0, 0);
    }

    //Rotates sprite masks to create the "cut" effect
    void RotateMasks()
    {
        part1Mask.transform.rotation = new Quaternion();
        part2Mask.transform.rotation = new Quaternion();

        part1Mask.transform.Rotate(0, 0, cutAngle);
        part2Mask.transform.Rotate(0, 0, cutAngle);

        AngleMaskPositions();
        CalculateDirection();
        SetStartingPositions();
    }

    //Repositions masks to create the cut effect at a set angle
    void AngleMaskPositions()
    {
        part1Mask.transform.position = part1Renderer.transform.position;
        part2Mask.transform.position = part2Renderer.transform.position;


        part1Mask.transform.position += new Vector3((part1Mask.transform.localScale.x / 2) * Mathf.Cos(cutAngle * Mathf.Deg2Rad), (part1Mask.transform.localScale.y / 2) * Mathf.Sin(cutAngle * Mathf.Deg2Rad), 0);
        part2Mask.transform.position -= new Vector3((part2Mask.transform.localScale.x / 2) * Mathf.Cos(cutAngle * Mathf.Deg2Rad), (part2Mask.transform.localScale.y / 2) * Mathf.Sin(cutAngle * Mathf.Deg2Rad), 0);
    }

    void CalculateDirection()
    {
        direction = new Vector2(Mathf.Cos(cutAngle * Mathf.Deg2Rad), Mathf.Sin(cutAngle * Mathf.Deg2Rad));
        direction *= spacingMagnitude;
    }

    void SetStartingPositions()
    {
        part1Renderer.transform.localPosition = new Vector3(-direction.x, -direction.y);
        part2Renderer.transform.localPosition = new Vector3(direction.x, direction.y);

    }

    public void SetFacingDirection(float dir)
    {
        transform.localScale = new Vector3(dir, 1);
    }
}
