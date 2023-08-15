using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseLoader : MonoBehaviour
{
    public static bool checkFoolderIfValid(string path)
    {
        if (!System.IO.File.Exists(path + "/branches.json")
            || !System.IO.File.Exists(path + "/builds.json")
            || !System.IO.File.Exists(path + "/commits-commits.json")
            || !System.IO.File.Exists(path + "/commits-files.json")
            || !System.IO.File.Exists(path + "/commits-languages.json")
            || !System.IO.File.Exists(path + "/commits-modules.json")
            || !System.IO.File.Exists(path + "/commits-stakeholders.json")
            || !System.IO.File.Exists(path + "/commits.json")
            || !System.IO.File.Exists(path + "/files.json")
            || !System.IO.File.Exists(path + "/issues-commits.json")
            || !System.IO.File.Exists(path + "/issues-stakeholders.json")
            || !System.IO.File.Exists(path + "/issues.json")
            || !System.IO.File.Exists(path + "/languages-files.json")
            || !System.IO.File.Exists(path + "/languages.json")
            || !System.IO.File.Exists(path + "/modules-files.json")
            || !System.IO.File.Exists(path + "/modules-modules.json")
            || !System.IO.File.Exists(path + "/modules.json")
            || !System.IO.File.Exists(path + "/stakeholders.json"))
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
        string branchesJSON = System.IO.File.ReadAllText(path + "/branches.json");
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
        string commitsJSON = System.IO.File.ReadAllText(path + "/commits.json");
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
        string commitsFilesJSON = System.IO.File.ReadAllText(path + "/commits-files.json");
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

    public static bool importCommitsFilesStakeholdersRelation(string path)
    {
        string commitsFilesStakeholdersJSON = System.IO.File.ReadAllText(path + "/commits-files-stakeholders.json");
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
        string filesJSON = System.IO.File.ReadAllText(path + "/files.json");
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
        string stakeholdersJSON = System.IO.File.ReadAllText(path + "/stakeholders.json");
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
