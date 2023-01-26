using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Chess;
using UnityEngine;

//Game Manager will track all the pieces on the board, and the players.
public class GameManager : MonoBehaviour
{
    private GridManager _gridManager;
    private ChessBoardInitializer _chessBoardInitializer;
    private List<Piece> _allPieces;

    public Player whitePlayer;
    public Player blackPlayer;
    private Player _activePlayer;
    private void Awake()
    {
        _chessBoardInitializer = GetComponent<ChessBoardInitializer>();
        _gridManager = GetComponent<GridManager>();
    }

    private void Start()
    {
        InitNewGame();
    }

    private void InitNewGame()
    {
        //todo: Clear current game if needed.

        _gridManager.GenerateGrid();
        _allPieces = _chessBoardInitializer.CreateStartingChessBoard(this, _gridManager);
        //Init players. We just tell them what color to be because I WILL forget to set an enum in the inspector.
        whitePlayer.Init(this,PieceColor.White);
        whitePlayer.SetStartingPieces(_allPieces.Where(x=>x.Color == PieceColor.White).ToList());
        blackPlayer.Init(this,PieceColor.Black);
        blackPlayer.SetStartingPieces(_allPieces.Where(x => x.Color == PieceColor.Black).ToList());

        whitePlayer.SetTurnActive();
        
        //basically we call "set turn active" then wait for "onplayerfinsihedturn" to be called by the player.
        //this creates an interdependency between players and managers. thats okay because we are using dependency injection (telling the players that we are their manager), and not dealing with scene references.
        //its also okay because its chess and we know exactly the scope of the game, and how flexible we need to code it. We dont need an arbitrary number of players.
    }

    //tell the other player it's now their turn.
    public void OnPlayerFinishedTurn(Player player)
    {
        //todo: clock switching. Maybe store separate game clocks with the Player object? I like that
        if (player != _activePlayer)
        {
            Debug.Log("Did a turn go out of order?");
        }

        if (player == whitePlayer)
        {
            _activePlayer = blackPlayer;
            blackPlayer.SetTurnActive();
        }
        else if(player == blackPlayer)
        {
            _activePlayer = whitePlayer;
            whitePlayer.SetTurnActive();
        }
        else
        {
            Debug.LogError("Can't Finish player turn for "+player);
        }
    }

    public void PieceCaptured(Piece piece)
    {
        _allPieces.Remove(piece);
        //capturedPieces.Add(piece);
        //the player.PieceCaptured function does a safety check, so we could just call it on both without the if statement, but that just feels wrong.
        if (piece.Color == PieceColor.Black)
        {
            blackPlayer.PieceCaptured(piece);
        }else if (piece.Color == PieceColor.White)
        {
            whitePlayer.PieceCaptured(piece);
        }
    }
}
