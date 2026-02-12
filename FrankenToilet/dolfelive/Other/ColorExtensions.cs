using UnityEngine;

namespace FrankenToilet.dolfelive;

public static class ColorExtensions
{
    extension(Color col)
    {
        public string ToHexString()
        {
            return
                ((byte)(col.r * 255)).ToString("X2") +
                ((byte)(col.g * 255)).ToString("X2") +
                ((byte)(col.b * 255)).ToString("X2") +
                ((byte)(col.a * 255)).ToString("X2");
        }
    }
}
