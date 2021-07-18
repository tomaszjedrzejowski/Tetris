using System;
using System.Collections.Generic;
using UnityEngine;

public enum tetraminos { I = 0, J = 1, L = 2, O = 3, S = 4, T = 5, Z = 6}
class TetraminoFactory : MonoBehaviour
{
    [SerializeField] private Tetramino Itetramino, Jtetramino, Ltetramino, Otetramino, Stetramino, Ttetramino, Ztetramino;
    private Dictionary<tetraminos, Tetramino> gamePieces;

    private void Start()
    {
        gamePieces = new Dictionary<tetraminos, Tetramino> { {tetraminos.I, Itetramino}, {tetraminos.J, Jtetramino }, { tetraminos.L, Ltetramino},
            { tetraminos.O, Otetramino}, { tetraminos.S, Stetramino}, { tetraminos.T, Ttetramino}, { tetraminos.Z, Ztetramino} };
    }

    public Tetramino GetTetramino(int tetraminoID)
    {
        if (gamePieces.ContainsKey((tetraminos)tetraminoID))
        {
            var tetramino = Instantiate(gamePieces[(tetraminos)tetraminoID]);
            return tetramino;
        }
        else return null;
    }
}

