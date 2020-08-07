using System;

[Serializable]
public class User
{
    public int coins;

    public User(int coins = 0)
    {
        this.coins = coins;
    }
}
