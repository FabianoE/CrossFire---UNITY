using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPatent : MonoBehaviour
{
    public static int ReturnPatentByExp(int exp)
    {
        switch (exp)
        {
            case int n when (n <= 456):
                return 1;
            case int n when (n <= 912):
                return 2;
            default:
                return 3;
        }
        return 0;
    }

    public static int ReturnNextPatentExp(int exp)
    {
        switch (exp)
        {
            case int n when (n <= 456):
                return 912;
            case int n when (n <= 912):
                return 1824;
            default:
                return 4;
        }
        return 0;
    }

    public static int ReturnMaxExpPatentByExp(int exp)
    {
        switch (exp)
        {
            case int n when (n <= 456):
                return 456;
            case int n when (n <= 912):
                return 912;
            default:
                return 4;
        }
        return 0;
    }

    public static int ReturnPorcentage(int currentExp)
    {
        return (currentExp / ReturnPatentByExp(currentExp));
    }
}
