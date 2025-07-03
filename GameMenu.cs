// GameMenu.cs
using SplashKitSDK;
using System;

public enum GameMode
{
    TwoPlayer,
    VsAI
}

public class GameMenu
{
    private GameMode _mode = GameMode.TwoPlayer;
    private bool _editingWhite = true;

    public (GameMode mode, string whitePlayer, string blackPlayer) Show()
    {
        string whitePlayer = "White Player";
        string blackPlayer = "Black Player";

        while (!SplashKit.WindowCloseRequested("2D Chess Game"))
        {
            SplashKit.ProcessEvents();
            SplashKit.ClearScreen(Color.White);

            SplashKit.DrawText("Welcome to Chess", Color.Black, "Arial", 32, 250, 100);

            // Player name input
            SplashKit.DrawText("White Player Name:", Color.Black, "Arial", 24, 250, 150);
            SplashKit.DrawText(whitePlayer, _editingWhite ? Color.Cyan : Color.DarkBlue, "Arial", 24, 450, 150);

            if (_mode == GameMode.TwoPlayer)
            {
                SplashKit.DrawText("Black Player Name:", Color.Black, "Arial", 24, 250, 200);
                SplashKit.DrawText(blackPlayer, !_editingWhite ? Color.Cyan : Color.DarkBlue, "Arial", 24, 450, 200);
            }
            else
            {
                SplashKit.DrawText("Black Player: AI Player", Color.Black, "Arial", 24, 250, 200);
            }

            SplashKit.DrawText("Press F1 for 2-Player Mode", Color.Black, "Arial", 24, 250, 250);
            SplashKit.DrawText("Press F2 for Play vs AI", Color.Black, "Arial", 24, 250, 300);
            SplashKit.DrawText("Press TAB to switch between players", Color.Black, "Arial", 20, 250, 350);
            SplashKit.DrawText("Press ENTER to start game", Color.Green, "Arial", 24, 250, 400);
            SplashKit.RefreshScreen();

            // Handle mode selection
            if (SplashKit.KeyTyped(KeyCode.F1Key))
            {
                _mode = GameMode.TwoPlayer;
                blackPlayer = "Black Player";
            }
            if (SplashKit.KeyTyped(KeyCode.F2Key))
            {
                _mode = GameMode.VsAI;
                blackPlayer = "AI Player";
            }

            // Handle switching between players
            if (SplashKit.KeyTyped(KeyCode.TabKey) && _mode == GameMode.TwoPlayer)
            {
                _editingWhite = !_editingWhite;
            }

            // Handle text input - backspace
            if (SplashKit.KeyTyped(KeyCode.BackspaceKey))
            {
                if (_editingWhite && whitePlayer.Length > 0)
                    whitePlayer = whitePlayer.Substring(0, whitePlayer.Length - 1);
                else if (!_editingWhite && _mode == GameMode.TwoPlayer && blackPlayer.Length > 0)
                    blackPlayer = blackPlayer.Substring(0, blackPlayer.Length - 1);
            }

            // Handle space key
            if (SplashKit.KeyTyped(KeyCode.SpaceKey))
            {
                if (_editingWhite)
                    whitePlayer += " ";
                else if (_mode == GameMode.TwoPlayer)
                    blackPlayer += " ";
            }

            // Handle alphanumeric keys
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (SplashKit.KeyTyped(key))
                {
                    string keyStr = SplashKit.KeyName(key);
                    if (keyStr.Length == 1 && char.IsLetterOrDigit(keyStr[0]))
                    {
                        if (_editingWhite)
                            whitePlayer += keyStr;
                        else if (_mode == GameMode.TwoPlayer)
                            blackPlayer += keyStr;
                    }
                }
            }

            // Start game on Enter
            if (SplashKit.KeyTyped(KeyCode.ReturnKey))
            {
                return (_mode, whitePlayer, blackPlayer);
            }
        }

        return (_mode, whitePlayer, blackPlayer);
    }
}