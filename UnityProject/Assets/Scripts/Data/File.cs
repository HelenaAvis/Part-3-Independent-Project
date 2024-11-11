using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class File
{
    static System.Random rng = new System.Random();

    public enum FileType
    {   
        txt,
        xml,
        csv,
        pdf,
        exe
    }

    public Asset storedOn;
    public string filename;
    public FileType fileType;
    public bool hostile;

    public File (Asset storedOn, string filename, FileType fileType, bool hostile) 
    {
        this.storedOn = storedOn;
        this.filename = filename + "." + fileType.ToString();
        this.fileType = fileType;
        this.hostile = hostile;
    }

    /// <summary>
    /// Generates a random filename of the specified length.
    /// </summary>
    /// <param name="length">The length of the filename to generate.</param>
    /// <returns>The generated filename.</returns>
    public static string GenerateFileName(int length)
    {
        string a = "abcdefghijklmnopqrstuvwxyz1234567890";
        string filename = "";

        for (int i = 0; i < length; i++)
        {
            filename += a[rng.Next(a.Length)];
        }

        return filename;
    }
}
