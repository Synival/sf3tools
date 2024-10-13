using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF3.Models
{
    public abstract class ModelArray : IModelArray
    {
        public abstract bool Load();
    }

    public abstract class ModelArray<T> : ModelArray
    {
    }
}
