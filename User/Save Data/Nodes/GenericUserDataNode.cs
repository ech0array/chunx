
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

static internal class GenericUserDataNode
{
    private static string GetFilePath(string userId)
    {
        const string fileName = "user_data_state.bin";
        return GetDirectory(userId) + "/" + fileName;
    }
    private static string GetDirectory(string userId)
    {
        return Application.persistentDataPath + "/" + userId;
    }

    private static UserData CreateNewSave(User user)
    {
        UserData userDataState = new UserData();
        Save(user.userId, userDataState);
        return userDataState;
    }
    internal static void Save(User user)
    {
        Save(user.userId, user.userData);
    }
    internal static void Save(string userId, UserData userDataState)
    {
        string savePath = GetDirectory(userId);

        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Create(GetFilePath(userId));
        binaryFormatter.Serialize(file, userDataState);
        file.Close();
    }

    internal static UserData Load(User user)
    {
        string savePath = GetDirectory(user.userId);
        Debug.Log(savePath);

        if (!Directory.Exists(savePath) || !File.Exists(GetFilePath(user.userId)))
            return CreateNewSave(user);

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Open(GetFilePath(user.userId), FileMode.Open);
        UserData userData = (UserData)binaryFormatter.Deserialize(file);
        file.Close();
        return userData;
    }
}