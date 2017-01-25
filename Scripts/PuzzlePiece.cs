using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PuzzlePiece : MonoBehaviour, IPuzzleTile
{
    
    // Reference to PuzzleGameScript 
    private PuzzleGame _puzzleGameScript;
    // Number of a Puzzle piece from 1-15
    [SerializeField]
    private int _pieceNumber;
    // X,Z coordinates in PuzzleField 
    [SerializeField]
    private int _x, _z;
    /// <summary>
    /// Represents the movement directions
    /// </summary>
    public enum Direction { NONE, FORWARD, BACK, LEFT, RIGHT };
    // Represents the direction the puzzle piece can move
    [SerializeField]
    private Direction _freeDirection = Direction.NONE;
   
    // These points define the start and end of an drag movement
    [SerializeField]
    private Vector3 _dragStartPoint, _dragEndPoint;
    // Store the mousepoint position projected to worldspace 
    private Vector3 screenPoint;
    // Offset between the object pivot and the mouse click
    private Vector3 offset;

    /// <summary>
    /// Sets the PuzzleGame reference
    /// </summary>
    /// <param name="reference"> PuzzleGame reference</param>
    public void SetPuzzleGameScript(PuzzleGame reference)
    {
        _puzzleGameScript = reference;
    }

   /// <summary>
   /// Called on every mouseclick on a puzzle piece
   /// Sets the _dragstartpoint, offset and sets the _dragEndPoint based on freeDirection 
   /// </summary>
    void OnMouseDown()
    {
        if (_freeDirection != Direction.NONE)
        {

            _dragStartPoint = transform.position;

            screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

            if (_freeDirection == Direction.LEFT)
            {
                _dragEndPoint = new Vector3(_dragStartPoint.x - 1f, _dragStartPoint.y, _dragStartPoint.z);
            }
            else if (_freeDirection == Direction.RIGHT)
            {
                _dragEndPoint = new Vector3(_dragStartPoint.x + 1f, _dragStartPoint.y, _dragStartPoint.z);
            }
            else if (_freeDirection == Direction.FORWARD)
            {
                _dragEndPoint = new Vector3(_dragStartPoint.x, _dragStartPoint.y, _dragStartPoint.z + 1f);
            }
            else if (_freeDirection == Direction.BACK)
            {
                _dragEndPoint = new Vector3(_dragStartPoint.x, _dragStartPoint.y, _dragStartPoint.z - 1f);
            }
        }





    }
    /// <summary>
    /// Represents the end of an MouseDrag, checks if the piece is closer to the dragEndPoint or not. Sets the position of a piece.
    /// If the piece was moved to the endPoint it calls the PieceMoved method from PuzzleGame script
    /// </summary>
    void OnMouseUp()
    {
        if (_freeDirection != Direction.NONE)
        {
            if (Vector3.Distance(transform.position, _dragEndPoint) < Vector3.Distance(transform.position, _dragStartPoint))
            {
                transform.position = _dragEndPoint;
                _puzzleGameScript.PieceMoved(this);

            }
            else
            {
                transform.position = _dragStartPoint;
            }
        }
    }
    /// <summary>
    /// Mouse drag function (starts with and OnMouseDown and ends with OnMouseUp)
    /// The drag path is set by _dragStartPoint and _dragEndPoint based on _freeDirection 
    /// </summary>
    void OnMouseDrag()
    {
        if (_freeDirection != Direction.NONE)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            if (_freeDirection == Direction.LEFT)
            {
                transform.position = new Vector3(Mathf.Clamp(curPosition.x, _dragEndPoint.x, _dragStartPoint.x), transform.position.y, transform.position.z);
            }
            else if (_freeDirection == Direction.RIGHT)
            {
                transform.position = new Vector3(Mathf.Clamp(curPosition.x, _dragStartPoint.x, _dragEndPoint.x), transform.position.y, transform.position.z);
            }
            else if (_freeDirection == Direction.FORWARD)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(curPosition.z, _dragStartPoint.z, _dragEndPoint.z));
            }
            else if (_freeDirection == Direction.BACK)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(curPosition.z, _dragEndPoint.z, _dragStartPoint.z));
            }
        }
    }
    /// <summary>
    /// Sets the free movement direction of a puzzle piece
    /// </summary>
    /// <param name="dir">Free movement direction</param>
    public void setFreeDirection(Direction dir)
    {
        _freeDirection = dir;
    }
    /// <summary>
    /// Returns the position of the piece before the move
    /// </summary>
    /// <returns> Previous position of a piece</returns>
    public Vector3 GetPreviousPosition()
    {
        return _dragStartPoint;
    }

    /// <summary>
    /// Returns the number/value of an piece
    /// </summary>
    /// <returns>Puzzle piece number</returns>
         
    public int GetNumber()
    {
        return _pieceNumber;
    }
    /// <summary>
    /// Sets the puzzle piece number
    /// </summary>
    /// <param name="number">Puzzle piece number</param>
    public void SetNumber(int number)
    {
        _pieceNumber = number;
        transform.GetChild(0).GetComponent<TextMesh>().text = number.ToString();
    }
    /// <summary>
    /// Returns X coordinate in PuzzleField
    /// </summary>
    /// <returns>X coordinate</returns>
    public int GetX()
    {
        return _x;
    }
    /// <summary>
    /// Returns Z coordinate in PuzzleField
    /// </summary>
    /// <returns>Z coordinate</returns>
    public int GetZ()
    {
        return _z;
    }
    /// <summary>
    /// Sets X,Z coordinates in the PuzzleField
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    public void SetXZ(int x, int z)
    {
        _x = x;
        _z = z;
    }
}

