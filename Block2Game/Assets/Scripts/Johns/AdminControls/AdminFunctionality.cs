using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;
using TMPro;
public class AdminFunctionality : MonoBehaviour
{
    private Encryption _Encrypt;
    [SerializeField] TMP_InputField _Username;
    [SerializeField] TMP_InputField _Password;
    [SerializeField] Button _TrackButton;
    private void Awake()
    {
        _Password.contentType = TMP_InputField.ContentType.Password;
        _Encrypt = new Encryption();
    }

    public static void SavePassword(string username, string hashedPassword, byte[] salt)
    {
        // Create a new object to store the data
        var passwordData = new PasswordData { username = username, hashedPassword = hashedPassword, salt = Convert.ToBase64String(salt) };

        // Convert the object to a JSON string
        string json = JsonUtility.ToJson(passwordData);

        // Write the JSON string to a file
        File.WriteAllText(Application.persistentDataPath + "/admin.json", json);
    }
    public static (string, string, byte[]) LoadPassword()
    {
        // Read the contents of the file into a string
        string json = File.ReadAllText(Application.persistentDataPath + "/admin.json");

        // Convert the JSON string to an object
        var passwordData = JsonUtility.FromJson<PasswordData>(json);

        // Extract the data from the object
        string username = passwordData.username;
        string hashedPassword = passwordData.hashedPassword;
        byte[] salt = Convert.FromBase64String(passwordData.salt);

        // Return the data
        return (username, hashedPassword, salt);
    }
    public void ReadPas()
    {
        string _UsernameVar = _Username.text;
        string _PasswordVar = _Password.text;

        string persistentPath = Application.persistentDataPath + "/admin.json";
        if (File.Exists(persistentPath))
        {
            var test = LoadPassword();
            if (_UsernameVar == test.Item1 && _Encrypt.ValidatePassword(_PasswordVar, test.Item2, test.Item3))
            {
                _TrackButton.gameObject.SetActive(true);
            }
            
        }       
       
    }
    public void CreatePass()
    {
        string persistentPath = Application.persistentDataPath + "/admin.json";
        if (File.Exists(persistentPath))
        {
            return;
        }
       
        string _UsernameVar = _Username.text;
        string _PasswordVar = _Password.text;
        var _Salt = _Encrypt.CreateSalt();
        var pass = _Encrypt.HashPassword(_PasswordVar, _Salt);

        SavePassword(_UsernameVar, pass, _Salt);
    }
    [Serializable]
    public class PasswordData
    {
        public string username;
        public string hashedPassword;
        public string salt;
    }
}
