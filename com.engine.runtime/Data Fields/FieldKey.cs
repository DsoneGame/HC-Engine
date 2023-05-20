using UnityEngine;
using System;
using System.IO;

namespace Engine.Data
{
    [Serializable]
    public class FieldKey<T> : IField<T>
    {
        public static string FilePath(string fileName = "")
        {
            string directoryPath = Application.persistentDataPath + "/data/";

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            return directoryPath + fileName + ".json";
        }

        [SerializeField, HideInInspector] protected bool _autoSave;
        [SerializeField, HideInInspector] protected string _fileName;

        [SerializeField] protected string _key;
        [SerializeField] protected T _value;

        private bool _isLoaded;

        public string Key => _key;
        public string fileName => _fileName;
        public bool hasValue => ES3.KeyExists(_key, FilePath(_fileName));

        public FieldKey(string key, string fileName, T value = default(T), bool autoSave = true)
        {
            _isLoaded = false;
            _autoSave = autoSave;

            _key = key;
            _value = value;

            _fileName = fileName ?? throw new ArgumentNullException("The path file has a null value!.");
        }

        public virtual T value
        {
            get
            {
                if (_isLoaded == true) return _value;

                _isLoaded = true;
                return _value = ES3.Load(_key, FilePath(_fileName), _value);
            }
            set
            {
                
                if (_value == null || !_value.Equals(value))
                {
                    _value = value;
                    if (_autoSave == true)
                    {
                        
                        Save();
                    }
                    
                }
                
            }
        }

        public void Save()
        {
            ES3.Save(_key, _value, FilePath(_fileName));
        }

        public void Delete()
        {
            ES3.DeleteKey(_key, FilePath(_fileName));
        }
    }
}