using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public partial class FightController : MonoBehaviour
{
    public void ChooseEnemies(Side side, int countTarget, List<HeroController> listTarget, TypeSelect typeSelect = TypeSelect.Order)
    {
        listTarget.Clear();
        List<Warrior> workTeam = ((side == Side.Left) ? rightTeam : leftTeam).Where(x => x.heroController != null).ToList();
        workTeam = workTeam.Where(x => x?.heroController.IsDeath == false).ToList();
        if (countTarget > workTeam.Count) countTarget = workTeam.Count;
        countTarget = (int)Mathf.Clamp(countTarget, 0, workTeam.Count);
        if (countTarget > 0)
        {
            switch (typeSelect)
            {
                case TypeSelect.FirstLine:
                    for (int i = 0; i < workTeam.Count; i++)
                        listTarget.Add(workTeam[i].heroController);
                    break;
                case TypeSelect.SecondLine:
                    for (int i = 0; i < workTeam.Count; i++)
                        listTarget.Add(workTeam[i].heroController);
                    break;
                case TypeSelect.All:
                    for (int i = 0; i < workTeam.Count; i++)
                        listTarget.Add(workTeam[i].heroController);
                    break;
                case TypeSelect.Random:
                    int rand = 0;
                    for (int i = 0; i < countTarget; i++)
                    {
                        rand = Random.Range(0, workTeam.Count);
                        listTarget.Add(workTeam[rand].heroController);
                        workTeam.RemoveAt(rand);
                    }
                    break;
                case TypeSelect.GreatestHP:
                    workTeam.Sort(new WarriorHPComparer());
                    for (int i = 0; i < countTarget; i++) listTarget.Add(workTeam[i].heroController);
                    break;
                case TypeSelect.LeastHP:
                    workTeam.Sort(new WarriorHPComparer());
                    for (int i = 0; i < countTarget; i++) listTarget.Add(workTeam[countTarget - i].heroController);
                    break;
                case TypeSelect.GreatestAttack:
                    workTeam.Sort(new WarriorAttackComparer());
                    for (int i = 0; i < countTarget; i++) listTarget.Add(workTeam[i].heroController);
                    break;
                case TypeSelect.LeastAttack:
                    workTeam.Sort(new WarriorAttackComparer());
                    for (int i = 0; i < countTarget; i++) listTarget.Add(workTeam[countTarget - i].heroController);
                    break;
                case TypeSelect.GreatestInitiative:
                    workTeam.Sort(new WarriorInitiativeComparer());
                    for (int i = 0; i < countTarget; i++) listTarget.Add(workTeam[i].heroController);
                    break;
                case TypeSelect.LeastInitiative:
                    workTeam.Sort(new WarriorInitiativeComparer());
                    for (int i = 0; i < countTarget; i++) listTarget.Add(workTeam[countTarget - i].heroController);
                    break;
                case TypeSelect.GreatestArmor:
                    workTeam.Sort(new WarriorArmorComparer());
                    for (int i = 0; i < countTarget; i++) listTarget.Add(workTeam[i].heroController);
                    break;
                case TypeSelect.LeastArmor:
                    workTeam.Sort(new WarriorArmorComparer());
                    for (int i = 0; i < countTarget; i++) listTarget.Add(workTeam[countTarget - i].heroController);
                    break;
                case TypeSelect.IsAlive:
                    workTeam = workTeam.FindAll(x => x.heroController.hero.generalInfo.IsAlive == true);
                    for (int i = 0; i < countTarget; i++) listTarget.Add(workTeam[i].heroController);
                    break;
                case TypeSelect.IsNotAlive:
                    workTeam = workTeam.FindAll(x => x.heroController.hero.generalInfo.IsAlive == false);
                    for (int i = 0; i < countTarget; i++) listTarget.Add(workTeam[i].heroController);
                    break;
                case TypeSelect.People:
                    workTeam = workTeam.FindAll(x => x.heroController.hero.generalInfo.Race == Race.People);
                    for (int i = 0; i < countTarget; i++) listTarget.Add(workTeam[i].heroController);
                    break;
                case TypeSelect.Elf:
                    workTeam = workTeam.FindAll(x => x.heroController.hero.generalInfo.Race == Race.Elf);
                    for (int i = 0; i < countTarget; i++) listTarget.Add(workTeam[i].heroController);
                    break;
                case TypeSelect.Undead:
                    workTeam = workTeam.FindAll(x => x.heroController.hero.generalInfo.Race == Race.Undead);
                    for (int i = 0; i < countTarget; i++) listTarget.Add(workTeam[i].heroController);
                    break;
                case TypeSelect.Daemon:
                    workTeam = workTeam.FindAll(x => x.heroController.hero.generalInfo.Race == Race.Daemon);
                    for (int i = 0; i < countTarget; i++) listTarget.Add(workTeam[i].heroController);
                    break;
                case TypeSelect.God:
                    workTeam = workTeam.FindAll(x => x.heroController.hero.generalInfo.Race == Race.God);
                    for (int i = 0; i < countTarget; i++) listTarget.Add(workTeam[i].heroController);
                    break;
                case TypeSelect.Elemental:
                    workTeam = workTeam.FindAll(x => x.heroController.hero.generalInfo.Race == Race.Elemental);
                    for (int i = 0; i < countTarget; i++) listTarget.Add(workTeam[i].heroController);
                    break;
                case TypeSelect.Warrior:
                    workTeam = workTeam.FindAll(x => x.heroController.hero.generalInfo.ClassHero == Vocation.Warrior);
                    for (int i = 0; i < countTarget; i++) listTarget.Add(workTeam[i].heroController);
                    break;
                case TypeSelect.Wizard:
                    workTeam = workTeam.FindAll(x => x.heroController.hero.generalInfo.ClassHero == Vocation.Wizard);
                    for (int i = 0; i < countTarget; i++) listTarget.Add(workTeam[i].heroController);
                    break;
                case TypeSelect.Archer:
                    workTeam = workTeam.FindAll(x => x.heroController.hero.generalInfo.ClassHero == Vocation.Archer);
                    for (int i = 0; i < countTarget; i++) listTarget.Add(workTeam[i].heroController);
                    break;
                case TypeSelect.Pastor:
                    workTeam = workTeam.FindAll(x => x.heroController.hero.generalInfo.ClassHero == Vocation.Pastor);
                    for (int i = 0; i < countTarget; i++) listTarget.Add(workTeam[i].heroController);
                    break;
                case TypeSelect.Slayer:
                    workTeam = workTeam.FindAll(x => x.heroController.hero.generalInfo.ClassHero == Vocation.Slayer);
                    for (int i = 0; i < countTarget; i++) listTarget.Add(workTeam[i].heroController);
                    break;
                case TypeSelect.Tank:
                    workTeam = workTeam.FindAll(x => x.heroController.hero.generalInfo.ClassHero == Vocation.Tank);
                    for (int i = 0; i < countTarget; i++) listTarget.Add(workTeam[i].heroController);
                    break;
                case TypeSelect.Support:
                    workTeam = workTeam.FindAll(x => x.heroController.hero.generalInfo.ClassHero == Vocation.Support);
                    for (int i = 0; i < countTarget; i++) listTarget.Add(workTeam[i].heroController);
                    break;
                case TypeSelect.Order:
                    for (int i = 0; i < countTarget; i++) listTarget.Add(workTeam[i].heroController);
                    break;

            }
        }
    }

    public void ChooseEnemies(Side side, int countTarget, List<HeroController> listTarget, Race race)
    {

    }


    //Brain fight
    public HeroController ChooseEnemy(Side side)
    {
        HeroController result = null;
        List<Warrior> workTeam = (side == Side.Left) ? rightTeam : leftTeam;
        for (int i = 0; i < workTeam.Count; i++)
        {
            if (workTeam[i].heroController.IsDeath == false)
            {
                result = workTeam[i].heroController;
                break;
            }
        }
        if (result == null)
        {
            Win(side);
        }
        return result;
    }
}

[System.Serializable]
public class Warrior
{
    public HeroController heroController = null;
    public HexagonCell Cell { get => heroController.Cell; }
    public Warrior(HeroController heroController)
    {
        this.heroController = heroController;
    }

}
public class WarriorHPComparer : IComparer<Warrior>
{
    public int Compare(Warrior w1, Warrior w2)
    {
        if (w1.heroController.hero.characts.HP < w2.heroController.hero.characts.HP)
            return 1;
        else if (w1.heroController.hero.characts.HP > w2.heroController.hero.characts.HP)
            return -1;
        else
            return 0;
    }
}

public class WarriorAttackComparer : IComparer<Warrior>
{
    public int Compare(Warrior w1, Warrior w2)
    {
        if (w1.heroController.hero.characts.Damage < w2.heroController.hero.characts.Damage)
            return 1;
        else if (w1.heroController.hero.characts.Damage > w2.heroController.hero.characts.Damage)
            return -1;
        else
            return 0;
    }
}

public class WarriorInitiativeComparer : IComparer<Warrior>
{
    public int Compare(Warrior w1, Warrior w2)
    {
        if (w1.heroController.hero.characts.Initiative < w2.heroController.hero.characts.Initiative)
            return 1;
        else if (w1.heroController.hero.characts.Initiative > w2.heroController.hero.characts.Initiative)
            return -1;
        else
            return 0;
    }
}
public class WarriorArmorComparer : IComparer<Warrior>
{
    public int Compare(Warrior w1, Warrior w2)
    {
        if (w1.heroController.hero.characts.GeneralArmor < w2.heroController.hero.characts.GeneralArmor)
            return 1;
        else if (w1.heroController.hero.characts.GeneralArmor > w2.heroController.hero.characts.GeneralArmor)
            return -1;
        else
            return 0;
    }
}


public class HeroInitiativeComparer : IComparer<HeroController>
{
    public int Compare(HeroController o1, HeroController o2)
    {
        if (o1.hero.characts.Initiative < o2.hero.characts.Initiative)
            return 1;
        else if (o1.hero.characts.Initiative > o2.hero.characts.Initiative)
            return -1;
        else
            return 0;
    }
}
