using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal sealed class LinkHead : SingleMonoBehaviour<LinkHead>
{


    // Global methods and fields?
    internal List<SandboxCall> sandboxCalls = new List<SandboxCall>();
    internal List<SandboxEvent> sandboxEvents = new List<SandboxEvent>();
    internal List<SandboxValue> sandboxValues = new List<SandboxValue>();

    protected override bool isPersistant => true;

    internal class Values
    {
        internal int teamAScore;
        internal int teamBScore;
        internal int teamCScore;
        internal int teamDScore;
    }
    internal Values values = new Values();

    internal void ResetAll()
    {
        values = new Values();
    }

    internal Action onTest;

    internal void TestGlobalCall()
    {

    }
}