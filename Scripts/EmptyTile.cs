using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyTile : MonoBehaviour,IPuzzleTile {
    //Begining position of raycasts
    public Vector3 _raycastOrigin;

    // X,Z coordinates in PuzzleField 
    [SerializeField]
    private int _x, _z;
    // Number of a tile - set to 0
    private int number=0;


    /// <summary>
    /// sets the origin of raycast , fires rays in 4 directions, and if a puzzlepiece is hit, sets the oposit freeMovement direction to the piece
    /// </summary>
    public void SetNeighborsFreeDirection()
    {
        _raycastOrigin = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        RaycastHit hit;
        if (Physics.Raycast(_raycastOrigin,Vector3.left,out hit,0.8f))
        {
            hit.collider.gameObject.GetComponent<PuzzlePiece>().setFreeDirection(PuzzlePiece.Direction.RIGHT);
        }
        if (Physics.Raycast(_raycastOrigin, Vector3.right, out hit, 0.8f) )
        {
            hit.collider.gameObject.GetComponent<PuzzlePiece>().setFreeDirection(PuzzlePiece.Direction.LEFT);
        }
        if (Physics.Raycast(_raycastOrigin, Vector3.forward, out hit, 0.8f))
        {
            hit.collider.gameObject.GetComponent<PuzzlePiece>().setFreeDirection(PuzzlePiece.Direction.BACK);
        }
        if (Physics.Raycast(_raycastOrigin, Vector3.back, out hit, 0.8f) )
        {
            hit.collider.gameObject.GetComponent<PuzzlePiece>().setFreeDirection(PuzzlePiece.Direction.FORWARD);
        }


    }
    /// <summary>
    /// Returns the number/value of an piece
    /// </summary>
    /// <returns>Puzzle piece number</returns>
    public int GetNumber()
    {
        return number;
    }
    /// <summary>
    /// Sets the puzzle piece number
    /// </summary>
    /// <param name="number">Puzzle piece number</param>
    public void SetNumber(int number)
    {
        this.number = number;
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
