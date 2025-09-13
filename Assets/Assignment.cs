
/*
This RPG data streaming assignment was created by Fernando Restituto with 
pixel RPG characters created by Sean Browning.
*/

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


#region Assignment Instructions

/*  Hello!  Welcome to your first lab :)

Wax on, wax off.

    The development of saving and loading systems shares much in common with that of networked gameplay development.  
    Both involve developing around data which is packaged and passed into (or gotten from) a stream.  
    Thus, prior to attacking the problems of development for networked games, you will strengthen your abilities to develop solutions using the easier to work with HD saving/loading frameworks.

    Try to understand not just the framework tools, but also, 
    seek to familiarize yourself with how we are able to break data down, pass it into a stream and then rebuild it from another stream.


Lab Part 1

    Begin by exploring the UI elements that you are presented with upon hitting play.
    You can roll a new party, view party stats and hit a save and load button, both of which do nothing.
    You are challenged to create the functions that will save and load the party data which is being displayed on screen for you.

    Below, a SavePartyButtonPressed and a LoadPartyButtonPressed function are provided for you.
    Both are being called by the internal systems when the respective button is hit.
    You must code the save/load functionality.
    Access to Party Character data is provided via demo usage in the save and load functions.

    The PartyCharacter class members are defined as follows.  */

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


/*
    Access to the on screen party data can be achieved via …..

    Once you have loaded party data from the HD, you can have it loaded on screen via …...

    These are the stream reader/writer that I want you to use.
    https://docs.microsoft.com/en-us/dotnet/api/system.io.streamwriter
    https://docs.microsoft.com/en-us/dotnet/api/system.io.streamreader

    Alright, that’s all you need to get started on the first part of this assignment, here are your functions, good luck and journey well!
*/


#endregion


#region Assignment Part 1

static public class AssignmentPart1
{
   
    static public void SavePartyButtonPressed()
    {

        //save characters into party.txt
        string path = Path.Combine(Application.persistentDataPath, "party.txt");

        // creates a file for characters
        using (StreamWriter writer = new StreamWriter(path))
        {

            // for each party character in gamecontent, it loops every character in the party
            foreach (PartyCharacter pc in GameContent.partyCharacters)
            {

                writer.Write($"{pc.classID},{pc.health},{pc.mana},{pc.strength},{pc.agility},{pc.wisdom}");
               

                // loop, if character equipment is not null and characters count is greater than zero, if it is true or the character-
                // has equipment ----> save, otherwise null.
                if(pc.equipment != null && pc.equipment.Count > 0 )
                {

                    foreach (int eq in pc.equipment)
                    {
                        writer.Write($",{eq}");
                    }

                }
                writer.WriteLine();
             
            }
        }
        // save testing
        Debug.Log("Test Save CHaracters"+path);

    }

    static public void LoadPartyButtonPressed()
    {
        // build a file txt for characters
        string path = Path.Combine(Application.persistentDataPath, "party.txt");


        // if there's no file exist in "path", if its true, return
        if (!File.Exists(path))
        {
            Debug.LogWarning("Characters not found at " + path);
            return;
        }

        // Remove Duplicate CHaracters file
        GameContent.partyCharacters.Clear();


        // if charactes file is exist in path
        using (StreamReader sr = new StreamReader(path))
        {

            string spacing;


            while((spacing = sr.ReadLine()) != null)
            {
                string[] Chracter_parts = spacing.Split(',');



                if(Chracter_parts.Length < 6)
                {
                    Debug.LogWarning("Error" + spacing);
                    continue;
                }

                int Chracter_classID = int.Parse(Chracter_parts[0]);
                int Character_health = int.Parse(Chracter_parts[1]);
                int Character_mana = int.Parse(Chracter_parts[2]);
                int Character_strength = int.Parse(Chracter_parts[3]);
                int Character_agility = int.Parse(Chracter_parts[4]);
                int Character_wisdom = int.Parse(Chracter_parts[5]);

                
                // construct a new party
                PartyCharacter P_Character = new PartyCharacter(Chracter_classID, Character_health, Character_mana
                                                                , Character_strength, Character_agility, Character_wisdom);

                //Equipment Load
                LinkedList<int> Equipment_List = new LinkedList<int>();
                for ( int i = 6; i < Chracter_parts.Length; i++)
                {

                    if(!string.IsNullOrWhiteSpace(Chracter_parts[i]))
                    {
                        Equipment_List.AddLast(int.Parse(Chracter_parts[i]));
                    }

                }


                P_Character.equipment = Equipment_List;
                // after contruct a new party, it add the loaded charactesr to Linkedlist in party character function
                GameContent.partyCharacters.AddLast(P_Character);

            }

        }
        // refresh gamecontent to load the saved characters
        GameContent.RefreshUI();
        //test for characters load
        Debug.Log("Test Load Characters" +  path);


    }



}



#endregion


#region Assignment Part 2

//  Before Proceeding!
//  To inform the internal systems that you are proceeding onto the second part of this assignment,
//  change the below value of AssignmentConfiguration.PartOfAssignmentInDevelopment from 1 to 2.
//  This will enable the needed UI/function calls for your to proceed with your assignment.
static public class AssignmentConfiguration
{
    public const int PartOfAssignmentThatIsInDevelopment = 1;
}

/*

In this part of the assignment you are challenged to expand on the functionality that you have already created.  
    You are being challenged to save, load and manage multiple parties.
    You are being challenged to identify each party via a string name (a member of the Party class).

To aid you in this challenge, the UI has been altered.  

    The load button has been replaced with a drop down list.  
    When this load party drop down list is changed, LoadPartyDropDownChanged(string selectedName) will be called.  
    When this drop down is created, it will be populated with the return value of GetListOfPartyNames().

    GameStart() is called when the program starts.

    For quality of life, a new SavePartyButtonPressed() has been provided to you below.

    An new/delete button has been added, you will also find below NewPartyButtonPressed() and DeletePartyButtonPressed()

Again, you are being challenged to develop the ability to save and load multiple parties.
    This challenge is different from the previous.
    In the above challenge, what you had to develop was much more directly named.
    With this challenge however, there is a much more predicate process required.
    Let me ask you,
        What do you need to program to produce the saving, loading and management of multiple parties?
        What are the variables that you will need to declare?
        What are the things that you will need to do?  
    So much of development is just breaking problems down into smaller parts.
    Take the time to name each part of what you will create and then, do it.

Good luck, journey well.

*/

static public class AssignmentPart2
{

    static List<string> listOfPartyNames;

    static public void GameStart()
    {
        listOfPartyNames = new List<string>();
        listOfPartyNames.Add("sample 1");
        listOfPartyNames.Add("sample 2");
        listOfPartyNames.Add("sample 3");

        GameContent.RefreshUI();
    }

    static public List<string> GetListOfPartyNames()
    {
        return listOfPartyNames;
    }

    static public void LoadPartyDropDownChanged(string selectedName)
    {
        GameContent.RefreshUI();
    }

    static public void SavePartyButtonPressed()
    {
        GameContent.RefreshUI();
    }

    static public void DeletePartyButtonPressed()
    {
        GameContent.RefreshUI();
    }

}

#endregion


