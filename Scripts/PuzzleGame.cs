using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGame : MonoBehaviour
{
    public CanvasHandler canvas;
    // Parent for Tiles
    public Transform puzzlePool;
    // Reference to puzzle field
    private PuzzleField _puzzleField;
    /// number of moves made by player to solve puzzle
    [SerializeField]
    private int _moves;
    
    /// <summary>
    /// Called after a piece was moved, inceremnts the moves counter, resets all free movement directions to none, switches tiles
    /// sets new free directions and checks if the game is solved
    /// </summary>
    /// <param name="pieceMoved">reference to a piece that was moved</param>
    public void PieceMoved(PuzzlePiece pieceMoved)
    {

        _moves++;
        canvas.SetMovesCount(_moves);
        foreach (PuzzlePiece temp in puzzlePool.GetComponentsInChildren<PuzzlePiece>())
        {
            temp.setFreeDirection(PuzzlePiece.Direction.NONE);
        }
        _puzzleField.SwitchTiles(pieceMoved);

        _puzzleField.SetMovableTiles();
        if (_puzzleField.IsEmptyInRightCorner())
        {
            if (_puzzleField.IsSolved())
            {
                canvas.ShowSolvedMenu(_moves);
            }
        }



    }

    void Start()
    {
        _puzzleField = GetComponent<PuzzleField>();
    }

    public void NewGame()
    {

        canvas.SetMovesCount(0);
        do
        {
            for (int i = 0; i < puzzlePool.childCount; i++)
            {

                Destroy(puzzlePool.GetChild(i).gameObject);
            }
            _puzzleField.InitField();
           
            Debug.Log(_puzzleField.IsSolvable());
        }
        while (!_puzzleField.IsSolvable());
        }

    
    /// <summary>
    /// Quits the game
    /// </summary>
#if UNITY_STANDALONE
    public void QuitGame()
    {
        Application.Quit();
    }
#endif


}

