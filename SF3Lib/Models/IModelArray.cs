﻿using SF3.Types;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace SF3.Models
{
    public interface IModelArray<T>
    {
        /// <summary>
        /// Loads models from its respective XML file(s).
        /// </summary>
        /// <returns>Return 'true' on success, or 'false' if the .XML file(s) do not exist or are in use.</returns>
        bool Load();

        /// <summary>
        /// A mutable array of models of type T.
        /// </summary>
        T[] Models { get; }
    }
}
