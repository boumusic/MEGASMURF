using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree
{

    public Dictionary<int, bool> tree;

    public SkillTree()
    {
        tree = new Dictionary<int, bool>();
    }

    public void Unlock(SkillType type, int lvl)
    {
        if(tree.ContainsKey((int)type + lvl))
        {
            tree[(int)type + lvl] = true;
        }
    }

    public int CheckEffect(SkillType type)
    {
        int effect = 0;
        switch (type)
        {
            case SkillType.IncreaseArmy:
                if (tree[(int)type + 3]) 
                {
                    effect += 2;
                }
                if (tree[(int)type + 2])
                {
                    effect += 2;
                }
                if (tree[(int)type + 1])
                {
                    effect += 2;
                }
                return effect;
            case SkillType.MoreLoot:
                int amount = 0;
                if (tree[(int)type + 3])
                {
                    amount += 1;
                }
                if (tree[(int)type + 2])
                {
                    amount += 1;
                }
                if (tree[(int)type + 1])
                {
                    amount += 1;
                }
                switch (amount)
                {
                    case 1:
                        return 20;
                    case 2:
                        return 50;
                    case 3:
                        return 80;
                }
                return effect;
            case SkillType.MaestroMobility:
                if (tree[(int)type + 3])
                {
                    effect += 1;
                }
                if (tree[(int)type + 2])
                {
                    effect += 1;
                }
                if (tree[(int)type + 1])
                {
                    effect += 1;
                }
                return effect;
            case SkillType.MaestroLife:
                if (tree[(int)type + 3])
                {
                    effect += 2;
                }
                if (tree[(int)type + 2])
                {
                    effect += 2;
                }
                if (tree[(int)type + 1])
                {
                    effect += 2;
                }
                return effect;
            case SkillType.ReduceCosts:
                int amount2 = 0;
                if (tree[(int)type + 3])
                {
                    amount2 += 1;
                }
                if (tree[(int)type + 2])
                {
                    amount2 += 1;
                }
                if (tree[(int)type + 1])
                {
                    amount2 += 1;
                }
                switch (amount2)
                {
                    case 1:
                        return 10;
                    case 2:
                        return 25;
                    case 3:
                        return 50;
                }
                return effect;
            case SkillType.SaferZone:
                if (tree[(int)type + 3])
                {
                    effect += 1;
                }
                if (tree[(int)type + 2])
                {
                    effect += 1;
                }
                if (tree[(int)type + 1])
                {
                    effect += 1;
                }
                return effect;
        }
        return 0;
    }

    // Change the priiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiice heeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeere
    public int CheckCost(SkillType type, int lvl)
    {
        switch (type)
        {
            case SkillType.IncreaseArmy:
                switch (lvl)
                {
                    case 1:
                        return 2;
                    case 2:
                        return 4;
                    case 3:
                        return 6;
                }
                break;
            case SkillType.MoreLoot:
                switch (lvl)
                {
                    case 1:
                        return 2;
                    case 2:
                        return 5;
                    case 3:
                        return 8;
                }
                break;
            case SkillType.MaestroMobility:
                switch (lvl)
                {
                    case 1:
                        return 8;
                    case 2:
                        return 14;
                    case 3:
                        return 20;
                }
                break;
            case SkillType.MaestroLife:
                switch (lvl)
                {
                    case 1:
                        return 4;
                    case 2:
                        return 6;
                    case 3:
                        return 8;
                }
                break;
            case SkillType.ReduceCosts:
                switch (lvl)
                {
                    case 1:
                        return 2;
                    case 2:
                        return 5;
                    case 3:
                        return 7;
                }
                break;
            case SkillType.SaferZone:
                switch (lvl)
                {
                    case 1:
                        return 3;
                    case 2:
                        return 8;
                    case 3:
                        return 10;
                }
                break;
        }
        return 0;
    }

    public bool CheckDependencies(SkillType type, int lvl)
    {
        switch (type)
        {
            case SkillType.IncreaseArmy:
                switch (lvl)
                {
                    case 1:
                        return true;
                    case 2:
                        return tree[14] || tree[18];
                    case 3:
                        return tree[6] || tree[19] || tree[23];
                }
                break;
            case SkillType.MoreLoot:
                switch (lvl)
                {
                    case 1:
                        return true;
                    case 2:
                        return tree[18] || tree[22];
                    case 3:
                        return tree[10] || tree[15] || tree[23];
                }
                break;
            case SkillType.MaestroMobility:
                switch (lvl)
                {
                    case 1:
                        return true;
                    case 2:
                        return tree[14] || tree[22];
                    case 3:
                        return tree[2] || tree[15] || tree[19];
                }
                break;
            case SkillType.MaestroLife:
                switch (lvl)
                {
                    case 1:
                        return tree[1] || tree[5];
                    case 2:
                        return tree[2] || tree[10] || tree[13] || tree[15];
                    case 3:
                        return tree[7] || tree[11] || tree[14];
                }
                break;
            case SkillType.ReduceCosts:
                switch (lvl)
                {
                    case 1:
                        return tree[5] || tree[9];
                    case 2:
                        return tree[2] || tree[6] || tree[17] || tree[19];
                    case 3:
                        return tree[3] || tree[11] || tree[18];
                }
                break;
            case SkillType.SaferZone:
                switch (lvl)
                {
                    case 1:
                        return tree[1] || tree[9];
                    case 2:
                        return tree[6] || tree[10] || tree[21] || tree[23];
                    case 3:
                        return tree[3] || tree[7] || tree[22];
                }
                break;
        }
        return true;
    }
}
