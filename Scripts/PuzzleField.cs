using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleField : MonoBehaviour
{

    /// <summary>
    /// Prefab of puzzlePiece - assigned through editor 
    /// </summary>
    public GameObject puzzlePiecePrefab;
    /// <summary>
    /// Pref of emptyTile - assigned through editor
    /// </summary>
    public GameObject emptyTilePrefab;
    //Width of grid (column,row count)
    private int _width = 4;
    //Array represents the puzzle field grid
    [SerializeField]
    private IPuzzleTile[,] _field;
    //Empty tile reference
    [SerializeField]
    private EmptyTile _emptyTile;
    //list of all numbers in order
    private List<IPuzzleTile> _tiles = new List<IPuzzleTile>();

    /// <summary>
    /// //////////////////////////////////////////////////////////////////////////////////////////////////// REMOVE 
    /// </summary>
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            IsSolved();
        }
    }
    /// <summary>
    /// Function for returning the generated puzzles solvability.
    /// The algorithm first counts the total inversions count.
    /// Then checks if the empty tile is on odd row or even row.
    /// If its on the odd row - puzzle is solvable when the inversion count is even
    /// If its on the even row - puzzle is solvable when the inversion count is odd

    /// </summary>
    /// <returns>Returns the solvability of the puzzle</returns>
    public bool IsSolvable()
    {

        int inversions = 0;

        for (int i = 0; i < _tiles.Count; i++)
        {
            for (int j = i + 1; j < _tiles.Count; j++)
            {
                if (_tiles[i].GetNumber() > _tiles[j].GetNumber() && _tiles[j].GetNumber() != 0)
                {
                    inversions++;
                }
            }
        }

        Debug.Log("INVERSIoNS " + inversions);
        int debugZ = 4 - _emptyTile.GetZ();
        Debug.Log("BLANK ROW " + debugZ);
        Debug.Log("row" + _emptyTile.GetZ());
        if ((_emptyTile.GetZ()) % 2 == 0)
        {
            return inversions % 2 != 0;
        }
        else
        {
            return inversions % 2 == 0;
        }




    }
    /// <summary>
    /// Function for returning bool if the puzzle is solved and the game is over
    /// First part is putting all the numbers from _field array to _tiles list in the right order
    /// Then the inversions are counted - if the inversions count is 0 the puzzle is solved
    /// </summary>
    /// <returns>If the puzzle is solved</returns>
    public bool IsSolved()
    {
       
        _tiles.Clear();
        for (int i = 0; i < _width; i++)
        {
          for (int j = 0; j < _width; j++)
            {

             _tiles.Add(_field[j, i]);

            }
        }
        
        

        int inversions = 0;

        for (int i = 0; i < _tiles.Count; i++)
        {
            for (int j = i + 1; j < _tiles.Count; j++)
            {
                if (_tiles[i].GetNumber() > _tiles[j].GetNumber() && _tiles[j].GetNumber() != 0)
                {
                    inversions++;
                }
            }
        }


        return inversions == 0;



    }

    /// <summary>
    /// Function to check if the empty place is in the lower right corner
    /// </summary>
    /// <returns>Empty in right corner</returns>

    public bool IsEmptyInRightCorner()
    {
        if (_emptyTile.GetX() == _width - 1 && _emptyTile.GetZ() == _width - 1)
            return true;
        else return false;
    }

    /// <summary>
    /// Switching references, _field coordinates and positions of moved tile and emptytile
    /// </summary>
    /// <param name="tile">Tile reference</param>
    public void SwitchTiles(PuzzlePiece tile)
    {

        PuzzlePiece temp = tile;
        int tempX = temp.GetX();
        int tempZ = temp.GetZ();

        _emptyTile.transform.position = temp.GetPreviousPosition();
        temp.SetXZ(_emptyTile.GetX(), _emptyTile.GetZ());

        _field[temp.GetX(), temp.GetZ()] = temp;
        _emptyTile.SetXZ(tempX, tempZ);
        _field[_emptyTile.GetX(), _emptyTile.GetZ()] = _emptyTile;


    }
    /// <summary>
    /// Calls method on emptyTile to update free movement directions of puzzlePieces
    /// </summary>
    public void SetMovableTiles()
    {
        _emptyTile.SetNeighborsFreeDirection();
    }

    /// <summary>
    /// Creates the field by calling the instantiatePieces
    /// </summary>
    public void InitField()
    {
        StartCoroutine(InstantiatePieces());
    }

    /// <summary>
    /// Coroutine that creates and instantiates puzzle pieces and the emptyTile

    /// </summary>

    IEnumerator InstantiatePieces()
    {
        //Clears the list of ordered numbers if a new game is created

        _tiles.Clear();

        //begining transform position on the plane/grid
        float x = -1.5f, z = 1.5f;
        //creating a new field array
        _field = new IPuzzleTile[_width, _width];
        //list of unique random taken numbers
        List<int> randomTakenNumbers = new List<int>();
        //temporary variable for new prefab instances
        GameObject tempGameObject;
        //random number var
        int randomNumber;
        //creating all puzzleTiles and placing them into the field
        for (int i = 0; i < _width; i++)
        {

            for (int j = 0; j < _width; j++)
            {

                bool uniquePosition;
                //do while cycle for generating a unique number from 1-15
                do
                {
                    uniquePosition = true;
                    // generating new number
                    randomNumber = Random.Range(0, 16);

                    if (randomTakenNumbers.Count > 0)
                    {
                        foreach (int number in randomTakenNumbers)
                        {
                            // if there is a number that equals the generated number , the cycle needs to generate another number
                            if (randomNumber == number) uniquePosition = false;
                            
                        }
                    }
                    // the first number is added without checking
                    else randomTakenNumbers.Add(randomNumber);

                } while (!uniquePosition);
                // after the check cycle, a unique random number is added to the list
                randomTakenNumbers.Add(randomNumber);
                // if the number is not 0 (not empty) a new puzzlepiece is instantiated on the right position, placed to _field array
                if (randomNumber != 0)
                {
                    tempGameObject = Instantiate(puzzlePiecePrefab, new Vector3(x + j, 0, z - i), Quaternion.identity) as GameObject;
                    _field[j, i] = tempGameObject.GetComponent<IPuzzleTile>();

                    ((PuzzlePiece)_field[j, i]).SetPuzzleGameScript(GetComponent<PuzzleGame>());
                }
                // here the empty tile is instantiated , a reference of this tile is saved to _emptytile variable
                else
                {
                    tempGameObject = Instantiate(emptyTilePrefab, new Vector3(x + j, 0, z - i), Quaternion.identity) as GameObject;
                    _emptyTile = tempGameObject.GetComponent<IPuzzleTile>() as EmptyTile;
                    _field[j, i] = tempGameObject.GetComponent<IPuzzleTile>();


                }
                //parent is added to have them all organized
                tempGameObject.transform.parent = GetComponent<PuzzleGame>().puzzlePool;
                //set all important values - number, coordinates 
                _field[j, i].SetNumber(randomNumber);
                _field[j, i].SetXZ(j, i);
                //tile is added to the list
                _tiles.Add(_field[j, i]);


            }
        }
        // after the whole cycle we wait for all the tiles to be placed 
        yield return new WaitForSeconds(.4f);
        //check for new movement options
        SetMovableTiles();


    }
    /// <summary>
    /// Returns the width of field
    /// </summary>
    /// <returns>Field/Grid width</returns>
    public int GetWidth()
    {
        return _width;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IPuzzleTile[,] GetField()
    {
        return _field;
    }


}
