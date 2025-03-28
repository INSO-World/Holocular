using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseLoader : MonoBehaviour
{
    public static bool checkFoolderIfValid(string path)
    {
        RuntimeDebug.Log("Checking if path " + path + " is a Binocular Export");
        if (!System.IO.File.Exists(path + System.IO.Path.DirectorySeparatorChar + "branches.json")
            || !System.IO.File.Exists(path + System.IO.Path.DirectorySeparatorChar + "commits-commits.json")
            || !System.IO.File.Exists(path + System.IO.Path.DirectorySeparatorChar + "commits-files.json")
            || !System.IO.File.Exists(path + System.IO.Path.DirectorySeparatorChar + "commits-files-users.json")
            || !System.IO.File.Exists(path + System.IO.Path.DirectorySeparatorChar + "commits-users.json")
            || !System.IO.File.Exists(path + System.IO.Path.DirectorySeparatorChar + "commits.json")
            || !System.IO.File.Exists(path + System.IO.Path.DirectorySeparatorChar + "files.json")
            || !System.IO.File.Exists(path + System.IO.Path.DirectorySeparatorChar + "users.json"))
        {
            return false;
        }
        return true;
    }

    public static bool importDatabase(string path)
    {
        if (!importBranches(path)
            || !importCommits(path)
            || !importCommitsFilesRelation(path)
            || !importCommitsCommitsRelation(path)
            || !importCommitsStakeholdersRelation(path)
            || !importCommitsFilesStakeholdersRelation(path)
            || !importFiles(path)
            || !importStakeholders(path))
        {
            return false;
        }

        return true;
    }

    public static bool importBranches(string path)
    {
        string branchesJSON = System.IO.File.ReadAllText(path + System.IO.Path.DirectorySeparatorChar + "branches.json");
        try
        {
            Main.branches = JsonUtility.FromJson<DBBranches>("{\"branches\":" + branchesJSON + "}");
            RuntimeDebug.Log("Branches Imported Sucessfull");
            return true;
        }
        catch (System.Exception ex)
        {
            return false;
            RuntimeDebug.Log("Error Importing Branches");
        }
    }

    public static bool importCommits(string path)
    {
        string commitsJSON = System.IO.File.ReadAllText(path + System.IO.Path.DirectorySeparatorChar + "commits.json");
        try
        {
            Main.commits = JsonUtility.FromJson<DBCommits>("{\"commits\":" + commitsJSON + "}");
            RuntimeDebug.Log("Commits Imported Sucessfull");
            return true;
        }
        catch (System.Exception ex)
        {
            return false;
            RuntimeDebug.Log("Error Importing Commits");
        }
    }

    public static bool importCommitsFilesRelation(string path)
    {
        string commitsFilesJSON = System.IO.File.ReadAllText(path + System.IO.Path.DirectorySeparatorChar + "commits-files.json");
        try
        {
            Main.commitsFiles = JsonUtility.FromJson<DBCommitsFiles>("{\"commitsFiles\":" + commitsFilesJSON + "}");
            RuntimeDebug.Log("Commits-Files Realtion Imported Sucessfull");
            return true;
        }
        catch (System.Exception ex)
        {
            return false;
            RuntimeDebug.Log("Error Importing Commits-Files Realtion ");
        }
    }

    public static bool importCommitsStakeholdersRelation(string path)
    {
        string commitsStakeholdersJSON = System.IO.File.ReadAllText(path + System.IO.Path.DirectorySeparatorChar + "commits-users.json");
        try
        {
            Main.commitsStakeholders = JsonUtility.FromJson<DBCommitsStakeholders>("{\"commitsStakeholders\":" + commitsStakeholdersJSON + "}");
            RuntimeDebug.Log("Commits-Stkeholders Realtion Imported Sucessfull");
            return true;
        }
        catch (System.Exception ex)
        {
            return false;
            RuntimeDebug.Log("Error Importing Commits-Stakeholders Realtion ");
        }
    }

    public static bool importCommitsCommitsRelation(string path)
    {
        string commitsCommitsJSON = System.IO.File.ReadAllText(path + System.IO.Path.DirectorySeparatorChar + "commits-commits.json");
        try
        {
            Main.commitsCommits = JsonUtility.FromJson<DBCommitsCommits>("{\"commitsCommits\":" + commitsCommitsJSON + "}");
            RuntimeDebug.Log("Commits-Commits Realtion Imported Sucessfull");
            return true;
        }
        catch (System.Exception ex)
        {
            return false;
            RuntimeDebug.Log("Error Importing Commits-Commits Realtion ");
        }
    }


    public static bool importCommitsFilesStakeholdersRelation(string path)
    {
        string commitsFilesStakeholdersJSON = System.IO.File.ReadAllText(path + System.IO.Path.DirectorySeparatorChar + "commits-files-users.json");
        try
        {
            Main.commitsFilesStakeholders = JsonUtility.FromJson<DBCommitsFilesStakeholders>("{\"commitsFilesStakeholders\":" + commitsFilesStakeholdersJSON + "}");
            RuntimeDebug.Log("Commits-Files-Stakeholders Realtion Imported Sucessfull");
            return true;
        }
        catch (System.Exception ex)
        {
            return false;
            RuntimeDebug.Log("Error Importing Commits-Files-Stakeholders Realtion ");
        }
    }

    public static bool importFiles(string path)
    {
        string filesJSON = System.IO.File.ReadAllText(path + System.IO.Path.DirectorySeparatorChar + "files.json");
        try
        {
            Main.files = JsonUtility.FromJson<DBFiles>("{\"files\":" + filesJSON + "}");
            RuntimeDebug.Log("Files Imported Sucessfull");
            return true;
        }
        catch (System.Exception ex)
        {
            return false;
            RuntimeDebug.Log("Error Importing Files");
        }
    }

    public static bool importStakeholders(string path)
    {
        string stakeholdersJSON = System.IO.File.ReadAllText(path + System.IO.Path.DirectorySeparatorChar + "users.json");
        try
        {
            Main.stakeholders = JsonUtility.FromJson<DBStakeholders>("{\"stakeholders\":" + stakeholdersJSON + "}");
            RuntimeDebug.Log("Stakeholders Imported Sucessfull");
            return true;
        }
        catch (System.Exception ex)
        {
            return false;
            RuntimeDebug.Log("Error Importing Stakeholders");
        }
    }
}
