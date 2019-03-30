using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Scripting;

namespace UGF.Code.Analysis.Editor.Tests
{
    
    
    /// <summary>
    /// 
    /// </summary>
    public class TestTarget
    {
        public Type Type { get; set; }
    }
    
    
    
    
    
    /// <summary>
    /// 
    /// </summary>
    [Preserve]
    public class TestTarget1
    {
        public ICollection Collection { get; set; }
    }
    
    
    
    
    /// <summary>
    /// 
    /// </summary>
    public class TestTarget2
    {
        public Vector2 Vector2 { get; set; }
    }
}
