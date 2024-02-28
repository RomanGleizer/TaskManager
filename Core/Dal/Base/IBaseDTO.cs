using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dal.Base;

public interface IBaseDTO<T>
{
    T Id { get; set; }
}
