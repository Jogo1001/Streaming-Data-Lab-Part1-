

// Thinking About Clean Code
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#region Assignment Part 1

static public class AssignmentConfiguration
{
    public const int PartOfAssignmentThatIsInDevelopment = 2;
}

static public class AssignmentPart1
{
    private static string GetPartyFilePath()
    {
        return Path.Combine(Application.persistentDataPath, "party.txt");
    }

    public static void SavePartyButtonPressed()
    {
        string path = GetPartyFilePath();
        string content = PartySerializer.SerializeParty("SingleParty", GameContent.partyCharacters);
        File.WriteAllText(path, content);

        Debug.Log($"Party saved to {path}");
    }

    public static void LoadPartyButtonPressed()
    {
        string path = GetPartyFilePath();
        if (!File.Exists(path))
        {
            Debug.LogWarning($"No saved party found at {path}");
            return;
        }

        string content = File.ReadAllText(path);
        var (_, characters) = PartySerializer.DeserializeParty(content);
        GameContent.partyCharacters = characters;

        GameContent.RefreshUI();
        Debug.Log($"Party loaded from {path}");
    }
}

#endregion









