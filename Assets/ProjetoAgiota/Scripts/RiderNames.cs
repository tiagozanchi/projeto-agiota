using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiderNames
{
    private static string[] NAMES = new string[] {
            "Vivian",
            "Marlon",
            "Diya",
            "Felipe",
            "Rodrigo",
            "Nicholas",
            "Camryn",
            "Albert",
            "Caitlin",
            "Helen",
            "Declan",
            "Bailee",
            "Brendan",
            "Charlotte",
            "Lincoln",
            "Nyasia",
            "Jayleen",
            "Destiny",
            "Cristal",
            "Kailee",
            "Deacon",
            "Rose",
            "Jovani",
            "Victor",
            "Cora"
    };

    public static string getRandomRiderName()
    {
        return NAMES[Random.Range(0, NAMES.Length)];
    }
}
