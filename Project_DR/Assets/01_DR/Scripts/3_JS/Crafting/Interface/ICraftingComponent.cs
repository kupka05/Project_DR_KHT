using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Crafting
{
    public interface ICraftingComponent
    {
        bool CheckCraft();
        bool CheckEnhance();
        void Craft();
        void Enhance(int type);
    }
}
