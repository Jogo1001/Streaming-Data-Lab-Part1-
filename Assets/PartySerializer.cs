using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PartySerializer : MonoBehaviour
{
    public static string SerializeParty(string partyName, IEnumerable<PartyCharacter> characters)
    {
        StringWriter sw = new StringWriter();
        sw.WriteLine(partyName);
        foreach (var Class in characters)
        {
            sw.Write($"{Class.classID},{Class.health},{Class.mana},{Class.strength},{Class.agility},{Class.wisdom}");
            if (Class.equipment != null)
            {
                foreach (int Equipment in Class.equipment)
                    sw.Write($",{Equipment}");
            }
            sw.WriteLine();
        }
        return sw.ToString();
    }

    public static (string partyName, LinkedList<PartyCharacter> characters) DeserializeParty(string fileContent)
    {
        StringReader sr = new StringReader(fileContent);
        string partyName = sr.ReadLine();
        var characters = new LinkedList<PartyCharacter>();
        string line;
        while ((line = sr.ReadLine()) != null)
        {
            var parts = line.Split(',');
            if (parts.Length < 6) continue;
            var pc = new PartyCharacter(
                int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]),
                int.Parse(parts[3]), int.Parse(parts[4]), int.Parse(parts[5]));

            var eq = new LinkedList<int>();
            for (int i = 6; i < parts.Length; i++)
                if (!string.IsNullOrWhiteSpace(parts[i]))
                    eq.AddLast(int.Parse(parts[i]));
            pc.equipment = eq;

            characters.AddLast(pc);

        }
        return (partyName, characters);
    }
}
