using The_Ritual;
using System;

internal class Starter
{
    private static void Main(string[] args)
    {
        Game gameInstance = new Game();
        gameInstance.StartGame(gameInstance);
    }
}