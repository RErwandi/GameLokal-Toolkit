using System;

namespace GameLokal.Toolkit
{
    public interface IGameSave
    {
        string GetUniqueName();
        object GetSaveData();
        Type GetSaveDataType();
        void ResetData();
        void OnLoad(object generic);
    }
}