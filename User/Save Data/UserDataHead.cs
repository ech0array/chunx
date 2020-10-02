using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static internal class UserDataHead
{
    internal static void Save(User user)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {

        }
        else if (Application.platform == RuntimePlatform.Android)
        {

        }
        else if (Application.platform == RuntimePlatform.Switch)
        {

        }
        else if (Application.platform == RuntimePlatform.XboxOne)
        {

        }
        else if (Application.platform == RuntimePlatform.PS4)
        {

        }

        GenericUserDataNode.Save(user);
    }
    internal static UserData Load(User user)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {

        }
        else if (Application.platform == RuntimePlatform.Android)
        {

        }
        else if (Application.platform == RuntimePlatform.Switch)
        {

        }
        else if (Application.platform == RuntimePlatform.XboxOne)
        {

        }
        else if (Application.platform == RuntimePlatform.PS4)
        {

        }

        return GenericUserDataNode.Load(user);
    }
}