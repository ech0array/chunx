using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
sealed internal class UserData
{
    [Serializable]
    internal class UserSettingsData
    {
        internal bool invertYLook;
        internal bool allowVibration;

        #region Schemes
        internal string pcAxisInputScheme;
        internal string pcButtonInputScheme;

        internal string touchAxisInputScheme;
        internal string touchButtonInputScheme;

        internal string nSwitchAxisInputScheme;
        internal string nSwitchButtonInputScheme;


        internal string xboxOneAxisInputScheme;
        internal string xboxOneButtonInputScheme;

        internal string playstation4AxisInputScheme;
        internal string playstation4ButtonInputScheme;
        #endregion
    }
    [SerializeField]
    internal UserSettingsData settingsData = new UserSettingsData();

    [SerializeField]
    internal List<UniverseData> universeDatas = new List<UniverseData>();
}