using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FunkySheep.Colors
{
  [CreateAssetMenu(menuName = "FunkySheep/Colors/List")]
  public class ColorList : ScriptableObject
  {
      public List<Color> Value;
  }
}