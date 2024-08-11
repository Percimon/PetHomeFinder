using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetHomeFinder.Domain
{
    public class PetPhoto
    {
        public Guid Id { get; private set; }
        public string FilePath { get; private set; }
        public bool IsMain { get; private set; }
    }
}
