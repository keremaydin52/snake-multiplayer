using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public Sprite Empty;
    public Sprite Apple;
    public Sprite SnakeHead;
    public Sprite SnakeBody;
    private Image image;
    
    private RectTransform _rectTransform;
    private TileContent _content;
    
    public RectTransform RectTransform
    {
        get
        {
            return _rectTransform;
        }
    }

    public TileContent Content
    {
        get
        {
            return _content;
        }
        set
        {
            _content = value;
            ZRotation = 0;
            switch (_content)
            {
                case TileContent.Empty:
                    image.sprite = Empty;
                    break;
                case TileContent.Apple:
                    image.sprite = Apple;
                    break;
                
                case TileContent.SnakeHead:
                    image.sprite = SnakeHead;
                    break;
                case TileContent.SnakeBody:
                    image.sprite = SnakeBody;
                    break;
            }
        }
    }

    private float _zRotation = 0;
    public float ZRotation
    {
        get
        {
            return _zRotation;
        }
        set
        {
            _zRotation = value;
            transform.rotation = Quaternion.Euler(0, 0, value);
        }
    }

    void Awake()
    {
        image = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();
        Content = TileContent.Empty;
    }

    public void Reset()
    {
        Content = TileContent.Empty;
    }
}

public enum TileContent
{
    Empty, Apple, SnakeHead, SnakeBody
}
