using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public LinkedList<Vector2Int> queue;
    private Vector2Int _lastDirection;
    
    public Vector2Int LastDirection
    {
        get
        {
            if (queue.Count == 0)
                return _lastDirection;

            return queue.Last.Value;
        }
    }
    
    void Start()
    {
        Reset();
    }
    
    void Update()
    {
        // Keyboard controls
        if (Input.GetKeyDown("up") && LastDirection != Vector2.down)
        {
            Enqueue(Vector2Int.up);
        }
        else if (Input.GetKeyDown("down") && LastDirection != Vector2.up)
        {
            Enqueue(Vector2Int.down);
        }
        else if (Input.GetKeyDown("left") && LastDirection != Vector2.right)
        {
            Enqueue(Vector2Int.left);
        }
        else if (Input.GetKeyDown("right") && LastDirection != Vector2.left)
        {
            Enqueue(Vector2Int.right);
        }

        if (Input.GetMouseButtonDown(0))
        {
            var position = Input.mousePosition;
            if (position.x < Screen.width / 2)
            {
                if (LastDirection == Vector2.up)
                {
                    Enqueue(Vector2Int.left);
                }
                else if (LastDirection == Vector2.down)
                {
                    Enqueue(Vector2Int.right);
                }
                else if (LastDirection == Vector2.left)
                {
                    Enqueue(Vector2Int.down);
                }
                else if (LastDirection == Vector2.right)
                {
                    Enqueue(Vector2Int.up);
                }
            }
            else
            {
                if (LastDirection == Vector2.up)
                {
                    Enqueue(Vector2Int.right);
                }
                else if (LastDirection == Vector2.down)
                {
                    Enqueue(Vector2Int.left);
                }
                else if (LastDirection == Vector2.left)
                {
                    Enqueue(Vector2Int.up);
                }
                else if (LastDirection == Vector2.right)
                {
                    Enqueue(Vector2Int.down);
                }
            }
        }
    }
    
    private void Enqueue(Vector2Int direction)
    {
        queue.AddLast(direction);
        _lastDirection = direction;
    }
    
    public Vector2Int NextDirection()
    {
        if (queue.Count == 0) return _lastDirection;

        var first = queue.First.Value;
        queue.RemoveFirst();

        return first;
    }

    public void Reset()
    {
        queue = new LinkedList<Vector2Int>();
        Enqueue(Vector2Int.up);
    }
}
