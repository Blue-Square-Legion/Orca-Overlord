using UnityEngine;

public interface IHealth
{
    void OnGetDamage(int dmg);
    void OnRecoverHealth(int hp);
    void OnHealthOver();
}
