using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPuzzleTile  {

    int GetNumber();
    void SetNumber(int number);
    void SetXZ(int x, int z);
    int GetX();
    int GetZ();
}
