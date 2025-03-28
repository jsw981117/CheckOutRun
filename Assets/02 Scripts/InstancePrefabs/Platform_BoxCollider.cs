using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Platform_BoxCollider : MonoBehaviour
{
    float minusY = 0.4f;

    //현재 프리팹 오브젝트의 Scale에 따라 BoxCollider2D의 크기를 조정합니다.
    private void Awake()
    {
        BoxCollider2D box2D = gameObject.AddComponent<BoxCollider2D>();
        box2D.size = new Vector2(box2D.size.x, box2D.size.y - (minusY / transform.localScale.y));
        box2D.offset = new Vector2(box2D.offset.x, box2D.offset.y - ((minusY / 2) / transform.localScale.y));
        box2D.usedByEffector = true;
    }
}
