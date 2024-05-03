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
    [SerializeField] Button _ExtractData;
    [SerializeField] Button _ResetData;
    [SerializeField] GameObject _AdminField;
    private void Awake()
    {
        _Password.contentType = TMP_InputField.ContentType.Password;
        _Encrypt = new Encryption();
    }

    public static void SavePassword(string username, string hashedPassword, byte[] salt, byte[] key, byte[] iv)
    {
        // Create a new object to store the data
        var passwordData = new PasswordData { username = username, hashedPassword = hashedPassword, salt = Convert.ToBase64String(salt), key = Convert.ToBase64String(key), iv = Convert.ToBase64String(iv) };

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
        // Get the entered username and password
        string _UsernameVar = _Username.text;
        string _PasswordVar = _Password.text;

        // Create the file path for the "admin.json" file
        string persistentPath = Application.persistentDataPath + "/admin.json";

        // Check if the "admin.json" file exists
        if (File.Exists(persistentPath))
        {
            // Load the stored username and encrypted password from the file
            var test = LoadPassword();

            // Compare the entered username and password to the ones stored in the file
            if (_UsernameVar == test.Item1 && _Encrypt.ValidatePassword(_PasswordVar, test.Item2, test.Item3))
            {
                // If the entered values match the stored values, activate the "TrackButton" game object
                _TrackButton.gameObject.SetActive(true);

                // and the "AdminField" game object
                _AdminField.gameObject.SetActive(true);
            }
        }
    }
    public void DecryptList()
    {
        // Create the file path for the "DataAdmin.json" file on the desktop
        string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/DataAdmin.json";
        // Check if "DataAdmin.json" file exists, if yes delete it
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        // Create the file path for the "Stats.json" file in the Application's persistent data path
        string dataPath = Application.persistentDataPath + "/Stats.json";
        // Check if the "Stats.json" file exists, if not return
        if (!File.Exists(dataPath))
        {
            return;
        }
        // Read the contents of the "Stats.json" file
        string json = File.ReadAllText(Application.persistentDataPath + "/Stats.json");
        // Convert the JSON string to a Score object
        var emailData = JsonHelper.FromJson<Score>(json);
        // Read the contents of the "admin.json" file
        string jsonAdmin = File.ReadAllText(Application.persistentDataPath + "/admin.json");
        // Convert the JSON string to a PasswordData object
        var passwordDataAdmin = JsonUtility.FromJson<PasswordData>(jsonAdmin);
        // Get the encryption key and initialization vector from the PasswordData object
        var key = Convert.FromBase64String(passwordDataAdmin.key);
        var iv = Convert.FromBase64String(passwordDataAdmin.iv);
        // Create an array to store the decrypted data
        Data[] _SaveScores = new Data[emailData.Length];
        // Loop through the email data and decrypt the email addresses
        for (int i = 0; i < emailData.Length; i++)
        {
            var email = emailData[i]._HashedEmail;
            var decrypt = _Encrypt.DecryptEmail(email, key, iv);
            // Create a new Data object with the decrypted email address
            Data _pointAdd = new Data { Name = emailData[i]._Name, LapTime = emailData[i]._LapTime, Email = decrypt };
            _SaveScores[i] = _pointAdd;
        }
        // Convert the decrypted data to a JSON string
        string jsonData = JsonHelper.ToJson(_SaveScores, true);
        // Write the decrypted data to the "DataAdmin.json" file on the desktop
        File.WriteAllText(path, jsonData);

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

        var key = _Encrypt.CreateKey();
        SavePassword(_UsernameVar, pass, _Salt, key.Item1, key.Item2);
    }

    public void ResetData()
    {
        string persistentPath = Application.persistentDataPath + "/Stats.json";
        if (File.Exists(persistentPath))
        {

            File.Delete(persistentPath);

        }
    }
    [Serializable]
    public class PasswordData
    {
        public string username;
        public string hashedPassword;
        public string salt;

        public string key;
        public string iv;
    }

    [System.Serializable]
    public struct Score
    {
        public string _Name;
        public float _LapTime;
        public string _HashedEmail;

    }
    [System.Serializable]
    public struct Data
    {
        public string Name;
        public float LapTime;
        public string Email;

    }

    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }
        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }


        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }
}
