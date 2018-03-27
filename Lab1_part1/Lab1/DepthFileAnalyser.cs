using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Lab1
{
    class DepthFileAnalyser
    {
        string _path;
        int _length;
        double _enthrophy;
        Dictionary<char, double> _alphabet = new Dictionary<char, double>();
        double _informationCount;
        public DepthFileAnalyser(string path)
        {
            _path = path;
            ReadFile();
            _alphabet=_alphabet.Select(elem => new KeyValuePair<char, double>(elem.Key, 1.0 * elem.Value / _length))
                .OrderBy(elem => elem.Key)
                .ToDictionary(elem => elem.Key, elem => elem.Value);
            foreach(var item in _alphabet)
            {
                _enthrophy += item.Value* Math.Log(item.Value, 2);
            }
            _enthrophy *= -1;
            _informationCount = Math.Ceiling(_enthrophy * _length / 8);
        }
        public double Enthrophy
        {
            get { return _enthrophy; }
        }
        public double InformationCount
        {
            get { return _informationCount; }
        }
        public int Lenght
        {
            get { return _length; }
        }
        public string GetFrequency()
        {
            string result = "";
            foreach (var item in _alphabet)
            {
                result += item.Key.ToString() + " --> " + item.Value.ToString() + "\r\n";
            }
            return result;
        }
        private void ReadFile()
        {
            using (StreamReader st = new StreamReader(_path, Encoding.Default))
            {
                char s;
                while (!st.EndOfStream)
                {
                    s = (char)st.Read();
                    if (_alphabet.Keys.Contains(s))
                        _alphabet[s]++;
                    else
                        _alphabet.Add(s, 1);
                    _length++;
                }
            }
        }
        public override string ToString()
        {
            return GetFrequency() + "Середня ентропія: " + _enthrophy.ToString() + 
                "\nК-сть інформації в тексті: " + _informationCount.ToString() + "\nРозмір файлу: "+_length.ToString();
        }

    }
}
