using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class Room : ISerializationCallbackReceiver
    {
        public int[,] Grid;
        public int Height, Width;
        public int Designation;

        // Unity's serializer is annoying: https://forum.unity.com/threads/how-i-serialize-multidimensional-arrays.988704/
        [SerializeField, HideInInspector] private List<SerializablePackage<int>> serializable;

        [Serializable]
        struct SerializablePackage<TVal>
        {
            public int X;
            public int Y;
            public TVal Value;
            public SerializablePackage(int x, int y, TVal val)
            {
                X = x;
                Y = y;
                Value = val;
            }
        }
        public void OnBeforeSerialize()
        {
            serializable = new List<SerializablePackage<int>>();
            for (int i = 0; i < Grid.GetLength(0); i++) {
                for (int j = 0; j < Grid.GetLength(1); j++) {
                    serializable.Add(new SerializablePackage<int>(i, j, Grid[i, j]));
                }
            }
        }

        public void OnAfterDeserialize()
        {
            Grid = new int[Width, Height];
            foreach (var package in serializable) {
                Grid[package.X, package.Y] = package.Value;
            }
        }
    }
}
