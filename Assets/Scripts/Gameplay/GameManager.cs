using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Chess;
using Chess.UI;
using Unity.VisualScripting;
using UnityEngine;

//Game Manager will track all the pieces on the board, and the players.
public class GameManager : MonoBehaviour
{
    [Header("Game Config")]
    public int StartingMinutes = 3;
    public int StartingSeconds = 0;
    public int SecondsToAddEachTurn = 2;
    
    [Header("UI Config")]
    [SerializeField] private GameSplash _splash;

    [SerializeField] private UIGameOverPanel _gameOverPanel;
    public GridManager GridManager => _gridManager;
    private GridManager _gridManager;
    public ChessBoardInitializer ChessBoardInitializer => _chessBoardInitializer;
    private ChessBoardInitializer _chessBoardInitializer;
    private List<Piece> _allPieces;

    [Header("Player Setup")]
    public Player whitePlayer;
    public Player blackPlayer;
    private Player _activePlayer;

    private int _turnCount = 0;
    public ChessTimer Timer => _timer;
    private ChessTimer _timer;
    
    private void Awake()
    {
        _chessBoardInitializer = GetComponent<ChessBoardInitializer>();
        _gridManager = GetComponent<GridManager>();
        _timer = new ChessTimer(new TimeSpan(0,0,StartingMinutes,StartingSeconds));
        _splash.Display();
    }
    

    private void OnEnable()
    {
        GameSplash.OnStartGame += InitNewGame;
    }

    private void OnDisable()
    {
        GameSplash.OnStartGame -= InitNewGame;
    }
    void Start()
    {
        whitePlayer.Init(this, PieceColor.White,InputState.StartSplash);
        blackPlayer.Init(this, PieceColor.Black,InputState.StartSplash);
        //
        _gridManager.GenerateGrid();
        _allPieces = _chessBoardInitializer.CreateStartingChessBoard(this, _gridManager);
        //Init players. We just tell them what color to be because I WILL forget to set an enum in the inspector.
        whitePlayer.SetStartingPieces(_allPieces.Where(x=>x.Color == PieceColor.White).ToList());
        blackPlayer.SetStartingPieces(_allPieces.Where(x => x.Color == PieceColor.Black).ToList());
    }
    private void InitNewGame()
    {
        //todo: Clear current game if needed.
        _turnCount = 0;
        whitePlayer.NewGame(true);
        blackPlayer.NewGame(false);
        
        //start timer
        _timer.StartTimeForPlayer(PieceColor.White);
        
        //basically we call "set turn active" then wait for "onplayerfinsihedturn" to be called by the player.
        //this creates an interdependency between players and managers. thats okay because we are using dependency injection (telling the players that we are their manager), and not dealing with scene references.
        //its also okay because its chess and we know exactly the scope of the game, and how flexible we need to code it. We dont need an arbitrary number of players.
    }

    //tell the other player it's now their turn.
    public void OnPlayerFinishedTurn(Player player)
    {
        //todo: Check if kings are in check.
        if (player.King.IsInCheck())
        {
            //that's not a valid move!
        }
        
        //todo: clock switching. Maybe store separate game clocks with the Player object? I like that
        if (_activePlayer != null && player != _activePlayer)
        {
            Debug.LogError("Did a turn go out of order?");
        }

        if (player == whitePlayer)
        {
            _activePlayer = blackPlayer;
            blackPlayer.SetTurnActive();
            _timer.StartTimeForPlayer(PieceColor.Black);
            if (_turnCount > 1)
            {
                _timer.AddTimeToPlayer(PieceColor.White,SecondsToAddEachTurn);
            }
        }
        else if(player == blackPlayer)
        {
            _activePlayer = whitePlayer;
            whitePlayer.SetTurnActive();
            _timer.StartTimeForPlayer(PieceColor.White);
            if (_turnCount > 1)
            {
                _timer.AddTimeToPlayer(PieceColor.Black, SecondsToAddEachTurn);
            }
        }
        else
        {
            Debug.LogError("Can't Finish player turn for "+player);
        }

        _turnCount++;
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

        if (piece is King king)
        {
            //The game ends
            GameIsOver(Piece.OppositeColor(king.Color));
        }
    }

    private void Update()
    {
        if (_timer.IsPlayerOutOfTime(PieceColor.Black))
        {
            GameIsOver(PieceColor.White);
        }else if (_timer.IsPlayerOutOfTime(PieceColor.White))
        {
            GameIsOver(PieceColor.Black);
        }
    }

    private void GameIsOver(PieceColor winner)
    {
        _timer.Stop();
        //broadcast static action that UI listens to
        //change player input to game over state
        //after both players want to restart... we restart
        //flip board side? change player->arcade input?
        
        _gameOverPanel.SetVictory(winner);
    }

    public Player ColorToPlayer(PieceColor playerColor)
    {
        if (playerColor == PieceColor.Black)
        {
            return blackPlayer;
        }else if (playerColor == PieceColor.White)
        {
            return whitePlayer;
        }

        //should never happen
        return null;
    }

    public bool IsSpaceInCheck(Vector2Int tilePos, PieceColor attackingColor)
    {
        Player attackingPlayer = ColorToPlayer(attackingColor);
        //loop through all possible moves for all pieces of the attacking color and 
        
        var pieces = attackingPlayer.GetAvailablePieces();
        foreach (var piece in pieces)
        {
            foreach (var destination in piece.ValidDestinations())
            {
                if (destination.Position == tilePos)
                {
                    return true;
                }
            }
        }

        return false;
    }

    //only call this if a king is in check
    public void CheckForCheckMate(King victim)
    {
        //check if king is currently in check... again? 
        //check if the king has any available moves (king moves checks for check). 
        //if so, not checkmate.
        //otherwise, get all pieces with available moves
        //loop through each piece.
        //
    }

    public void SelectReadyToStartGame(Player player)
    {
        _splash.SetPlayerReady(player.Color);
    }
}
