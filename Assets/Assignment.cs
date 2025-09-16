

// Thinking About Clean Code
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


#region Assignment Instructions



public partial class PartyCharacter
{
    public int classID;
    public int health;
    public int mana;
    public int strength;
    public int agility;
    public int wisdom;

    public LinkedList<int> equipment;

}


#endregion


#region Assignment Part 1

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


#region Assignment Part 2


static public class AssignmentConfiguration
{
    public const int PartOfAssignmentThatIsInDevelopment = 2;
}



static public class AssignmentPart2
{
    static List<string> listOfPartyNames;
    static string partyFolderPath;

    private static string MakeSafeFileName(string rawName)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
            rawName = rawName.Replace(c, '_');
        return rawName;
    }

    public static void GameStart()
    {
        partyFolderPath = Path.Combine(Application.persistentDataPath, "Parties");
        if (!Directory.Exists(partyFolderPath))
            Directory.CreateDirectory(partyFolderPath);

        listOfPartyNames = new List<string>();
        foreach (string partyFilePath in Directory.GetFiles(partyFolderPath, "*.party"))
        {
            using StreamReader sr = new StreamReader(partyFilePath);
            string partyName = sr.ReadLine();
            if (!string.IsNullOrWhiteSpace(partyName))
                listOfPartyNames.Add(partyName);
        }

        GameContent.RefreshUI();
    }

    public static List<string> GetListOfPartyNames()
    {
        return listOfPartyNames;
    }

    public static void LoadPartyDropDownChanged(string selectedName)
    {
        string safeFileName = MakeSafeFileName(selectedName) + ".party";
        string path = Path.Combine(partyFolderPath, safeFileName);
        if (!File.Exists(path)) return;

        string content = File.ReadAllText(path);
        var (_, characters) = PartySerializer.DeserializeParty(content);
        GameContent.partyCharacters = characters;

        GameContent.RefreshUI();
    }

    public static void SavePartyButtonPressed()
    {
        string partyName = GameContent.GetPartyNameFromInput();
        if (string.IsNullOrWhiteSpace(partyName)) return;

        string safeFileName = MakeSafeFileName(partyName) + ".party";
        string path = Path.Combine(partyFolderPath, safeFileName);

        string content = PartySerializer.SerializeParty(partyName, GameContent.partyCharacters);
        File.WriteAllText(path, content);

        if (!listOfPartyNames.Contains(partyName))
            listOfPartyNames.Add(partyName);

        GameContent.RefreshUI();
    }

    public static void DeletePartyButtonPressed(string selectedName)
    {
        string safeFileName = MakeSafeFileName(selectedName) + ".party";
        string path = Path.Combine(partyFolderPath, safeFileName);

        if (File.Exists(path))
            File.Delete(path);

        listOfPartyNames.Remove(selectedName);
        GameContent.partyCharacters.Clear();

        GameContent.RefreshUI();
    }
}

#endregion


