using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseLoader : MonoBehaviour
{
    public static bool checkFoolderIfValid(string path)
    {
        if (!System.IO.File.Exists(path+"/branches.json")
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
        if (!importCommits(path)|| !importBranches(path))
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
}
