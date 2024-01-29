using System;
using static System.Net.Mime.MediaTypeNames;

public class Witch : Enemy
{
    private const int witchHealth = 20;
    private const int witchDamage = 5;

    public Witch()
    {
        Health = witchHealth;
        Damage = witchDamage;
    }
}