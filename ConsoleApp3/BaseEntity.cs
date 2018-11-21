using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp3
{

    public interface IBaseEntity
    {
        Guid Id { get; set; }
    }
    public abstract class BaseEntity:IBaseEntity
    {

        [Keyword]

        public Guid Id { get; set; }
    }

    
}
