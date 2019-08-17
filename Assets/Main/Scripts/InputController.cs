using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRoyale
{
    [System.Serializable]
    public class InputController
    {
        public List<InputKey> list_Keys = new List<InputKey>();

        private Dictionary<string, InputKey> dict_MappedKeys = new Dictionary<string, InputKey>();

        bool bl_Initialized = false;

        private void Initialize()
        {
            if(bl_Initialized)
            {
                return;
            }
            foreach (InputKey _Key in list_Keys)
            {
                if (!dict_MappedKeys.ContainsKey(_Key.str_Name))
                {
                    dict_MappedKeys.Add(_Key.str_Name, _Key);
                }
            }
        }

        public bool bl_Check(string str_Name)
        {
            if (!dict_MappedKeys.ContainsKey(str_Name))
            {
                return false;
            }
            return dict_MappedKeys[str_Name].bl_Check();
        }

        public float fl_Check(string str_Name)
        {
            if (!dict_MappedKeys.ContainsKey(str_Name))
            {
                return 0f;
            }
            return dict_MappedKeys[str_Name].fl_Check();
        }

        // Update is called once per frame
        public void Update()
        {
            Initialize();

            foreach (InputKey _Key in list_Keys)
                _Key.Update();
        }
    }

    public enum InputType
    {
        Down,
        Up,
        Release,
        Pressed
    }

    [System.Serializable]
    public class InputKey
    {
        public string str_Name;
        public InputType _Type;
        public float fl_Sensibility = 0.1f;
        private float fl_Act;
        private float fl_Prev;

        public bool bl_Check()
        {
            switch (_Type)
            {
                case InputType.Down:
                    return fl_Act > fl_Sensibility;

                case InputType.Up:
                    return fl_Act <= fl_Sensibility;

                case InputType.Release:
                    return fl_Act < fl_Prev;

                case InputType.Pressed:
                    return fl_Act > fl_Prev;

                default:
                    return false;
            }
        }

        public float fl_Check()
        {
            return fl_Act;
        }

        public void Update()
        {
            fl_Prev = fl_Act;
            fl_Act = Input.GetAxis(str_Name);
        }
    }
}
